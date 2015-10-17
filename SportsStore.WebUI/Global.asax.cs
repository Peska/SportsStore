using SportsStore.Domain.Entities;
using SportsStore.WebUI.Infrastructure.Binders;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SportsStore.WebUI
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder());
		}
	}
}
