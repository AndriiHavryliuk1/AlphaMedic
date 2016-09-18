using Owin;
using Microsoft.Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(Rest.Startup))]
namespace Rest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //var config = ConfigureWebApi();            
            //app.UseWebApi(config);
        }


        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            return config;
        }

        

    }
}