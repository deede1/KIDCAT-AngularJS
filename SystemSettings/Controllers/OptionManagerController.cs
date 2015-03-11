using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Verso.Common.Contracts;
using Verso.Common.Exceptions;
using Verso.Common.MenuProvider;
using Verso.Common.UXAdmin;
using Verso.Common.UserProvider;
using Verso.Common.Contracts.UXAdmin;
using VersoMVC.Controllers.Helper;

namespace VersoMVC.Areas.SystemSettings.Controllers
{
    public class OptionManagerController : Controller
    {
        private const string ErrorMessage = "We encountered an error saving your update.";
        private readonly IUxAdminLibraryOptionManagerProvider _uxAdminLibraryOptionManagerProvider;
        private readonly ISessionProvider _sessionProvider;
        private readonly IMenuProvider _menuProvider;

        public OptionManagerController(
            IUxAdminLibraryOptionManagerProvider uxAdminLibraryOptionManagerProvider, 
            ISessionProvider sessionProvider, 
            IMenuProvider menuProvider)
        {
            _menuProvider = menuProvider;
            _uxAdminLibraryOptionManagerProvider = uxAdminLibraryOptionManagerProvider;
            _sessionProvider = sessionProvider;
        }

        public ActionResult Index(string ot)
        {
            return View(@"~\Areas\SystemSettings\Views\OptionManager\" + ot + ".cshtml");
        }
        
        [HttpGet]
        public JsonResult GetLoginOptions()
        { 
            User user = _sessionProvider.ClientSession;
            var loginOption = _uxAdminLibraryOptionManagerProvider.GetLoginOptions(user);
            return new AgJson(loginOption, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveLoginOption(LoginOptions oLoginOptions)
        {
            User user = _sessionProvider.ClientSession;         
            _uxAdminLibraryOptionManagerProvider.SaveLoginOptions(0,oLoginOptions,user);
            return Json("Your data has been successfully saved");
        }

        [HttpPost]
        public JsonResult GetSearchOption()
        {
            User user = _sessionProvider.ClientSession;
            var searchOptionResult = _uxAdminLibraryOptionManagerProvider.GetSearchOptions(user);            
            return new AgJson(searchOptionResult, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSearchOption(SearchOptionManager mySearchOption)
        {
            User user = _sessionProvider.ClientSession;            
            _uxAdminLibraryOptionManagerProvider.UpdateSearchOptions(mySearchOption.SearchOptionId, mySearchOption, user);
            return Json("Your data has been successfully saved", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFacetMapOption()
        {
            var facetMap = new FacetMappingDetails(_sessionProvider.FacetMap);
            return new AgJson(facetMap, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetResultOption()
        {
            User user = _sessionProvider.ClientSession;
            var optionResult = (_sessionProvider != null) ? _sessionProvider.ResultsOption : null;
            if (optionResult == null)
            {
                optionResult = _uxAdminLibraryOptionManagerProvider.GetResultsOptions(user);
            }
            var facetMap = new FacetMappingDetails(_sessionProvider.FacetMap);
            var model = new {optionResult, facetMap};
            return new AgJson(model, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SaveResultOption(ResultsOptionManager myResultOption)
        {
            User user = _sessionProvider.ClientSession;
            _uxAdminLibraryOptionManagerProvider.UpdateResultsOptions(0, myResultOption, user);

            _uxAdminLibraryOptionManagerProvider.SetFacetManagement(_sessionProvider.FacetMap, new FacetMappingDetails
                {
                    StaffDetails = myResultOption.StaffDetails,
                    GuestPatronDetails = myResultOption.GuestPatronDetails
                }, user);
            return Json("Your data has been successfully saved", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFullRecordOption()
        {
            User user = _sessionProvider.ClientSession;
            var optionResult = _uxAdminLibraryOptionManagerProvider.GetFullRecordOptions(user);
            return new AgJson(optionResult, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SaveFullRecordOption(FullRecordOptionManager myFullRecordOption)
        {
            User user = _sessionProvider.ClientSession;
            _uxAdminLibraryOptionManagerProvider.UpdateFullRecordOptions(0, myFullRecordOption, user);
            return Json("Your data has been successfully saved", JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetWebLinkOptionsByUser()
        {            
            return new AgJson(_uxAdminLibraryOptionManagerProvider.GetWebLinkPageByUser(_sessionProvider.ClientSession));
        }
        
        [HttpPost]
        public JsonResult GetWebLinkOptions()
        {                        
            var optionResult = _uxAdminLibraryOptionManagerProvider.GetWebLinksOptions(_sessionProvider.ClientSession);
            return new AgJson(optionResult);
        }
        
        [HttpPost]
        public JsonResult SaveWebLinkOptions(WebLinksOptions xWebLinksOptions)
        {            
            _uxAdminLibraryOptionManagerProvider.UpdateWebLinkOptions(0, xWebLinksOptions, _sessionProvider.ClientSession);            
            return Json("Your data has been successfully saved");
        }
        
        [HttpGet]
        public JsonResult GetHeaderFooterOption()
        {
            //User user = _session.ClientSession;
            //var optionResult = _uxAdminLibraryOptionManagerProvider.GetHeaderFooterOptions(user);
            var optionResult = _sessionProvider.HeaderFooterOptions;
            return new AgJson(optionResult, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult SaveHeaderFooterOption(HeaderFooterOption myHeaderFooterOption)
        {
            User user = _sessionProvider.ClientSession;
            _uxAdminLibraryOptionManagerProvider.UpdateHeaderFooterOptions(0, myHeaderFooterOption,user);
            _sessionProvider.HeaderFooterOptions = myHeaderFooterOption;
            return Json("Your data has been successfully saved", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult HeaderImageUpload(HttpPostedFileBase file)
        {

            if (file == null || file.ContentLength == 0)
            {
                return Json(new
                    {
                        IsValid = false,
                        Message = "No file was uploaded.",
                    }, JsonRequestBehavior.AllowGet);
            }

            string fileName = String.Format("{0}.jpg", Guid.NewGuid().ToString());
            //var imagePath = Path.Combine(Server.MapPath("/download_temp/uxadmin/"), fileName);


            #if DEBUG

            file.SaveAs(Path.Combine(Server.MapPath(Url.Content("~/download_temp/uxadmin/")), fileName));           
                return Json(new
                    {
                        IsValid = true,
                        Message = string.Empty,                        
                        ImagePath = Path.Combine(Url.Content("~/download_temp/uxadmin/"), fileName)
                    }, JsonRequestBehavior.AllowGet);

            #else
                if (!(Directory.Exists(Server.MapPath("/download_temp/uxadmin/"))))
                {
                    //DirectoryInfo di = Directory.CreateDirectory(Server.MapPath("/download_temp/uxadmin/"));
                    Directory.CreateDirectory(Server.MapPath("/download_temp/uxadmin/"));
                }

                file.SaveAs(Path.Combine(Server.MapPath("/download_temp/uxadmin/"), fileName));
                return Json(new
                {
                    IsValid = true,
                    Message = string.Empty,
                    ImagePath = Url.Content(String.Format("/download_temp/uxadmin/{0}", fileName))
                }, JsonRequestBehavior.AllowGet);

            #endif

        }
        
        [HttpGet]
        public JsonResult GetAvailableThemes()
        {
            User user = _sessionProvider.ClientSession;
            var options = _uxAdminLibraryOptionManagerProvider.GetAvailableThemes(user);
            return new AgJson(options, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLibraryThemeId()
        {
            User user = _sessionProvider.ClientSession;
            var options = _uxAdminLibraryOptionManagerProvider.GetColorOptions(user).ColorThemeId;
            return new AgJson(options, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLibraryTheme(UIColorOptions options)
        {

            int? themeId = options == null ? null : options.ColorThemeId;

            User user = _sessionProvider.ClientSession;
            var sucess = _uxAdminLibraryOptionManagerProvider.SaveLibraryTheme(themeId, user);
            var mgsText = sucess ? "This change has been successfully saved." : "There was a problem saving this change.";
            return Json(new
            {
                status = sucess,
                message = mgsText
            });
        }

        [HttpPost]
        public JsonResult SaveUiColorOption(UIColorOptions options)
        {
            var sucess = false;
            if (!(options == null || options.ColorThemeId == null || !options.isCustom ||
                (options.ColorThemeId > 0 && options.ColorThemeId <= 7)))
            {
                var selected = options.Selected;
                var user = _sessionProvider.ClientSession;
                options = _uxAdminLibraryOptionManagerProvider.SaveColorOptions(user, options);
                sucess = options.Selected;
                options.Selected = selected;
            }
            
            return Json(new
            {
                status = sucess,
                theme = options
            });
        }

        #region Staff Menu Tab Management
        [HttpGet]
        public JsonResult GetStaffMenuTab()
        {
            var user = _sessionProvider.ClientSession;
            var type = _sessionProvider.LibrarySettingsSession.LicenseType;

            var staffMenuTabs = _menuProvider.GetMenuOrder(user, type);
            return new AgJson(staffMenuTabs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveGlobalStaffMenuOrder(SortMenuTabOptions myStaffMenu)
        {
            var user = _sessionProvider.ClientSession;
            var type = _sessionProvider.LibrarySettingsSession.LicenseType;
            var myMenu = new List<MenuOrdinal>();
            var counter = 0;
            StaffMenuTabs rslt;
            try
            {
                foreach (dynamic o in myStaffMenu.MenuTabs)
                {
                    myMenu.Add(new MenuOrdinal
                    {
                        LanguageId = o.LanguageId,
                        MenuText = o.MenuText,
                        Ordinal = counter,
                        Suppress = o.Suppress
                    });
                    counter++;
                }
                var mnuOk = _menuProvider.SetStaffMenuOrdinals(user, type, myMenu);
                var alwOk = _menuProvider.SetMenuAllowLocalOrdinals(user, type, myStaffMenu.AllowLibraries);
                rslt = _menuProvider.GetMenuOrder(user, type);
                rslt.EncounteredError = !(mnuOk && alwOk);
                if (rslt.EncounteredError) { rslt.ErrorMessage = ErrorMessage; }
            }
            catch (Exception ex)
            {
                rslt = _menuProvider.GetMenuOrder(user, type);
                var exec = new AgHandledException(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(exec);
                rslt.EncounteredError = true;
                rslt.ErrorMessage = ErrorMessage;
            }

            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveLocalStaffMenuOrder(SortMenuTabOptions myStaffMenu)
        {
            var user = _sessionProvider.ClientSession;
            var type = _sessionProvider.LibrarySettingsSession.LicenseType;
            var myMenu = new List<MenuOrdinal>();
            var counter = 0;
            StaffMenuTabs rslt;
            try
            {
                foreach (dynamic o in myStaffMenu.MenuTabs)
                {
                    myMenu.Add(new MenuOrdinal
                    {
                        LanguageId = o.LanguageId,
                        MenuText = o.MenuText,
                        Ordinal = counter,
                        Suppress = o.Suppress
                    });
                    counter++;
                }
                var mnuOk = _menuProvider.SetLibraryMenuOrdinals(user, type, myMenu);
                rslt = _menuProvider.GetMenuOrder(user, type);
                rslt.EncounteredError = !mnuOk;
                if (rslt.EncounteredError) { rslt.ErrorMessage = ErrorMessage; }
            }
            catch (Exception ex)
            {
                rslt = _menuProvider.GetMenuOrder(user, type);
                var exec = new AgHandledException(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(exec);
                rslt.EncounteredError = true;
                rslt.ErrorMessage = ErrorMessage;
            }

            return Json(rslt, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveLibraryMenuOrdinals()
        {
            var user = _sessionProvider.ClientSession;
            var type = _sessionProvider.LibrarySettingsSession.LicenseType;
            StaffMenuTabs rslt;
            try
            {
                var mnuOk = _menuProvider.RemoveLibraryMenuOrdinals(user);
                rslt = _menuProvider.GetMenuOrder(user, type);
                rslt.EncounteredError = !mnuOk;
                if (rslt.EncounteredError) { rslt.ErrorMessage = ErrorMessage; }
            }
            catch (Exception ex)
            {
                rslt = _menuProvider.GetMenuOrder(user, type);
                var exec = new AgHandledException(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(exec);
                rslt.EncounteredError = true;
                rslt.ErrorMessage = ErrorMessage;
            }

            return Json(rslt, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
