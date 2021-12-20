using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("VajraWorldWebAPP",typeof(UppaiWeb.Startup))]
namespace UppaiWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
