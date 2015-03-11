using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Verso.Common.Contracts;
using Verso.Common.Contracts.UXAdmin;
using Verso.Common.Extensions;
using Verso.Common.SearchProvider;
using Verso.Common.UXAdmin;
using Verso.Common.UserProvider;
using Verso.Common.WidgetProvider;
using VersoMVC.Areas.SystemSettings.Models;
using VersoMVC.Models;
using WCF.DAL;
using VersoMVC.Controllers.Helper;

//using LocalProviders.Interface;
namespace VersoMVC.Areas.SystemSettings.Controllers
{
    public class WidgetManagerController : Controller
    {
        private readonly ISearchProvider _searchProvider;
        private readonly ISessionProvider _sessionProvider;
        private readonly IWidgetManagerProvider _widgetManagerProvider;
        private readonly ILibraryConfigProvider _libraryConfigProvider;
        private readonly IPersistenLinkProvider _persistenLinkProvider;
        private readonly IItemProvider _itemProvider;
        private readonly IAdvancedSearchProvider _advancedSearchProvider;
        private readonly IPageManagerProvider _pageManagerProvider;


        //private readonly IZUxAdminWidgetProvider _zUxAdminWidgetProvider; //uxAdmin Widget V2

        public WidgetManagerController(IWidgetManagerProvider widgetManagerProvider, ISessionProvider sessionProvider,
                                       ISearchProvider searchProvider, ILibraryConfigProvider libraryConfigProvider, IItemProvider itemProvider, IPersistenLinkProvider persistenLinkProvider,
                                        IAdvancedSearchProvider advancedSearchProvider, IPageManagerProvider pageManagerProvider
            //,IZUxAdminWidgetProvider zUxAdminWidgetProvider //uxAdmin Widget V2
            )
        {
            _widgetManagerProvider = widgetManagerProvider;
            _sessionProvider = sessionProvider;
            _searchProvider = searchProvider;
            _libraryConfigProvider = libraryConfigProvider;
            _itemProvider = itemProvider;
            _persistenLinkProvider = persistenLinkProvider;
            _advancedSearchProvider = advancedSearchProvider;
            _pageManagerProvider = pageManagerProvider;
            // _zUxAdminWidgetProvider = zUxAdminWidgetProvider; //uxAdmin Widget V2
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateWidget()
        {
            return View();
        }

        public ActionResult GetWidgetInfoView(string id, bool isGlobal = false, bool isLocal = false, string typeId = "-1", string typeName = "")
        {
            User user = _sessionProvider.ClientSession;
            var additionalJson = "";

            int work;
            if (!int.TryParse(id, out work)) { work = 0; }
        
            if (work > 0)
            {
                WidgetCollector myWidgetData = _widgetManagerProvider.GetWidgetDetailById(id, user);
                var serializerx = new JavaScriptSerializer();
                serializerx.RegisterConverters(new[] { new DynamicJsonConverter() });
                dynamic data = serializerx.Deserialize<object>(myWidgetData.JsonData);
                var widgetCollector = new WidgetCollector();
                switch ((int)data.WidgetTypeId)
                {
                    case 4:
                        var jserializer = new JavaScriptSerializer();
                        AutoGraphics.Verso.TransferObjects.ConfigSystem configSystem = SvcCache.LoadSystemConfig(user);                        
                        AdvancedSearchFields advancedSearchFields = _advancedSearchProvider.GetAdvancedSearchFields(user, "", configSystem);                        
                        var myResourceGroup = new List<ResourceGroup>();
                        foreach (var resGroup in advancedSearchFields.ResourceGroups)
                        {
                            var myResourceNode = new List<ResourceNode>();
                            myResourceNode.AddRange(resGroup.ResourceNodes.Where(resNode => resNode.ResourceType.ToLower() == "ag" || resNode.ResourceType.ToLower() == "av" ));
                            if (myResourceNode.Count > 0)
                            {
                                myResourceGroup.Add(new ResourceGroup()
                                    {
                                        Collapse = resGroup.Collapse,
                                        ID = resGroup.ID,
                                        Name = resGroup.Name,
                                        ResourceNodes = myResourceNode
                                    });
                            }                            
                        }                       
                        string formatGroupList = jserializer.Serialize(advancedSearchFields.FormatGroupList);
                        string materialTypes = jserializer.Serialize(advancedSearchFields.MaterialTypes);
                        string resourceGroupList = jserializer.Serialize(myResourceGroup);
                        string searchIndexList = jserializer.Serialize(advancedSearchFields.SearchIndexTypes);
                        string scopeList = jserializer.Serialize(advancedSearchFields.LibraryLocations);
                        string libraryLicenseType = jserializer.Serialize(_sessionProvider.LibrarySettingsSession.LicenseType);
                        additionalJson = "{\"MaterialTypes\": " + materialTypes + ",\"FormatsGroupList\": " + formatGroupList + ", \"ResourceGroupList\": " + resourceGroupList + ",\"SearchIndexList\": " + searchIndexList + ",\"ScopeList\": " + scopeList + " , \"LibraryLicenseType\": " + libraryLicenseType + "}";          
                        widgetCollector.PreDefineData = additionalJson;
                        widgetCollector.JsonData = myWidgetData.JsonData;
                        widgetCollector.WidgetForGlobal = myWidgetData.WidgetForGlobal;
                        widgetCollector.WidgetForLocal = myWidgetData.WidgetForLocal;
                        widgetCollector.WidgetId = myWidgetData.WidgetId;
                        break;
                    default:
                        widgetCollector.JsonData = myWidgetData.JsonData;
                        widgetCollector.WidgetForGlobal = myWidgetData.WidgetForGlobal;
                        widgetCollector.WidgetForLocal = myWidgetData.WidgetForLocal;
                        widgetCollector.WidgetId = myWidgetData.WidgetId;
                        break;
                }
                var widgetTypeName = data.WidgetType;
                if (!string.IsNullOrEmpty(widgetTypeName))
                {
                    return View(@"~\Areas\SystemSettings\Views\WidgetManager\" + data.WidgetType + ".cshtml", widgetCollector);        
                }
                else
                {
                    return View(@"~\Areas\SystemSettings\Views\WidgetManager\NoWidgetPage.cshtml");
                }
            }
            else
            {       
                switch (typeId)
                {
                    case "4":
                        var jserializer = new JavaScriptSerializer();
                        AutoGraphics.Verso.TransferObjects.ConfigSystem configSystem = SvcCache.LoadSystemConfig(user);                        
                        AdvancedSearchFields advancedSearchFields = _advancedSearchProvider.GetAdvancedSearchFields(user, "", configSystem);
                        var myResourceGroup = new List<ResourceGroup>();
                        foreach (var resGroup in advancedSearchFields.ResourceGroups)
                        {
                            var myResourceNode = new List<ResourceNode>();
                            myResourceNode.AddRange(resGroup.ResourceNodes.Where(resNode => resNode.ResourceType.ToLower() == "ag" || resNode.ResourceType.ToLower() == "av"));
                            if (myResourceNode.Count > 0)
                            {
                                myResourceGroup.Add(new ResourceGroup()
                                    {
                                        Collapse = resGroup.Collapse,
                                        ID = resGroup.ID,
                                        Name = resGroup.Name,
                                        ResourceNodes = myResourceNode
                                    });
                            }                           
                        }
                        string formatGroupList = jserializer.Serialize(advancedSearchFields.FormatGroupList);
                        string materialTypes = jserializer.Serialize(advancedSearchFields.MaterialTypes);
                        string resourceGroupList = jserializer.Serialize(myResourceGroup);
                        string searchIndexList = jserializer.Serialize(advancedSearchFields.SearchIndexTypes);
                        string scopeList = jserializer.Serialize(advancedSearchFields.LibraryLocations);
                        string libraryLicenseType = jserializer.Serialize(_sessionProvider.LibrarySettingsSession.LicenseType);
                       // additionalJson = "{\"FormatsGroupList\": " + formatGroupList + ", \"ResourceGroupList\": " + resourceGroupList + ",\"SearchIndexList\": " + searchIndexList + "}";
                        additionalJson = "{\"MaterialTypes\": " + materialTypes + ",\"FormatsGroupList\": " + formatGroupList + ", \"ResourceGroupList\": " + resourceGroupList + ",\"SearchIndexList\": " + searchIndexList + ",\"ScopeList\": " + scopeList + " , \"LibraryLicenseType\": " + libraryLicenseType + "}";                     
                    break;
                }
                if (!string.IsNullOrEmpty(typeName))
                {
                    return View(@"~\Areas\SystemSettings\Views\WidgetManager\" + typeName + ".cshtml",
                                new WidgetCollector()
                                    {
                                        WidgetId = 0,
                                        WidgetForGlobal = isGlobal,
                                        WidgetForLocal = isLocal,
                                        PreDefineData = additionalJson,
                                        JsonData = "{\"WidgetTypeId\":" + typeId + ",\"WidgetType\":\"" + typeName + "\"}"
                                    });
                }
                else
                {
                    return View(@"~\Areas\SystemSettings\Views\WidgetManager\NoWidgetPage.cshtml");
                }
                
            }
            
        }

        public ActionResult GetWidgetPartialView(string id)
        {
            return PartialView(@"~\Areas\SystemSettings\Views\WidgetManager\" + id + ".cshtml");    
            
        }

        [HttpGet]
        public JsonResult GetWidgetTypeList()
        {
            User user = _sessionProvider.ClientSession;
            List<WidgetTypeList> myWidgetTypeList = _widgetManagerProvider.GetWidgetTypeList(user);

            var data = new WidgetManager
                {
                    WidgetTypesList = myWidgetTypeList,
                    IsCusSuperUser = user.IsCustSuper
                };
            return new AgJson(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetWidgetList()
        {
            User user = _sessionProvider.ClientSession;
            List<WidgetList> myWidgetList = _widgetManagerProvider.GetWidgetResult(user);
            List<WidgetList> myWidgetListGlobal = _widgetManagerProvider.GetWidgetResult(user,true);

            var data = new WidgetResult
                {
                    WidgetList = myWidgetList,
                    WidgetListGlobal = user.IsCustSuper? myWidgetListGlobal:null,
                    IsCustomerSuperUser = user.IsCustSuper
                };
            return new AgJson(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetWidgetData(WidgetList myWidgetInput)
        {
            User user = _sessionProvider.ClientSession;
            WidgetCollector myWidgetData = _widgetManagerProvider.GetWidgetDetailById(myWidgetInput.Index, user);

            return new AgJson(myWidgetData, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateWidget(WidgetCollector myWidgetDataCollection)
        {
            User user = _sessionProvider.ClientSession;
            var rslt = _widgetManagerProvider.UpdateWidgetById(
                myWidgetDataCollection.WidgetId,                                
                myWidgetDataCollection.JsonData,
                user, myWidgetDataCollection.WidgetForGlobal);
            rslt.Message = rslt.Status ? "Your Widget has been updated" : rslt.Message;
                     
            if (myWidgetDataCollection.WidgetId > 0)
            {
                var searchIdList = new List<string>
                    {
                        "G".ToRealTimeShowCaseCachedKey(user.LibraryProfileKey, myWidgetDataCollection.WidgetId).ToString(),
                        "P".ToRealTimeShowCaseCachedKey(user.LibraryProfileKey, myWidgetDataCollection.WidgetId).ToString(),
                        "S".ToRealTimeShowCaseCachedKey(user.LibraryProfileKey, myWidgetDataCollection.WidgetId).ToString()
                    };
                _searchProvider.RemoveSearchResultCache(searchIdList, user);
            }          
            return new AgJson(rslt, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult InsertWidget(WidgetCollector myWidgetDataCollection)
        {
            User user = _sessionProvider.ClientSession;
            var rslts = _widgetManagerProvider.InsertWidget(myWidgetDataCollection.WidgetForLocal,myWidgetDataCollection.WidgetForGlobal, myWidgetDataCollection.JsonData, user);
            rslts.ForEach(r=>r.Message = r.Status ? "Your Widget has been created" : r.Message);
            return new AgJson(rslts, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteLocalWidget(WidgetList myWidgetInput)
        {
            //uxamdin widget v2
            User user = _sessionProvider.ClientSession;
            //_zUxAdminWidgetProvider.DeleteWidget(user, int.Parse(myWidgetInput.Index), "Local");
            return Json("");
        }

        [HttpPost]
        public JsonResult DeleteGlobalWidget(WidgetList myWidgetInput)
        {
            //uxamdin widget v2
            User user = _sessionProvider.ClientSession;
            //_zUxAdminWidgetProvider.DeleteWidget(user, int.Parse(myWidgetInput.Index),"Global");
            return Json("");
        }

        [HttpPost]
        public JsonResult DeleteWidget(WidgetList myWidgetInput)
        {
            User user = _sessionProvider.ClientSession;
            _widgetManagerProvider.DeleteWidgetById(myWidgetInput.Index, user);
            return new AgJson("Your Widget has been deleted", JsonRequestBehavior.AllowGet);
        }

        public ShowCaseWidgetPerform Perform(SearchQuery searchQuery)
        {            
            User user = _sessionProvider.ClientSession;
            Guid searchCacheId = _searchProvider.PerformSearch(searchQuery, user);
            string searchTerm = searchQuery.Filters == null ? "" : string.Join(" ", searchQuery.Filters);
            searchTerm = string.Format("{0} {1}", searchTerm.Replace("\"", "'").Replace("\\", "\\\\"), searchQuery.Query);
            var model = new ShowCaseWidgetPerform()
            {
                SearchID = searchCacheId,
                SearchTerm = searchTerm.Trim().Replace("+", string.Empty),
            };
            return model;
        }
        [HttpPost]
        public JsonResult GetShowCaseGuid()
        {            
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetIndexFormatResource()
        {
            User user =_sessionProvider.ClientSession;
            var formatResourceData = _libraryConfigProvider.GetIndexFormatResource(user);

            return new AgJson(formatResourceData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TopicSearchPredefineData()
        {
            User user = _sessionProvider.ClientSession;
            var biascSearchInfoData = _widgetManagerProvider.GetBisacSearchInfo(user);
            //var formatResourceData = _libraryConfigProvider.GetIndexFormatResource(user);

            var topicSearchRedefineData = new TopicSearchPredefineDataModel
                {
                    MyBisaSearchInfo = biascSearchInfoData.Select(x => new TopicSearchAvailableList()
                        {
                            Id = x.BisacCode,                            
                            Name = x.BisacHeading.Replace("&", "and")
                           /* ,
                            TopicSearchPredefineData,
                            TopicSearchCriteria = new TopicSearchCriteria()
                                {
                                    SearchTeam = new List<SearchTeam>()
                                        {
                                            new SearchTeam()
                                                {
                                                    Index =x.Sl1Index,
                                                    Phase = x.Sl1Term
                                                },
                                            new SearchTeam()
                                                {
                                                    Index =x.Sl2Index,
                                                    Phase = x.Sl2Term
                                                },
                                            new SearchTeam()
                                                {
                                                    Index =x.Sl3Index,
                                                Phase = x.Sl3Term
                                                }

                                        },
                                    MaterialTypes = new string[] { "bks" },
                                    ResourceTypes = new string[] { "1743695" },
                                }
                             */  
                        }),
                    //MyFormatAndResource = formatResourceData
                };
            return new AgJson(topicSearchRedefineData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetShowCaseDisplayItem(string[] memberIdData)
        {
            var support = _sessionProvider.SearchSupport;
            var myShowCasePersistentLink = memberIdData.Select(data => _itemProvider.GetFullRecordBy(data, support)).Select(item => new ShowCasePersistentLink()
                {
                    JacketArt = item.JacketArtUrl,
                    PersistentKey = _persistenLinkProvider.SavePersistentLinkKey(item.PersistenLinkKey, support.User), 
                    Title = item.Title
                }).ToList();

            return new AgJson(myShowCasePersistentLink, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetShowCaseResult(AdvancedSearchJSON mySearchQuery)
        {
            var support = _sessionProvider.SearchSupport;

            var myAdvanceSearchJson = new AdvancedSearchJSON()
            {
                searchTerms = new List<SearchTerm>() {new SearchTerm(){ index = 0, @operator = "", option = "", phrase = "facebook", phraseType = ""}},
                formats = new List<string>() { "", ""},        
                languages = new List<string>() { "english"},
                resources = new List<int>() { 1986705, 1743696, 1743518, 1743582, 1743581, 1743676, 1743519, 1743525, 1743599, 1743521, 1743523, 1743528, 1743548, 1743544, 1743545, 1743547, 1743583, 1743696, 1743549, 1743684, 1743667, 1743666, 1743580, 1743515, 1743516, 1743517, 1743520, 1743522, 1743524, 1743585, 1743526, 1743527, 1743693, 1743529, 1743660, 1743543, 1743656, 1743546, 1743530, 1743531, 1743532, 1743533, 1743534, 1743535, 1743536, 1743537, 1743538, 1743539, 1743540, 1743541, 1743542, 1743586, 1743578, 1743579, 1743694 },        
                library = "",
                libraryLocations = 0,
                materialTypes = new List<string>(){""},
                pageSize = 24,
                pubDate1 = 0,
                pubDate2 = 0,
                pubDateOperator = "",
                sortOrder = "",
                publication = "",        
            };


            Guid searchCacheId = _searchProvider.PerformAdvancedSearch(myAdvanceSearchJson, support.User);
            SearchResult searchResult;
            //do
            //{
            //    searchResult = _searchProvider.PollForResultsBy(searchCacheId, 1, mySearchQuery.pageSize, user, 0);
            //    Thread.Sleep(10000);
            //} while (!searchResult.IsActive);

            searchResult = _searchProvider.PollForResultsBy(searchCacheId, 1, mySearchQuery.pageSize, support, 0, _sessionProvider);
            Thread.Sleep(10000);
            searchResult = _searchProvider.PollForResultsBy(searchCacheId, 1, mySearchQuery.pageSize, support, 0, _sessionProvider);
            var model = new SearchModel
            {
                Clusters = searchResult.Clusters,
                IsActive = searchResult.IsActive,

              //  SearchTerm = searchTerm,                
              //  SearchID = searchCacheID,                
              //  TotalMatchedRecords = searchResult.TotalMatchedRecords,
              //  TotalMergedRecords = searchResult.TotalMergedRecords,
              //  FacetGroups = searchResult.FacetGroups,
              //  ResourceSummaries = searchResult.ResourceSummaries,
              //  StartRecord = startRecord,
              //  RecordCount = count,
              //  IsDebug = isDebug,
              //  GlobalFacets = searchResult.GlobalFacets,
              //  PollID = searchResult.PollID,
              //  FacetType = facettype,
              //  SortBy = UxAdminSortOptions.SortByOption,
              //  DefaultSortBy = UxAdminSortOptions.DefaultOption,
              //  ProgressPercentage = searchResult.ProgressPercentage,
              //  NumberResource = searchResult.NumberResource,
              //  ResourceReturn = searchResult.ResourceReturn
            };

            //var member = "cbb9e2cc-9c4f-445b-bd9f...7c51e846,0,16,0,1,bks,1";
            //var x = _itemProvider.GetFullRecordBy(member, user);



            return new AgJson(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ShowCaseResult(ShowCaseSearchQuery mySearchWidget)
        {
            Guid searchCacheId = mySearchWidget.SearchCacheId;
            int startRecord = mySearchWidget.StartRecord;
            int count = mySearchWidget.Count;
            string searchTerm = mySearchWidget.SearchTerm;
            bool isDebug = mySearchWidget.IsDebug;
            string facettype = mySearchWidget.Facettype;

            var support = _sessionProvider.SearchSupport;
            SearchModel model;

            SearchResult searchResult = _searchProvider.PollForResultsBy(searchCacheId, startRecord, count, support, 0, _sessionProvider);
                model = new SearchModel
                    {
                        //SearchTerm = searchTerm,
                        Clusters = searchResult.Clusters,
                        SearchID = searchCacheId,
                        IsActive = searchResult.IsActive,
                        TotalMatchedRecords = searchResult.TotalMatchedRecords,
                        TotalMergedRecords = searchResult.TotalMergedRecords,
                        FacetGroups = searchResult.FacetGroups,
                        //ResourceSummaries = searchResult.ResourceSummaries,
                        //StartRecord = startRecord,
                        //RecordCount = count,
                        //IsDebug = isDebug,
                        GlobalFacets = searchResult.GlobalFacets,
                        PollID = searchResult.PollID,
                        //FacetType = facettype
                    };


                return new AgJson(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDrawImage(ImageCollector myWidgetDataCollection)
        {
            string imageFilename = myWidgetDataCollection.ImageFilename;
            string base64 = myWidgetDataCollection.Base64.Split(',')[1];


            using (var ms = new MemoryStream(Convert.FromBase64String(base64)))
            {
                using (var bm2 = new Bitmap(ms))
                {
                    if (Debugger.IsAttached)
                    {
                        bm2.Save(Path.Combine(Server.MapPath(Url.Content("~/download_temp/uxadmin/")), imageFilename));
                    }
                    else
                    {
                        if (!(Directory.Exists(Server.MapPath("/download_temp/uxadmin/"))))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(Server.MapPath("/download_temp/uxadmin/"));
                        }

                        bm2.Save(Path.Combine(Server.MapPath("/download_temp/uxadmin/"), imageFilename));
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public WrappedJsonResult UploadImage(HttpPostedFileWrapper imageFile)
        {
            if (imageFile == null || imageFile.ContentLength == 0)
            {
                return new WrappedJsonResult
                    {
                        Data = new
                            {
                                IsValid = false,
                                Message = "No file was uploaded.",
                                ImagePath = string.Empty
                            }
                    };
            }

            string fileName = String.Format("{0}.jpg", Guid.NewGuid().ToString());
            //var imagePath = Path.Combine(Server.MapPath("/download_temp/uxadmin/"), fileName);


            if (Debugger.IsAttached)
            {
                imageFile.SaveAs(Path.Combine(Server.MapPath(Url.Content("~/download_temp/uxadmin/")), fileName));
                return new WrappedJsonResult
                    {
                        Data = new
                            {
                                IsValid = true,
                                Message = string.Empty,
                                ImagePath = Path.Combine(Url.Content("~/download_temp/uxadmin/"), fileName)
                            }
                    };
            }
            else
            {
                if (!(Directory.Exists(Server.MapPath("/download_temp/uxadmin/"))))
                {
                    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath("/download_temp/uxadmin/"));
                }

                imageFile.SaveAs(Path.Combine(Server.MapPath("/download_temp/uxadmin/"), fileName));
                return new WrappedJsonResult
                    {
                        Data = new
                            {
                                IsValid = true,
                                Message = string.Empty,
                                ImagePath = Url.Content(String.Format("/download_temp/uxadmin/{0}", fileName))
                            }
                    };
            }
        }
    }

    public class WrappedJsonResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.Write("<html><body><textarea id=\"jsonResult\" name=\"jsonResult\">");
            base.ExecuteResult(context);
            context.HttpContext.Response.Write("</textarea></body></html>");
            context.HttpContext.Response.ContentType = "text/html";
        }
    }

    public class ShowCasePersistentLink
    {
        public string Title { get; set; }
        public string JacketArt { get; set; }
        public string PersistentKey { get; set; }
    }

    public class TopicSearchPredefineDataModel
    {
        public IEnumerable<TopicSearchAvailableList> MyBisaSearchInfo { get; set; }
        //public IndexFormatResource MyFormatAndResource { get; set; }                
        //public List<BisacSearchInfo> MyBisaSearchInfo { get; set; }
        //{ Id: "1", SystemPredefine: true, Name: "Archaeology", MaterialTypes: [""], ResourceTypes: [""], TopicSearchCriteria: { SearchTeam: [{ Index: "", Phase: "Archaeology", Condition: "" }, { Index: "", Phase: "", Condition: "" }, { Index: "", Phase: "", Condition: ""}]}}]

    }
    public class TopicSearchAvailableList
    {
        public string Id { get; set; }
        public string Name { get; set; }
      //public bool SystemPredefine { get; set; }
      //public TopicSearchCriteria TopicSearchCriteria { get; set; }
    }
    public class TopicSearchCriteria
    {
        public List<SearchTeam> SearchTeam { get; set; }
        public string[] MaterialTypes { get; set; }
        public string[] ResourceTypes { get; set; }
        
    }
    public class SearchTeam
    {
        public int Index { get; set; }
        public string Phase{ get; set; }
        public string Condition{ get; set; }
    }
}
