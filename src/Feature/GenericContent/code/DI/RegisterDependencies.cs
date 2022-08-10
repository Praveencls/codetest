

using CodeTest.Feature.GenericCallout.Controllers;

namespace CodeTest.Feature.GenericCallout.DI
{
   
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<AlternateCalloutController>();
        }
    }
}