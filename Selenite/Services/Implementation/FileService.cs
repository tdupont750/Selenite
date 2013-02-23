using System.Collections.Generic;
using System.IO;

namespace Selenite.Services.Implementation
{
    public class FileService : IFileService
    {
        public IList<string> GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }
    }
}