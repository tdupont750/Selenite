using System.Collections.Generic;
using Selenite.Commands;

namespace Selenite.Services
{
    public interface ICommandService
    {
        ICollection<string> GetCommandNames();
        IDictionary<string, string> GetCommandValues(ICommand command);
        IList<string> GetCommandProperties(string commandName);
        ICommand CreateCommand(string name, IDictionary<string, string> values);
        ICommand CreateCommand(dynamic command);
    }
}