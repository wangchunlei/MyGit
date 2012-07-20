using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domas.Web.Tools.PortableAreas;
using LoginPartialArea.Login.Messages;

namespace LoginPartialArea.Login
{
    class LoginRegistraion : PortableAreaRegistration
    {
        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context, IApplicationBus bus)
        {
            bus.Send(new RegistrationMessage("Registering Login Portable Area"));
            context.MapRoute(
                "login", 
                "login/{controller}/{action}", 
                new { controller = "login", action = "index" });

            this.RegisterAreaEmbeddedResources();
        }
        public override string AreaName
        {
            get { return "Login"; }
        }
    }
}
