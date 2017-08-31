using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GhostPhotographerBlog.UI.Startup))]
namespace GhostPhotographerBlog.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
