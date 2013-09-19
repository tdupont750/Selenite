using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Common.Extensions
{
    public static class ICommandExtensions
    {
        public static void VerifyAndExecute(this ICommand command, object parameter)
        {
            if (command != null && command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }

        public static void RaiseCanExecuteChanged(this ICommand command)
        {
            var delegateCommand = command as DelegateCommand;

            if (delegateCommand != null)
            {
                delegateCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
