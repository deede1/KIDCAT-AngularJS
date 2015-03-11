using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VersoMVC.Areas.SystemSettings.Models
{
    public class EditPageModel
    {
        public string PageName { get; set; }
        public List<PageType> PageTypesList { get; set; }
        public string PageURL { get; set; }
        public bool EnableForPatrons { get; set; }
        public bool EnableForStaff { get; set; }
    }

    public class ExistingPage
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string PageType { get; set; }
        public string PageURL { get; set; }
        public bool EnableForPatrons { get; set; }
        public bool EnableForStaff { get; set; }
    }

    public class SavedPagesList
    {
        public List<ExistingPage> EditablePages { get; set; }
        public EditPageModel EditPageModel { get; set; }
    }
}