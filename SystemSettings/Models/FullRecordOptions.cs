using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VersoMVC.Areas.SystemSettings.Models
{
    public class FullRecordOptions
    {
        public bool DisplayMARCViewForPatrons { get; set; }
        public bool DisplayMARCViewForStaff { get; set; }

        public bool DisplaySubjectAccordionForPatrons { get; set; }
        public bool DisplaySubjectAccordionForStaff { get; set; }

        public bool DisplayAuthorAccordionForPatrons { get; set; }
        public bool DisplayAuthorAccordionForStaff { get; set; }

        public bool DisplayOnlineContentAccordionForPatrons { get; set; }
        public bool DisplayOnlineContentAccordionForStaff { get; set; }

        public bool DisplayRSSFeedsForPatrons { get; set; }
        public bool DisplayRSSFeedsForStaff { get; set; }

        public bool EnablePurchaseThisItem { get; set; }
        public bool EnableSuggestForPurchase { get; set; }
        public string PurchaseThisItemLabel { get; set; }
        public string SuggestForPurchaseLabel { get; set; }

        public bool IsNewCustomer { get; set; }
    }
}