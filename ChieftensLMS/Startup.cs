using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChieftensLMS.Startup))]
namespace ChieftensLMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
