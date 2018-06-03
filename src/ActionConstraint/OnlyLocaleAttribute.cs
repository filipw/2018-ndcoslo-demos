using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace ActionConstraint
{
    public class OnlyLocaleAttribute : Attribute, IActionConstraintFactory
    {
        private AcceptLanguageActionConstraint _constraint;

        public bool IsReusable => true;

        public OnlyLocaleAttribute(string locale)
        {
            _constraint = new AcceptLanguageActionConstraint(locale);
        }

        public IActionConstraint CreateInstance(IServiceProvider services)
        {
            return _constraint;
        }
    }
}
