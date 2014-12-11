
using System.Web.Http;
using AllocatorShare2.App_Start;
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
