using System.Web.Mvc;
using System.Web.Optimization;
using VersoMVC.App_Start;

namespace VersoMVC.Areas.Mobile
{
    public class MobileAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Mobile";
            }
        }
      
        public override void RegisterArea(AreaRegistrationContext context)
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            context.MapRoute(
                "Mobile_default",
                "Mobile/{controller}/{action}/{id}",
//                new { action = "Index", id = UrlParameter.Optional }
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "VersoMVC.Areas.Mobile.Controllers" }
            );
            context.MapRoute(
            "Mobile_ListViews",
            "mobile/search/briefview/{searchTerm}/{grouping}/{id}/{pollID}",
            new { controller = "MobileSearch", action = "ListCluster", grouping = "full", searchTerm = "Default" },
            new[] { "VersoMVC.Areas.Mobile.Controllers" }
            );
            context.MapRoute(
            "Mobile_FullRecord",              
            "mobile/Search/FullRecord/{searchTerm}/{id}/{max}/{nextRecordID}",
            new { controller = "MobileSearch", action = "FullRecord", searchTerm = "Default", max = UrlParameter.Optional, nextRecordID = UrlParameter.Optional },
            new[] { "VersoMVC.Areas.Mobile.Controllers" }
            );            
        }
    }
}
