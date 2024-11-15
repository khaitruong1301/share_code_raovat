using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebsiteRaoVat.Startup))]
namespace WebsiteRaoVat
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
