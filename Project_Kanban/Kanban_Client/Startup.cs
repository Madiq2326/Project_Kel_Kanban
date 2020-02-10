using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kanban_Client.Startup))]
namespace Kanban_Client
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
