using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Buttr.Web.ViewModels
{
    public class HomeViewModel
    {
        [DisplayName("Counties")]
        public MultiSelectList CountySelectList { get; set; }
        public List<string> SelectedCounties { get; set; }
        [DisplayName("Exclusions")]
        public string ExcludedZips { get; set; }        
        public string GeneratedZips { get; set; }
        public string IncludeZips { get; set; }
    }
}