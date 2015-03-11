using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VersoMVC.Areas.SystemSettings.Models
{
    public class ResultsOptions
    {
        public List<DropDownOption> ResultsViewOptions { get; set; }
        public List<DropDownOption> ResultsTypeOptions { get; set; }
        public int PatronsDefaultResultsView { get; set; }
        public int StaffDefaultResultsView { get; set; }
        public int PatronsDefaultResultsType { get; set; }
        public int StaffDefaultResultsType { get; set; }

        public List<DropDownOption> ResultsSortOptions { get; set; }
        public int PatronsDefaultResultsSort { get; set; }
        public int StaffDefaultResultsSort { get; set; }

        public List<DropDownOption> ClusteredSequenceOptions { get; set; }
        public int SelectedClusteredSequence { get; set; }

        public List<BooleanDropDownOption> DisplayPrintOptions { get; set; }
        public bool PatronsDisplayPrintOption { get; set; }
        public bool StaffDisplayPrintOption { get; set; }

        public List<BooleanDropDownOption> DisplayGridJacketOptions { get; set; }
        public bool PatronsDisplayGridJacket { get; set; }
        public bool StaffDisplayGridJacket { get; set; }

        public List<BooleanDropDownOption> DisplayRSSFeedOptions { get; set; }
        public bool PatronsDisplayRSSFeed { get; set; }
        public bool StaffDisplayRSSFeed { get; set; }

        public List<IntDropDownOption> ItemsPerScreenOptions { get; set; }
        public int SelectedItemsPerScreen { get; set; }
        public bool IsNewCustomer { get; set; }

        public List<BooleanDropDownOption> BooleanListOptions { get; set; }
    }

    public class DropDownOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class BooleanDropDownOption
    {
        public bool Value { get; set; }
        public string Name { get; set; }
    }

    public class IntDropDownOption
    {
        public int Value { get; set; }
        public string Name { get; set; }
    }

}