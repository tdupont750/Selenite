using System;
using System.Collections.Generic;
using Selenite.Commands;
using Selenite.Models;

namespace Selenite.Services
{
    public interface ICommandService
    {
        IEnumerable<Tuple<string, string>> GetCommandNames();
        IDictionary<string, string> GetCommandValues(ICommand command);
        IList<Tuple<string, string>> GetCommandProperties(string commandName);
        ICommand CreateCommand(string name, IDictionary<string, string> values, SeleniteTest test);
        ICommand CreateCommand(dynamic command, SeleniteTest test);
    }
}