using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VersoMVC.Areas.SystemSettings.Models
{
    public class CreateWidgetModel
    {
        public List<WidgetType> WidgetTypesList { get; set; }
    }

    public class WidgetType
    {
        public int Index { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }
}