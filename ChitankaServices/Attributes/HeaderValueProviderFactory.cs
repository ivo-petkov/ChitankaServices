using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;

namespace ChitankaServices.Attributes
{
    public class HeaderValueProviderFactory<T> : ValueProviderFactory where T : class
    {
        public override System.Web.Http.ValueProviders.IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            var headers = actionContext.ControllerContext.Request.Headers;
            return new HeaderValueProvider<T>(headers);
        }
    }
}