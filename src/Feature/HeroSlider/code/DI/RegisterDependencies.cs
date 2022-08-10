namespace CodeTest.Feature.HeroSlider.DI
{
    using CodeTest.Feature.HeroSlider.Controllers;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<HeroSliderController>();
        }
    }
}