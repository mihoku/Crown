using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(crown.Startup))]
namespace crown
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
