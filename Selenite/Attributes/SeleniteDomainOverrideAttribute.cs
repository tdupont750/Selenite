using System;

namespace Selenite
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class SeleniteDomainOverrideAttribute : Attribute
    {
        public virtual string DomainOverride
        {
            get { return _domainOverride; }
        }

        private readonly string _domainOverride;

        protected SeleniteDomainOverrideAttribute() 
        {
        }

        public SeleniteDomainOverrideAttribute(string domainOverride)
        {
            _domainOverride = domainOverride;
        }
    }
}