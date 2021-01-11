using System.Web.Mvc;

namespace VAPortale.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: "File_FotoPerFormato",
                url: "File/Immagine/{immagineMasterID}/{formatoImmagineID}",
                defaults: new { controller = "File", action = "ImmaginePerFormato" },
                namespaces: new[] { "VAPortale.Controllers" }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Login", id = UrlParameter.Optional },
                new[] { "VAPortale.Areas.Admin.Controllers" }
            );
        }
    }
}
