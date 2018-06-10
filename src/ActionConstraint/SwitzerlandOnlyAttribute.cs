using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;

namespace ActionConstraint
{
    public class SwitzerlandOnlyAttribute : Attribute, IActionConstraintFactory
    {
        private AcceptLanguageActionConstraint _constraint;

        public bool IsReusable => true;

        public SwitzerlandOnlyAttribute()
        {
            _constraint = new AcceptLanguageActionConstraint("de-CH");
        }

        public IActionConstraint CreateInstance(IServiceProvider services)
        {
            return _constraint;
        }
    }
}
