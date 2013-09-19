using System.Windows.Input;

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
    }
}
