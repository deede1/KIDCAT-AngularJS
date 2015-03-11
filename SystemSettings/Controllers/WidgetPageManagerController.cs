using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Verso.Common.Contracts;
using Verso.Common.Contracts.UXAdmin;
using Verso.Common.UXAdmin;
using Verso.Common.UserProvider;
using VersoMVC.Controllers.Helper;

namespace VersoMVC.Areas.SystemSettings.Controllers
{
    public class WidgetPageManagerController : Controller
    {
        //
        // GET: /SystemSettings/WidgetPageManager/
        private readonly ISessionProvider _session;
        private readonly IPageManagerProvider _pageManagerProvider;
        private readonly IWidgetManagerProvider _widgetManagerProvider;
        public WidgetPageManagerController( ISessionProvider session, IPageManagerProvider pageManagerProvider, IWidgetManagerProvider widgetManagerProvider)
        {
            _session = session;
            _pageManagerProvider = pageManagerProvider;
            _widgetManagerProvider = widgetManagerProvider;            
        }
        [HttpPost]
        public JsonResult GetWidgetPageDetail(PageList myPageInput)
        {
            User user = _session.ClientSession;
            //var myPageData = _pageManagerProvider.GetPageDetailById(myPageInput.Index, user);
            var widgetPageModel = new WidgetPageModel();

         /*   foreach (var VARIABLE in myPageData)
            {
                
            }
            */
            return new AgJson(widgetPageModel, JsonRequestBehavior.AllowGet);
        }

    }
    public class WidgetPageModel
    {
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public bool ShowThisPageForCurrentUser { get; set; }
        public bool ForceLibraryToUserGlobal { get; set; }
        public bool DefaultTo { get; set; }
        public string WidgetJsonData { get; set; }
    }
}
