using System.Resources;
using Business.Challenges.Properties;
using Business.Challenges.ViewModels;
using Shared.Framework.Dependency;

namespace Business.Challenges.Private
{
    public class SourceCodeTemplateCollector : IDependency
    {
        private readonly ResourceManager resourceManager;
        private const string ResourceTemplate = "Answer_Template_{0}";

        public SourceCodeTemplateCollector()
        {
            resourceManager = new ResourceManager(typeof(Resources));
        }

        public string GetTemplate(BusinessSection section)
        {
            var resourceName = string.Format(ResourceTemplate, section);
            
            return resourceManager.GetString(resourceName);
        }
    }
}