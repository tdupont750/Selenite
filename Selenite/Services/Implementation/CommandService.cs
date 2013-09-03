using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Selenite.Commands;
using Selenite.Commands.Base;
using Selenite.Models;

namespace Selenite.Services.Implementation
{
    public class CommandService : ICommandService
    {
        private static readonly Lazy<IDictionary<string, Type>> CommandTypeMap = new Lazy<IDictionary<string, Type>>(InitializeCommandTypeMap);

        private static IDictionary<string, Type> InitializeCommandTypeMap()
        {
            var commandTypes = AppDomain.CurrentDomain.GetTypesWithAttribute<CommandAttribute>();
            return commandTypes.ToDictionary(
                k => k.Item1.GetCommandName(), 
                v => v.Item1);
        }

        public IEnumerable<Tuple<string, string>> GetCommandNames()
        {
            return CommandTypeMap.Value
                .Select(command => new Tuple<string, string>(command.Key, GetTypeDescription(command.Value)));
        }

        private string GetTypeDescription(Type type)
        {
            var customAttributes = type.GetCustomAttributes<DescriptionAttribute>(false).ToList();
            return customAttributes.Any()
                ? customAttributes[0].Description
                : type.Name;
        }

        public IDictionary<string, string> GetCommandValues(ICommand command)
        {
            var result = new Dictionary<string, string>();

            var type = CommandTypeMap.Value[command.Name];
            var properties = GetProperties(type);

            foreach (var property in properties)
            {
                var value = property.GetValue(command, null);
                result[property.Name] = value == null
                    ? String.Empty
                    : value.ToString();
            }

            return result;
        }

        public IList<Tuple<string, string>> GetCommandProperties(string commandName)
        {
            var type = CommandTypeMap.Value[commandName];
            var properties = GetProperties(type);

            return properties
                .Select(p => new Tuple<string, string>(p.Name, GetPropertyDescription(p)))
                .ToList();
        }

        private string GetPropertyDescription(PropertyInfo property)
        {
            var customAttributes = property.GetCustomAttributes(typeof(DescriptionAttribute), false).ToList();
            return customAttributes.Any()
                ? ((DescriptionAttribute)customAttributes[0]).Description
                : property.Name;
        }

        public ICommand CreateCommand(string name, IDictionary<string, string> values, SeleniteTest test)
        {
            dynamic command = new {Name = name};

            foreach (var value in values)
                command[value.Key] = value.Value;

            return CreateCommand(command, test);
        }

        public ICommand CreateCommand(dynamic command, SeleniteTest test)
        {
            string name = command.Name;
            if (String.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Command name not specified");

            var type = CommandTypeMap.Value[name];
            if (type == null)
                throw new ArgumentException("Command not found: " + name);

            dynamic result = Activator.CreateInstance(type);

            var properties = GetProperties(type);

            foreach (var property in properties)
            {
                var pValue = command[property.Name];
                var jValue = pValue as JValue;
                var value = jValue == null 
                    ? (object) pValue 
                    : jValue.Value;

                if (value == null)
                    continue;

                var converted = Convert.ChangeType(value, property.PropertyType);
                property.InvokeSetMethod((object) result, converted);
            }

            var commandBase = (CommandBase)result;
            commandBase.Test = test;
            commandBase.Validate();

            return commandBase;
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type
                .GetProperties()
                .Where(p => p.CanWrite);
        }
    }
}