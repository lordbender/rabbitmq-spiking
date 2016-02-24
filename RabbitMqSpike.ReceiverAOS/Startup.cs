using Microsoft.Owin;

using Owin;

using RabbitMqSpike.ReceiverAOS;

[assembly: OwinStartup(typeof (Startup))]

namespace RabbitMqSpike.ReceiverAOS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}