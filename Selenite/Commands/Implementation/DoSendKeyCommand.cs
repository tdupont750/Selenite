using System;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;
using Selenite.Commands.Base;

namespace Selenite.Commands.Implementation
{
    /// <summary>
    /// Sends a single keystroke to the selected element.  Requires an element to be selected.
    /// This command is used to send the non-text commands.  For a full list see the Key property.
    /// </summary>
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

        /// <summary>
        /// The Key to send to the selected element.  This parameter is required.
        /// Possible options include:
        /// ADD, ALT, ARROW_DOWN, ARROW_LEFT, ARROW_RIGHT, ARROW_UP, BACK_SPACE, CANCEL, CLEAR, COMMAND, CONTROL, DECIMAL
        /// DELETE, DIVIDE, DOWN, END, ENTER, EQUALS, ESCAPE, F1, F2, F3 ... F10, F11, F12, HELP, HOME, INSERT, LEFT, LEFT_ALT
        /// LEFT_CONTROL, LEFT_SHIFT, META, MULTIPLY, NULL, NUMPAD0, NUMPAD1, NUMPAD2 ... NUMPAD9, PAGE_DOWN, PAGE_UP, PAUSE, RETURN,
        /// RIGHT, SEMICOLON, SEPARATOR, SHIFT, SPACE, SUBTRACT, TAB, UP
        /// </summary>
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