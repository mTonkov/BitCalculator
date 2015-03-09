using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BitCalculator.Startup))]
namespace BitCalculator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
