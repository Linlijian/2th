using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_2th.Startup))]
namespace _2th
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
