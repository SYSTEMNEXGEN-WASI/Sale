using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRM_V3.Startup))]
namespace CRM_V3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
