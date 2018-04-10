using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InrappAdmin.Web.Startup))]
namespace InrappAdmin.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
