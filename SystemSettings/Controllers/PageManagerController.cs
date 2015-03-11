using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Verso.Common;
using Verso.Common.Contracts;
using Verso.Common.Contracts.UXAdmin;
using Verso.Common.Extensions;
using Verso.Common.UXAdmin;
using Verso.Common.UserProvider;
using Verso.Common.WidgetProvider;
using WCF.DAL;
using VersoMVC.Controllers.Helper;
using Verso.Common.SearchProvider;

namespace VersoMVC.Areas.SystemSettings.Controllers
{
    public class PageManagerController : Controller
    {
        //
        // GET: /SystemSettings/PageManager/
        private readonly IPageManagerProvider _pageManagerProvider;
        private readonly ISessionProvider _session;
        private readonly ILibraryConfigProvider _libraryConfigProvider;
        private readonly ISearchProvider _searchProvider;
        private readonly IAdvancedSearchProvider _advancedSearchProvider;

        public PageManagerController(IPageManagerProvider pageManagerProvider, ISessionProvider session, ILibraryConfigProvider ilibraryConfigProvider, ISearchProvider searchProvider, IAdvancedSearchProvider advancedSearchProvider)
        {
            _pageManagerProvider = pageManagerProvider;
            _session = session;
            _libraryConfigProvider = ilibraryConfigProvider;
            _searchProvider = searchProvider;
            _advancedSearchProvider = advancedSearchProvider;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //return View();
            return PartialView(@"~\Areas\SystemSettings\Views\PageManager\Index.cshtml");
        }
        public ActionResult CreatePage()
        {
            User user = _session.ClientSession;
            
            return View(user);
        }
        [HttpPost]
        public JsonResult UpdateUserGlobal(PageGlobalModel myModel)
        {
            User user = _session.ClientSession;
            var sGlobalPageId = myModel.Index.Split('|')[0];
            SetUseGlobalResult model;
            int globalId;
            if (!int.TryParse(sGlobalPageId, out globalId))
            {
                model = new SetUseGlobalResult
                    {
                        Message = "Global usage could not be set.",
                        Status = false,
                        PageList = _pageManagerProvider.GetPageResult(user)
                    };
            }
            else
            {
                model = _pageManagerProvider.SetUseGlobal(user, globalId, myModel.UseGlobal);
            }
            
            return Json(model);
        }

