using Microsoft.WindowsAzure.ServiceRuntime;
using Orleans.Host;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OrleansR.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            if (RoleEnvironment.IsAvailable)
            {
                OrleansAzureClient.Initialize(Server.MapPath("~/AzureConfiguration.xml"));
            }
            else
            {
                Orleans.OrleansClient.Initialize(Server.MapPath("~/ClientConfiguration.xml"));
            }


        }
    }
}
