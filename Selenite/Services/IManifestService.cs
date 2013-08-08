using System.Collections.Generic;
using Selenite.Models;

namespace Selenite.Services
{
    public interface IManifestService
    {
        IList<string> GetManifestNames();
        string GetActiveManifestName();
        Manifest GetManifest(string manifestName);
        void SaveManifest(Manifest manifest);
        void SetActiveManifest(string manifestName);
        void SetActiveManifestDomain(string domainOverride);
        void ReloadManifest();
    }
}