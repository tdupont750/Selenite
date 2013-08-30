using System;
using System.Collections.Generic;
using System.IO;

namespace Selenite.Services.Implementation
{
    public class FileService : IFileService
    {
        private const string FileUriPrefix = "file:///";

        public IList<string> GetFiles(string path, string searchPattern)
        {
            if (path.StartsWith(FileUriPrefix, StringComparison.OrdinalIgnoreCase))
                path = path.Substring(FileUriPrefix.Length);
            return Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
        }

        public string ReadAllText(string path)
        {
            if (path.StartsWith(FileUriPrefix, StringComparison.OrdinalIgnoreCase))
                path = path.Substring(FileUriPrefix.Length);
            return File.ReadAllText(path);
        }

        public void WriteAllText(string path, string contents)
        {
            if (path.StartsWith(FileUriPrefix, StringComparison.OrdinalIgnoreCase))
                path = path.Substring(FileUriPrefix.Length);
            File.WriteAllText(path, contents);
        }
    }
}