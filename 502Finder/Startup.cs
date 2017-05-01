using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_502Finder.Startup))]
namespace _502Finder
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
