using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cosmetic.Startup))]
namespace Cosmetic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
