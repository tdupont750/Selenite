using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Selenite.Commands;
using Selenite.Commands.Base;
using Selenite.Extensions;

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

        public ICollection<string> GetCommandNames()
        {
            return CommandTypeMap.Value.Keys;
        }

        public IDictionary<string, string> GetCommandValues(ICommand command)
        {
            var result = new Dictionary<string, string>();

            var type = CommandTypeMap.Value[command.Name];
            var properties = GetProperties(type);

            foreach (var property in properties)
                result[property.Name] = property
                    .GetValue(command, null)
                    .ToString();

            return result;
        }

        public IList<string> GetCommandProperties(string commandName)
        {
            var type = CommandTypeMap.Value[commandName];
            var properties = GetProperties(type);

            return properties
                .Select(p => p.Name)
                .ToList();
        }

        public ICommand CreateCommand(string name, IDictionary<string, string> values)
        {
            dynamic command = new {Name = name};

            foreach (var value in values)
                command[value.Key] = value.Value;

            return CreateCommand(command);
        }

        public ICommand CreateCommand(dynamic command)
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

            result.Validate();

            return (CommandBase) result;
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            return type
                .GetProperties()
                .Where(p => p.CanWrite);
        }
    }
}