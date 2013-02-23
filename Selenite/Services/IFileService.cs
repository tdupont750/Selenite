using System.Collections.Generic;

namespace Selenite.Services
{
    public interface IFileService
    {
        IList<string> GetFiles(string path, string searchPattern);
        string ReadAllText(string path);
        void WriteAllText(string path, string contents);
    }
}