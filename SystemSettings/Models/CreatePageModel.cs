using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VersoMVC.Infrastructure;

namespace VersoMVC.Areas.SystemSettings.Models
{
    public class CreatePageModel
    {
        public string Index { get; set; }
        public string PageName { get; set; }
        public string PageType { get; set; }
        public List<PageType> PageTypesList { get; set; }
        public int SelectedPageType { get; set; }
        public string PageURL { get; set; }
        public bool EnableForPatrons { get; set; }
        public bool EnableForStaff { get; set; }
    }

    public class PageType
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    // No references - dead code? Bart 2013-08-06
    //public class CreatePageSampleData
    //{
    //    string PageName = "MyPageName";
    //    List<PageType> PageList = new List<PageType>
    //    {
    //        new PageType {Index = 1, Name = "Widget Page" },
    //        new PageType {Index = 2, Name = "Link Page" }
    //    };
    //    string PageURL = ConfigurationHelper.ContextPath + "/controlpanel/browseadmin/";
    //    bool EnableForPatrons = true;
    //    bool EnableForStaff = false;
    //}
}