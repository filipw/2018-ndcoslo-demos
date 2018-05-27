// adapted from https://github.com/aspnet/Mvc/tree/dev/src/Microsoft.AspNetCore.Mvc.WebApiCompatShim

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Convention2
{
    public class RestfulActionApplicationModelConvention : IControllerModelConvention
    {
        private static readonly string[] SupportedHttpMethodConventions = new string[]
        {
            "GET",
            "PUT",
            "POST",
            "DELETE",
            "PATCH",
            "HEAD",
            "OPTIONS",
        };

        public void Apply(ControllerModel controller)
        {
            var newActions = new List<ActionModel>();

            foreach (var action in controller.Actions)
            {
                SetHttpMethodFromConvention(action);

                foreach (var actionSelectorModel in action.Selectors)
                {
                    actionSelectorModel.ActionConstraints.Add(new OverloadActionConstraint());
                }

                var optionalParameters = new HashSet<string>();
                var uriBindingSource = (new FromUriAttribute()).BindingSource;
                foreach (var parameter in action.Parameters)
                {
                    // Some IBindingSourceMetadata attributes like ModelBinder attribute return null 
                    // as their binding source. Special case to ensure we do not ignore them.
                    if (parameter.BindingInfo?.BindingSource != null ||
                        parameter.Attributes.OfType<IBindingSourceMetadata>().Any())
                    {
                        // This has a binding behavior configured, just leave it alone.
                    }
                    else if (CanConvertFromString(parameter.ParameterInfo.ParameterType))
                    {
                        // Simple types are by-default from the URI.
                        parameter.BindingInfo = parameter.BindingInfo ?? new BindingInfo();
                        parameter.BindingInfo.BindingSource = uriBindingSource;
                    }
                    else
                    {
                        // Complex types are by-default from the body.
                        parameter.BindingInfo = parameter.BindingInfo ?? new BindingInfo();
                        parameter.BindingInfo.BindingSource = BindingSource.Body;
                    }

                    // For all non IOptionalBinderMetadata, which are not URL source (like FromQuery etc.) do not
                    // participate in overload selection and hence are added to the hashset so that they can be
                    // ignored in OverloadActionConstraint.
                    var optionalMetadata = parameter.Attributes.OfType<IOptionalBinderMetadata>().SingleOrDefault();
                    if (parameter.ParameterInfo.HasDefaultValue && parameter.BindingInfo.BindingSource == uriBindingSource ||
                        optionalMetadata != null && optionalMetadata.IsOptional ||
                        optionalMetadata == null && parameter.BindingInfo.BindingSource != uriBindingSource)
                    {
                        optionalParameters.Add(parameter.ParameterName);
                    }
                }

                action.Properties.Add("OptionalParameters", optionalParameters);

                // Action Name doesn't really come into play with attribute routed actions. However for a
                // non-attribute-routed action we need to create a 'named' version and an 'unnamed' version.
                var namedAction = action;

                var unnamedAction = new ActionModel(namedAction);
                unnamedAction.RouteValues.Add("action", null);
                newActions.Add(unnamedAction);
            }

            foreach (var action in newActions)
            {
                controller.Actions.Add(action);
            }
        }

        private void SetHttpMethodFromConvention(ActionModel action)
        {
            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints.OfType<HttpMethodActionConstraint>().Count() > 0)
                {
                    // If the HttpMethods are set from attributes, don't override it with the convention
                    return;
                }
            }

            // The Method name is used to infer verb constraints. Changing the action name has no impact.
            foreach (var verb in SupportedHttpMethodConventions)
            {
                if (action.ActionMethod.Name.StartsWith(verb, StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var selector in action.Selectors)
                    {
                        selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));
                    }

                    return;
                }
            }

            // If no convention matches, then assume POST
            foreach (var actionSelectorModel in action.Selectors)
            {
                actionSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { "POST" }));
            }
        }

        private static bool CanConvertFromString(Type destinationType)
        {
            destinationType = Nullable.GetUnderlyingType(destinationType) ?? destinationType;
            return IsSimpleType(destinationType) ||
                   TypeDescriptor.GetConverter(destinationType).CanConvertFrom(typeof(string));
        }

        private static bool IsSimpleType(Type type)
        {
            return type.GetTypeInfo().IsPrimitive ||
                type.Equals(typeof(decimal)) ||
                type.Equals(typeof(string)) ||
                type.Equals(typeof(DateTime)) ||
                type.Equals(typeof(Guid)) ||
                type.Equals(typeof(DateTimeOffset)) ||
                type.Equals(typeof(TimeSpan)) ||
                type.Equals(typeof(Uri));
        }
    }
}
