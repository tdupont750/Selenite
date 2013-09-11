using System;
using System.Collections.Generic;
using Selenite.Services;
using Selenite.Services.Implementation;

namespace Selenite.Global
{
    public static class ServiceResolver
    {
        private static readonly Lazy<IDictionary<Type, object>> DefaultServiceMap = new Lazy<IDictionary<Type, object>>(InitializeDefaultServiceMap);

        private static Func<Type, object> _resolver = DefaultResolver;
        
        public static T Get<T>()
        {
            return (T) Get(typeof(T));
        }

        public static object Get(Type type)
        {
            var result = _resolver(type);

            if (result == null)
                throw new NullReferenceException("Unable to resolve type: " + type.FullName);

            return result;
        }

        public static void Register<T>(T instance)
        {
            DefaultServiceMap.Value.Add(typeof(T), instance);
        }

        public static void SetResolver(Func<Type, object> resolver)
        {
            _resolver = resolver;
        }

        private static object DefaultResolver(Type type)
        {
            // DriverFactory is Transient
            if (typeof (IDriverFactory) == type)
                return new DriverFactory();

            return DefaultServiceMap.Value[type];
        }

        private static IDictionary<Type, object> InitializeDefaultServiceMap()
        {
            var configurationService = new ConfigurationService();
            var commandService = new CommandService();
            var fileService = new FileService();
            var manifestService = new ManifestService(configurationService, fileService);
            var testCollectionService = new TestCollectionService(configurationService, fileService, commandService, manifestService);
            var testService = new TestService();
            var seleniteDataService = new SeleniteDataService(testCollectionService, manifestService);

            return new Dictionary<Type, object>
            {
                { typeof(IConfigurationService), configurationService },
                { typeof(ICommandService), commandService },
                { typeof(IFileService), fileService },
                { typeof(ITestCollectionService), testCollectionService },
                { typeof(IManifestService), manifestService },
                { typeof(ITestService), testService },
                { typeof(ISeleniteDataService), seleniteDataService }
            };
        }
    }
}
