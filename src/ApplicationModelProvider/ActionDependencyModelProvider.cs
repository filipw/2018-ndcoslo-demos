using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApplicationModelProvider
{
    public class ActionDependencyModelProvider : IApplicationModelProvider
    {
        public int Order => -901;

        public void OnProvidersExecuted(ApplicationModelProviderContext context)
        {
        }

        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (var controllerModel in context.Result.Controllers)
            {
                foreach (var actionModel in controllerModel.Actions)
                {
                    foreach (var parameterModel in actionModel.Parameters)
                    {
                        if (parameterModel.ParameterType.IsInterface)
                        {
                            parameterModel.BindingInfo = new BindingInfo() { BindingSource = BindingSource.Services };
                        }
                    }
                }
            }
        }
    }
}
