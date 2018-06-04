using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace ParallelPipelines
{
    public static class MvcCoreBuilderExtensions
    {
        public static IMvcBuilder AddSpecificControllersOnly<T>(this IMvcBuilder builder)
        {
            return builder.ConfigureApplicationPartManager(manager =>
            {
                manager.ApplicationParts.Clear();
                manager.ApplicationParts.Add(new AssemblyPart(typeof(T).Assembly));
            });
        }
    }
}
