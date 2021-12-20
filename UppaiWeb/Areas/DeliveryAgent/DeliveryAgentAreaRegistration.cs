using System.Web.Mvc;

namespace UppaiWeb.Areas.DeliveryAgent
{
    public class DeliveryAgentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DeliveryAgent";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DeliveryAgent_default",
                "DeliveryAgent/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}