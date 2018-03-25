using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;


namespace TaskDispatchManager.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();
            #region 允许全局跨域

            var cors = new EnableCorsAttribute("*", "*", "*")
            {
                PreflightMaxAge = 100000
            };
            //跨域
            config.EnableCors(cors);

            #endregion
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "Api",
               routeTemplate: "{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional }
           );

            #region 使用CamelCase命名法序列化webApi的返回结果。

            //var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            //jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); 
            #endregion


            config.Filters.Add(new CustomExceptionFilterAttribute());
        }
    }
}