        public ActionResult GetPageInfoView(string id, string isGlobal , string typeId = "-1", string typeName = "")
        {
            User user = _session.ClientSession;

            var myPageData = new PageManagerModel();
            string pageName;
            var pageTitle = typeId == "-1" ? "Home Page" : "";
            if (typeName.ToLower() == "new")
            {
                var globalLibs = Convert.ToBoolean(isGlobal)
                                     ? _pageManagerProvider.GeLibrariesForGlobalPage(user, int.Parse(typeId))
                                     : new List<LibrarybMaster>();
                myPageData.PageCollector = new PageCollector
                    {
                    GlobalLibraryList = globalLibs,
                    //GlobalTemplate = "{\"PageTypeId\":0,\"PageType\":\"WidgetPage\",\"PageTypeEnable\":true,\"IsLocalPage\":true,\"IsGlobalPage\":false,\"ForceLibraryToUseGlobal\":false,\"SelectedLibrariesToForceUseGlobal\":\"\",\"PageTitleName\":\"123\",\"EnableGuests\":true,\"EnablePatrons\":true,\"EnableStaff\":true,\"DefaultPage\":\"Local\",\"LocalTemplate\":\"[]\",\"GlobalTemplate\":\"[]\",\"PageUrl\":\"\"}",
                    GlobalTemplate = "{\"PageTitleName\":\"" + pageTitle + "\",\"LocalTemplate\":\"[]\",\"EnableGuests\":true,\"EnablePatrons\":true,\"EnableStaff\":true,\"GlobalTemplate\":\"[]\",\"PageUrl\":\"\"}",
                    LocalTemplate = "{\"PageTitleName\":\"\",\"LocalTemplate\":\"[]\",\"EnableGuests\":true,\"EnablePatrons\":true,\"EnableStaff\":true,\"GlobalTemplate\":\"[]\",\"PageUrl\":\"\"}",
                    PageTypeId = int.Parse(typeId)
                };
                pageName = (Convert.ToBoolean(isGlobal)) ? int.Parse(typeId) != 1 ? "WidgetPageGlobal" : "LinkPageGlobal" : int.Parse(typeId) != 1 ? "WidgetPageLocal" : "LinkPageLocal";                

            }
            else
            {
                if (Convert.ToBoolean(isGlobal))
                {
                    myPageData.PageCollector = _pageManagerProvider.GetGlobalPageDetailById(int.Parse(id), user);
                    myPageData.IsCustomerSuperUser = user.IsCustSuper;
                    pageName = myPageData.PageCollector.PageTypeId != 1 ? "WidgetPageGlobal" : "LinkPageGlobal";
                }
                else
                {
                    myPageData.PageCollector = _pageManagerProvider.GetPageDetailById(id.Replace("~","|"), user);
                    myPageData.IsCustomerSuperUser = user.IsCustSuper;
                    pageName = myPageData.PageCollector.PageTypeId != 1 ? "WidgetPageLocal" : "LinkPageLocal";
                }    
            }
            
            return View(@"~\Areas\SystemSettings\Views\PageManager\" + pageName + ".cshtml", myPageData);
        
        }
        public ActionResult GetPagePartialView(string id)
        {
           // string idx = id != "LinkPage" ? "WidgetPage" : "LinkPage";            
            return PartialView(@"~\Areas\SystemSettings\Views\PageManager\" + id + ".cshtml");
        }


        [HttpGet]
        public JsonResult GetMenuPageList()
        {
            //var data = new PageResult
            //{
            //    PageList = new List<PageList>
            //        {

            //            new PageList {Index = "1", Name = "Home Page", WType="Page Page", Key  = "EclipsePage"},
            //            new PageList {Index = "2", Name = "Ebook", WType="Page Page", Key = "EclipsePage"},
            //            new PageList {Index = "3", Name = "Media", WType="Page Page", Key = "EclipsePage"},
            //            new PageList {Index = "4", Name = "Google",WType="Link Page", Key = "LinkPage"}
            //        }
            //};

            User user = _session.ClientSession;
            List<PageList> myPageList = _pageManagerProvider.GetPageResult(user);


            IEnumerable<PageList> myList = from x in myPageList
                                           select
                                               new PageList
                                                   {
                                                   Index = x.Index,
                                                   Key = x.Key,
                                                   Name = x.Name,
                                                   WType = x.WType,
                                                   IsGlobal = x.IsGlobal,                                                    
                                                   ShowTabToCurrentUser = x.ShowTabToCurrentUser,
                                                   ExtraInfo = x.Key == "LinkPage"? x.ExtraInfo:""
                                               };


            var data = new PageResult
            {
                PageList = myList.ToList(),
                ShowGlobalTab = user.IsCustSuper
            };


            return new AgJson(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPageList()
        {
            //var data = new PageResult
            //{
            //    PageList = new List<PageList>
            //        {
            
            //            new PageList {Index = "1", Name = "Home Page", WType="Page Page", Key  = "EclipsePage"},
            //            new PageList {Index = "2", Name = "Ebook", WType="Page Page", Key = "EclipsePage"},
            //            new PageList {Index = "3", Name = "Media", WType="Page Page", Key = "EclipsePage"},
            //            new PageList {Index = "4", Name = "Google",WType="Link Page", Key = "LinkPage"}
            //        }
            //};

            User user = _session.ClientSession;
            var data = new PageResult
                {
                    ShowGlobalTab = user.IsCustSuper,
                    PageList = _pageManagerProvider.GetPageResult(user),
                    GlobalPages =
                        user.IsCustSuper ? _pageManagerProvider.GetGlobalPagesList(user) : new List<GlobalPageList>()
                };
            return new AgJson(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPageTypeList()
        {
            var loginSettings = _libraryConfigProvider.GetLoginSettingsBy(_session.ClientSession);
            var libraryList = loginSettings.Libraries;

            User user = _session.ClientSession;
            List<PageTypeList> myPageList = _pageManagerProvider.GetPageTypeList(user);
            var data = new PageManager
            {
                PageTypesList =  myPageList ,
                Librarys = libraryList,
                IsCustomerSuperUser = user.IsCustSuper,
                currentLID = user.LibraryID
            };

            return new AgJson(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetLibraryList(PageList myPageInput)
        {
            User user = _session.ClientSession;
            var loginSettings = _libraryConfigProvider.GetLoginSettingsBy(_session.ClientSession);
            //var lockouts = _pageManagerProvider.GetHomePageLockouts(user);
            List<PageApplyLibraryList> applyLibraryList = null;
            try
            {
                if (int.Parse(myPageInput.Index.Split('|')[0]) > 0)
                {
                 applyLibraryList = _pageManagerProvider.GetGlobalPageAssignmentsForPage(int.Parse(myPageInput.Index.Split('|')[0]), user);
                 applyLibraryList.Add(new PageApplyLibraryList {AGLibID = user.LibraryID});
                }
            }
            catch(Exception e)
            {
                string error = e.ToString();
            }

            var librariesData = new ApplySelectedLibraryList
                {
                    Libraries = loginSettings.Libraries.ToList(),
                     SelectedApplyGlobalLibrary =applyLibraryList != null ? from x in applyLibraryList select x.AGLibID.Replace(" ", ""): null,
                     DefaultLibrary = user.LibraryID,
                     LockoutLibrary = string.Empty
                };
            return new AgJson(librariesData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetOnlyLibraryList(PageList myPageInput)
        {
            var loginSettings = _libraryConfigProvider.GetLoginSettingsBy(_session.ClientSession);
            //var applyLibraryList = _pageManagerProvider.GetGlobalPageAssignmentsForPage(int.Parse(myPageInput.Index.Split('|')[0]), user);
            var librariesData = new ApplySelectedLibraryList
                {
                Libraries = loginSettings.Libraries.ToList(),
              //  SelectedApplyGlobalLibrary = from x in applyLibraryList select x.AGLibID.Replace(" ", "")
            };
            return new AgJson(librariesData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GeLibrariesForGlobalPage(int pageTypeId)
        {
            var user = _session.ClientSession;
            var lockouts = _pageManagerProvider.GeLibrariesForGlobalPage(user, pageTypeId);
            return new AgJson(lockouts);
        }

        private List<int> GetResources()
        {

            var resourceGroups = _session.Resources;
            var rslt = new List<int>();
            foreach (var node in from @group in resourceGroups from node in @group.ResourceNodes where !rslt.Contains(node.ID) select node)
            {
                rslt.Add(node.ID);
            }
            return rslt;
        }

        private FixedListShowcase CleanListShowCase(List<int>resources, IEnumerable<FixedShowcaseItem> items)
        {
            var rslt = new List<FixedShowcaseItem>();
            var recs = resources; // GetResources();
            var msg = string.Empty;
            var noResources = false;
            foreach (var itm in items)
            {
                var sp = itm.FullRecordUrl.Split('/').Last();
                int rc;
                if (int.TryParse(sp, out rc))
                {
                    if (recs.Contains(rc)) { rslt.Add(itm); }
                }
            }
            if (rslt.Count == 0)
            {
                noResources = true;
                msg = "No resources available for showcase.";
            }           
            return new FixedListShowcase { Items = rslt, NoResources = noResources, Message = msg };
        }    

        public string MyCleanPageContent(string content,string id)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                JObject pageContent = JObject.Parse(content);
                var widgetTypeId = int.Parse((string)pageContent["WidgetTypeId"]);

                if (widgetTypeId == 4)
                {
                    var resources = GetResources();
                    var showcaseType = pageContent["selectedShowcaseType"] != null ? pageContent["selectedShowcaseType"].ToString().ToInt32() : 0;
                    var items = new List<FixedShowcaseItem>();
                    if (showcaseType == 0)
                    {
                        var selectedJsonResources = pageContent["resources"];
                        if (selectedJsonResources != null)
                        {
                            var selectedResources = selectedJsonResources.ToObject<List<int>>();
                            selectedResources.Where(rs => !resources.Contains(rs)).ToList().ForEach(k => resources.Remove(k));
                            if (selectedResources.Count > 0)
                            {
                                items = GetSearchResultFromCache(id);
                            }
                        }
                    }
                    else
                    {
                        var tempShowCaseItemsJson = pageContent["TempShowcaseItems"];
                        items = tempShowCaseItemsJson.Select(itemJson => new FixedShowcaseItem { JacketArtNote = string.IsNullOrEmpty((string)itemJson["JacketArtNote"]) ? "" : itemJson["JacketArtNote"].ToString(), Title = itemJson["Title"].ToString(), Author = itemJson["Author"].ToString(), FormatType = itemJson["FormatType"].ToString(), JackArtUrl = itemJson["JackArtUrl"].ToString(), FullRecordUrl = itemJson["FullRecordUrl"].ToString(), PubYear = itemJson["PubYear"].ToString() }).ToList();
                    }
                    var cleanShowCaseList = CleanListShowCase(resources, items);
                    pageContent["TempShowcaseItems"] = JToken.Parse(serializer.Serialize(cleanShowCaseList.Items)); //serializer.Serialize(cleanShowCaseList.Items);  // JObject.FromObject(cleanShowCaseList.Items);
                    pageContent["WidgetId"] = id;
                    return JObject.FromObject(pageContent).ToString();
                }
                else
                {
                    return content;
                }
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
         
        }      

        private List<FixedShowcaseItem> GetSearchResultFromCache(string id)
        {
            try
            {
                var widgetId = id.ToInt32();
                var support = _session.SearchSupport;
                var showCaseItems = new List<FixedShowcaseItem>();
                //var cachedKey =     resources.ToResultsCachedKey(widgetId, user.LibraryProfileKey, GetUserMode(user));
                var cachedKey = support.User.ToUserMode().ToRealTimeShowCaseCachedKey(support.User.LibraryProfileKey, widgetId);
                if (cachedKey != Guid.Empty)
                {
                    var uxAdminSortOptions = _session.ResultsOption;
                    if (uxAdminSortOptions != null)
                    {
                        var searchTerm = new List<SearchTerm>
                        {
                            new SearchTerm
                                {
                                    index = 1,
                                    option = "",
                                    indexLabel = "",
                                    @operator = "",
                                    phrase = "",
                                    phraseType = ""
                                }
                        };
                        bool hasCachedResults = _searchProvider.HasSeachResults(new AdvancedSearchJSON { SearchId = cachedKey, resources = new List<int>(), searchTerms = searchTerm, pageSize = 20 }, support.User);
                        if (hasCachedResults)
                        {
                            var results = _searchProvider.PollForResultsBy(cachedKey, 0, 20, support, 0,  _session);
                            if (results != null && results.TotalMatchedRecords > 0)
                            {
                                showCaseItems = results.Clusters.Select(data => new FixedShowcaseItem
                                {
                                    Author = data.Groupings[0].Items[0].Author,
                                    Title = data.Groupings[0].Items[0].Title,
                                    FormatType = data.Groupings[0].MaterialType,
                                    FullRecordUrl = String.Format("#fullrecord/sc/{0}/{1}", data.Groupings[0].Items[0].ItemID, data.Groupings[0].Items[0].ResourceID),
                                    JackArtUrl = data.Groupings[0].Items[0].JacketArtUrl,
                                    PubYear = data.Groupings[0].Items[0].PublishedYear,
                                    JacketArtNote = data.Groupings[0].Items[0].JacketArtNote
                                }).ToList();
                               
                            }
                        }
                    }
                }
                return showCaseItems;
            }
            catch (Exception ex)
            {
                var error = new List<FixedShowcaseItem>
                    {
                        new FixedShowcaseItem()
                            {
                                Title = ex.ToString(),
                                Author = ex.ToString(),
                            }
                    };                
                return error;
            }            
        }

        [HttpPost]
        public JsonResult GetPageContent(PageList myPageInput)
        {           
            User user = _session.ClientSession;
            var myPageData = _pageManagerProvider.GetPageDetailById(myPageInput.Index, user);
            var serializerx = new JavaScriptSerializer();            
            serializerx.RegisterConverters(new[] { new DynamicJsonConverter() });
            //dynamic data = serializerx.Deserialize<object>(myPageData.JsonData);

            //var isLocal = !myPageData.UseGlobal;
            //dynamic defaultPagePostion = isLocal ? serializerx.Deserialize<object>(myPageData.LocalTemplate) : serializerx.Deserialize<object>(myPageData.GlobalTemplate);
            //var widgetDetails = isLocal ? myPageData.LocalWidgetDetails : myPageData.GlobalWidgetDetails;
            //var pagePostion = serializerx.Deserialize<object>(isLocal ? defaultPagePostion.LocalTemplate : defaultPagePostion.GlobalTemplate);

            var model = new List<PageContentModel>();
       
            if (myPageData.UseGlobal)
            {
                var pageTemplate = myPageData.GlobalTemplate;
                if ( (myPageData.GlobalTemplate != null) && (pageTemplate.Length > 0))
                {
                    dynamic defaultPagePostion = serializerx.Deserialize<object>(myPageData.GlobalTemplate);
                    var widgetDetails = myPageData.GlobalWidgetDetails;
                    var pagePostion = serializerx.Deserialize<object>(defaultPagePostion.GlobalTemplate);
                    foreach (var d in pagePostion)
                    {
                        model.AddRange(from p in widgetDetails
                                       where int.Parse(p.Key) == d.id
                                       select new PageContentModel { LocX = d.row, LocY = d.col, PageContent = MyCleanPageContent(p.Value, p.Key)/* p.Value*/ });
                    }                      
                }
            }
            else
            {
                var pageTemplate = myPageData.LocalTemplate;
                if ((myPageData.LocalTemplate != null) && (pageTemplate.Length > 0))
                {
                    dynamic defaultPagePostion = serializerx.Deserialize<object>(myPageData.LocalTemplate);
                    var widgetDetails = myPageData.LocalWidgetDetails;
                    var pagePostion = serializerx.Deserialize<object>(defaultPagePostion.LocalTemplate);
                    foreach (var d in pagePostion)
                    {
                        model.AddRange(from p in widgetDetails
                                       where int.Parse(p.Key) == d.id
                                       select new PageContentModel { LocX = d.row, LocY = d.col, PageContent = MyCleanPageContent(p.Value, p.Key)/* p.Value*/ });
                    }   
                }                 
            }          
            return new AgJson(model);
        }

        [HttpPost]
        public JsonResult GetPageData(string index, bool isGlobal)
        {
            User user = _session.ClientSession;
       
            //var checkGlobal = (int.Parse(myPageInput.Index.Split('|')[0]) > -1) ? true : false;
            //object selectedLibraryData = null;
            //if (checkGlobal)
            //{
            //    //libraryList = loginSettings.Libraries;
            //    var applyLibraryList = _pageManagerProvider.GetGlobalPageAssignmentsForPage(int.Parse(myPageInput.Index.Split('|')[0]), user);
            //    selectedLibraryData = from x in applyLibraryList
            //                              select x.AGLibID.Replace(" ", "");
            //}
           
        

        var myPageData = _pageManagerProvider.GetPageDetailById(index, user);
            var tempPageData = new PageManagerModel
                {
                    PageCollector = myPageData,
                    IsCustomerSuperUser = user.IsCustSuper
                    //Librarys = (List<Field>) libraryList,
                   // SelectedApplyGlobalLibrary = (IEnumerable<string>) selectedLibraryData,
                   // ShowGlobalView = checkGlobal
                };

            return new AgJson(tempPageData);
        }

        [HttpPost]
        public JsonResult GetGlobalPageData(string index, bool isGlobal)
        {
            var user = _session.ClientSession;
            var tempPageData = new PageManagerModel{ IsCustomerSuperUser = user.IsCustSuper };
            int pageId;
            tempPageData.PageCollector = int.TryParse(index.Split('|')[0], out pageId) ? _pageManagerProvider.GetGlobalPageDetailById(pageId, user) : new PageCollector();
            return new AgJson(tempPageData);
        }

        [HttpPost]
        public JsonResult UpdatePage(UpdatePageCollector myPageDataCollection)
        {
            User user = _session.ClientSession;

            var jsonSerializer = new JavaScriptSerializer();

            var myData = new InsertPageObject
                {
                IsLocalPage = myPageDataCollection.IsLocalPage,
                IsGlobalPage = myPageDataCollection.IsGlobalPage,
                ForceLibraryToUseGlobal = myPageDataCollection.ForceLibraryToUseGlobal,
                SelectedLibrariesToForceUseGlobal = string.IsNullOrEmpty(myPageDataCollection.SelectedLibrariesToForceUseGlobal) ? "" : myPageDataCollection.SelectedLibrariesToForceUseGlobal,
                PageTitleName = myPageDataCollection.PageTitleName,
                PageDescToolTip = myPageDataCollection.PageDescToolTip,
                PageType = myPageDataCollection.PageType,
                PageTypeId = int.Parse(myPageDataCollection.PageTypeId.ToString(CultureInfo.InvariantCulture)),
                PageTypeEnable = myPageDataCollection.PageTypeEnable,
                EnableGuests = myPageDataCollection.EnableGuests,
                EnablePatrons = myPageDataCollection.EnablePatrons,
                EnableStaff = myPageDataCollection.EnableStaff,
                DefaultPage = myPageDataCollection.DefaultPage,
                LocalTemplate = myPageDataCollection.LocalTemplate,
                GlobalTemplate = myPageDataCollection.GlobalTemplate,
                PageUrl = string.IsNullOrEmpty(myPageDataCollection.PageUrl) ? "" : myPageDataCollection.PageUrl
            };

            bool isNewWidgetPage;
            if (myPageDataCollection.IsGlobalPage)
            {
                isNewWidgetPage = int.Parse(myPageDataCollection.PageId.Split('|')[0]) == 0;
            }
            else
            {
                isNewWidgetPage = int.Parse(myPageDataCollection.PageId.Split('|')[1]) == 0;
            }
            var myInsertJsonData = jsonSerializer.Serialize(myData);
            UxSaveResult rslt = isNewWidgetPage ? _pageManagerProvider.InsertPage(myInsertJsonData, user) : _pageManagerProvider.UpdatePageById(myPageDataCollection.PageId.ToString(CultureInfo.InvariantCulture), myInsertJsonData, user);
            rslt.Message = rslt.Status ? "Your Page has been Saved." : rslt.Message;
            return Json(rslt);
        }
        [HttpPost]
        public JsonResult InsertPage(PageCollector myPageDataCollection)
        {
            User user = _session.ClientSession;
            
            var jsonSerializer = new JavaScriptSerializer();
            var myCollectedList = (IDictionary<string, object>)jsonSerializer.DeserializeObject(myPageDataCollection.JsonData);
            var defaultType = myCollectedList.ContainsKey("DefaultPageType")
                                  ? myCollectedList["DefaultPageType"].ToString()
                                  : myPageDataCollection.IsGlobalPage ? "Global" : "Locals";

            var myData = new InsertPageObject
                {                    
                    IsLocalPage = myPageDataCollection.IsLocalPage,
                    IsGlobalPage = myPageDataCollection.IsGlobalPage,
                    ForceLibraryToUseGlobal = myPageDataCollection.IsGlobalPage && myPageDataCollection.ForceLibraryToUseGlobal,
                    SelectedLibrariesToForceUseGlobal = myPageDataCollection.IsGlobalPage? myPageDataCollection.ApplyToListLibrary.Replace("\\","").Replace("[","").Replace("]","").Replace("\"","") :"",
                    PageTitleName = myCollectedList["PageTitleName"].ToString(),
                    PageType = myCollectedList["PageType"].ToString(),
                    PageTypeId = int.Parse(myCollectedList["PageTypeId"].ToString()),
                    PageTypeEnable = Convert.ToBoolean(myCollectedList["PageTypeEnable"]),
                    EnableGuests = Convert.ToBoolean(myCollectedList["EnableGuests"]),
                    EnablePatrons = Convert.ToBoolean(myCollectedList["EnablePatrons"]),
                    EnableStaff = Convert.ToBoolean(myCollectedList["EnableStaff"]),
                    DefaultPage = defaultType,
                    LocalTemplate = myPageDataCollection.IsLocalPage ? jsonSerializer.Serialize(myCollectedList["WidgetLocation"]):"[]",
                    GlobalTemplate = myPageDataCollection.IsGlobalPage ? jsonSerializer.Serialize(myCollectedList["WidgetLocationGlobal"]):"[]",
                    PageUrl = myCollectedList["PageURL"].ToString()
                };
            var myInsertJsonData = jsonSerializer.Serialize(myData);
            var rslt = _pageManagerProvider.InsertPage(myInsertJsonData, user);
            rslt.Message = rslt.Status ? "Your Page has been created." : rslt.Message;
            return Json(rslt);
        }

        [HttpPost]
        public JsonResult DeletePage(PageList myPageInput)
        {
            User user = _session.ClientSession;
            _pageManagerProvider.DeletePageById(myPageInput.Index, user);
            return Json("Your Page has been deleted");
        }
        [HttpPost]
        public JsonResult DeleteGlobalPage(GlobalPageList myPageInput)
        {
            User user = _session.ClientSession;
            _pageManagerProvider.DeleteGlobalPageById(myPageInput.Index, user);
            return Json("Your Page has been deleted");
        }
        [HttpPost]
        public JsonResult UpdatePageOrder(PageCollector myPageDataCollection)
        {
            User user = _session.ClientSession;
            _pageManagerProvider.UpdatePageOrder(myPageDataCollection.JsonData, user);
            return Json("You Page has been created", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetGlobalPageAssignmentsForPage(PageList myPageInput)
        {
            User user = _session.ClientSession;
            var applyLibraryList = _pageManagerProvider.GetGlobalPageAssignmentsForPage(int.Parse(myPageInput.Index.Split('|')[0]), user);

            var model = from x in applyLibraryList
                        select x.AGLibID.Replace(" ", "");

            return new AgJson(model, JsonRequestBehavior.AllowGet);
        }
    }


    public class InsertPageObject
    {
        public int PageTypeId { get; set; }
        public string PageType { get; set; }
        public bool PageTypeEnable { get; set; }
        public bool IsLocalPage { get; set; }
        public bool IsGlobalPage { get; set; }
        public bool ForceLibraryToUseGlobal { get; set; }
        public string SelectedLibrariesToForceUseGlobal { get; set; }
        public string PageTitleName { get; set; }
        public string PageDescToolTip { get; set; }
        public bool EnableGuests { get; set; }
        public bool EnablePatrons { get; set; }
        public bool EnableStaff { get; set; }
        public string DefaultPage { get; set; }
        public string LocalTemplate { get; set; }
        public string GlobalTemplate { get; set; }

        public string PageUrl { get; set; }
    }

    
    public class ApplyLibList
    {
        public string Value { get; set; }
    }
    public class ApplySelectedLibraryList
    {
        public List<Field> Libraries { get; set; }
        public IEnumerable<string> SelectedApplyGlobalLibrary { get; set; }
        public string DefaultLibrary { get; set; }
        public string LockoutLibrary { get; set; }
    }
    public class PageManagerModel
    {
        public PageCollector PageCollector { get; set; }
        public List<Field> Librarys { get; set; }
        public IEnumerable<string> SelectedApplyGlobalLibrary { get; set; }
        public bool ShowGlobalView { get; set; }
        public bool IsCustomerSuperUser { get; set; }
        
        
    }
    public class PageContentModel
    {
        public int LocX { get; set; }
        public int LocY { get; set; }
        public string PageContent { get; set; }
        public string ResourceMessage { get; set; }
    }
    public class PageGlobalModel
    {
        public string Index { get; set; }
        public bool UseGlobal { get; set; }
    }

    public class FixedListShowcase
    {
        public List<FixedShowcaseItem> Items { get; set; }
        public bool NoResources { get; set; }
        public string Message { get; set; }    
    }

}
