using System.Globalization;
using System.Web.Mvc;
using Verso.Common.Contracts;
using Verso.Common.Contracts.Mobile;
using Verso.Common.MobileSearch;
using VersoMVC.ws;
using VersoMVC.ws.Model;

namespace VersoMVC.Areas.Mobile.Controllers
{
    public class MobileSearchController : Controller
    {
        private readonly IMobileSearchProvider _mobileSearchProvider;
        private readonly ISessionProvider _sessionManager;       
        public MobileSearchController(IMobileSearchProvider mobileSearchProvider, ISessionProvider sessionManager)
        {
            _mobileSearchProvider = mobileSearchProvider;
            _sessionManager = sessionManager;
        }
        public JsonResult GetAgentLoginSession(MobileAgentUserSession mobileAgentUserSession)
        {            
            var loginSession = _mobileSearchProvider.GetAgentLoginSession(mobileAgentUserSession, _sessionManager.ClientSession);
            return Json(loginSession);
        }
        [HttpPost]
        public JsonResult GetSearchResult(string keyword, string navId = "")
        {            
            var searchQuery = new MobileSearchQuery
                { 
                SearchTerm = keyword,
                PageSize = 20,
                SortBy = "1",
                NavigationId = navId
            };            
            return Json( _mobileSearchProvider.GetSearchResult(searchQuery, _sessionManager.ClientSession));
        }
        [HttpPost]
        public JsonResult GetRecordDetail(string agControlId)
        {
            //var fullRecord = _mobileSearchProvider.
            var mobileRecordQuery = new MobileRecordQuery
                {
                    AgControlId = int.Parse(agControlId)
                };
            var user = _sessionManager.ClientSession;
            var agenzoSession = new AgEnzoSession
                {
                    AlowLibrarySelection = false,
                    CustomerId = user.CustomerID,
                    LibraryId = user.LibraryID,
                    ServerName = user.ServerName,
                    SessionId = user.SessionID.ToString(CultureInfo.InvariantCulture)
                };

            var resourceInfo = _mobileSearchProvider.GetMainResourceInfo(user);
            var availabilities = new AgEnzoMyAccount().GetAvailabilitiesFor(agenzoSession, int.Parse(agControlId), int.Parse(resourceInfo.ResourceId),resourceInfo.ResourceType);
            var model = new RecordDetailModel
                {
                    Availabilities = availabilities,
                    RecordModel = _mobileSearchProvider.GetRecordDetails(mobileRecordQuery, user)
                };            
            return Json(model);
        }    
    }
    public class RecordDetailModel
    {
        public MobileRecordQuery RecordModel { get; set; }   
        public AgEnzoItemAvailabilitiesResult Availabilities { get; set; }
    }
}
