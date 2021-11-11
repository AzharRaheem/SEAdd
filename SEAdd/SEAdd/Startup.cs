using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SEAdd.Startup))]
namespace SEAdd
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
