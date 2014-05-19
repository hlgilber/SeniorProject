using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GraceChurchKelseyvilleAwana.Startup))]
namespace GraceChurchKelseyvilleAwana
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
