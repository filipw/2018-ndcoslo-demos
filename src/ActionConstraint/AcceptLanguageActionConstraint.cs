using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System;
using System.Linq;

namespace ActionConstraint
{
    public class AcceptLanguageActionConstraint : IActionConstraint, IActionConstraintMetadata
    {
        private string _locale;

        public AcceptLanguageActionConstraint(string locale)
        {
            _locale = locale;
        }

        public int Order => 0;

        public bool Accept(ActionConstraintContext context)
        {
            var headers = context.RouteContext.HttpContext.Request.GetTypedHeaders();

            // only allow route to be hit if the predefined header is present
            if (headers.AcceptLanguage != null && headers.AcceptLanguage.Any(x => x.Value.Equals(_locale, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return false;
        }
    }
}
