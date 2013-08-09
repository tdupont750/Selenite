using System.Collections.Generic;
using Selenite.Commands;
using Selenite.Models;

namespace Selenite.Services
{
    public interface ICommandService
    {
        ICollection<string> GetCommandNames();
        IDictionary<string, string> GetCommandValues(ICommand command);
        IList<string> GetCommandProperties(string commandName);
        ICommand CreateCommand(string name, IDictionary<string, string> values, Test test);
        ICommand CreateCommand(dynamic command, Test test);
    }
}