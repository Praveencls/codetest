using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeTest.Feature.Container.DI
{
    using CodeTest.Feature.Container.Repositories;
    using Controllers;
    // using Controllers;
    using Microsoft.Extensions.DependencyInjection;
    // using Repositories.Footer;
    using Sitecore.DependencyInjection;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IContainerRepository, ContainerRepository>();
            serviceCollection.AddTransient<ContainerController>();
        }
    }
}