using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Presentation.Legacy;

[assembly: OwinStartup(typeof(Startup))]
namespace Presentation.Legacy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new MyIdProvider());

            ConfigureAuth(app);
            app.MapSignalR();
        }
    }

    public class MyIdProvider: IUserIdProvider
    {
        public string GetUserId(IRequest request)
        {
            return request.User.Identity.GetUserId();
        }
    }
}
