using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AllocatorShare2.Startup))]
namespace AllocatorShare2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
