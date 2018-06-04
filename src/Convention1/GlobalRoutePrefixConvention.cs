using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Linq;

namespace Convention1
{
    public class GlobalRoutePrefixConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centralPrefix;

        public GlobalRoutePrefixConvention(string routeTemplate)
        {
            _centralPrefix = new AttributeRouteModel(new RouteAttribute(routeTemplate));
        }

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                foreach (var selectorModel in controller.Selectors)
                {
                    selectorModel.AttributeRouteModel = selectorModel.AttributeRouteModel != null
                        ? AttributeRouteModel.CombineAttributeRouteModel(_centralPrefix, selectorModel.AttributeRouteModel)
                        : _centralPrefix;
                }
            }
        }
    }
}
