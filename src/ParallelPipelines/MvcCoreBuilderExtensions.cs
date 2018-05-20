using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ParallelPipelines
{
    public static class MvcCoreBuilderExtensions
    {
        public static IMvcBuilder AddSpecificControllersOnly<TController>(this IMvcBuilder builder) where TController : ControllerBase
        {
            return builder.ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Clear();
                manager.FeatureProviders.Add(new TypedControllerFeatureProvider<TController>());
            }).AddApplicationPart(typeof(TController).Assembly);
        }

        public static IMvcCoreBuilder AddSpecificControllersOnly<TController>(this IMvcCoreBuilder builder) where TController : ControllerBase
        {
            return builder.ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Clear();
                manager.FeatureProviders.Add(new TypedControllerFeatureProvider<TController>());
            }).AddApplicationPart(typeof(TController).Assembly);
        }
    }
}
