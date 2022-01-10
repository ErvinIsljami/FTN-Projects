using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AliExpressWebRole.Startup))]
namespace AliExpressWebRole
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
