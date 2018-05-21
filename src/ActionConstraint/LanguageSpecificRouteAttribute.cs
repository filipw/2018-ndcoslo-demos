using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace ActionConstraint
{
    public class LanguageSpecificRouteAttribute : RouteAttribute, IActionConstraintFactory
    {
        private readonly IActionConstraint _constraint;

        public bool IsReusable => true;

        public LanguageSpecificRouteAttribute(string template, string locale) : base(template)
        {
            Order = -10;
            _constraint = new AcceptLanguageActionConstraint(locale);
        }

        public IActionConstraint CreateInstance(IServiceProvider services)
        {
            return _constraint;
        }
    }
}
