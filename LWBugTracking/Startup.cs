using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LWBugTracking.Startup))]
namespace LWBugTracking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
