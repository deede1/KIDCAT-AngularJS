using System.Web.Mvc;

namespace VersoMVC.Areas.SystemSettings
{
    public class SystemSettingsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SystemSettings";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SystemSettings_default",
                "SystemSettings/{controller}/{action}/{id}/{isGlobal}/{typeId}/{typeName}/{isLocal}",
                new { Controller = "LoginOptions", action = "Index", id = UrlParameter.Optional, typeId = UrlParameter.Optional, typeName = UrlParameter.Optional, isGlobal = UrlParameter.Optional, isLocal = UrlParameter.Optional }
            );
        }
    }
}
