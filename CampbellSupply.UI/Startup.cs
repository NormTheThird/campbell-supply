using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CampbellSupply.Startup))]
namespace CampbellSupply
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}