using System;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    public class DoSendKeyCommand : SingleSelectorCommandBase
    {
        private static readonly MemberInfo[] KeyMembers;

        static DoSendKeyCommand()
        {
            KeyMembers = typeof(Keys)
                .GetMembers()
                .Where(m => m.MemberType == MemberTypes.Field)
                .ToArray();
        }
        
        public string Key { get; set; }

        protected override void Execute(IWebDriver driver, dynamic context, IWebElement element)
        {
            var keyValue = GetKeyValue();
            element.SendKeys(keyValue);
        }

        public override void Validate()
        {
            GetKeyValue();
        }

        private string GetKeyValue()
        {
            var field = KeyMembers.FirstOrDefault(m => m.Name.Equals(Key, StringComparison.InvariantCultureIgnoreCase)) as FieldInfo;

            if (field == null)
                throw new ArgumentException();
            
            return field
                .GetValue(null)
                .ToString();
        }
    }
}