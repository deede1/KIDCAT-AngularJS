using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Verso.Common.Contracts;
using Verso.Common.Contracts.Mobile;
using Verso.Common.MobileSearch;
using Verso.Common.UserProvider;

namespace VersoMVC.Areas.Mobile.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Mobile/Home/
        private readonly IMobileSearchProvider _mobileSearchProvider;
        private readonly ISessionProvider _sessionManager;   
        public HomeController(IMobileSearchProvider mobileSearchProvider, ISessionProvider sessionManager)
        {
            _mobileSearchProvider = mobileSearchProvider;
            _sessionManager = sessionManager;
        }
        public ActionResult Index()
        {
            //User user = _sessionManager.ClientSession;

            //var searchQuery = new MobileSearchQuery()
            //    {
            //        CustomerId = user.CustomerID,
            //        CustomerServer = user.ServerName,
            //        LibraryDbPoolKey = user.MainLibraryDBPoolKey,
            //        SessionId = user.SessionID,
            //        SearchTerm = "Harry Potter",
            //        PageSize = 20,
            //        SortBy = "sadf",
            //        NavigationId = ""

            //    };
          
            //var searchResult = _mobileSearchProvider.GetSearchResult(searchQuery, user);
            return View(); 
        }

    }
}
