using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Buttr.Data.Repositories;
using Buttr.Domain;
using Buttr.Service;
using Buttr.Web.ViewModels;

namespace Buttr.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var model = new HomeViewModel();
            ViewBag.Message = "Your application description page.";

            List<SelectListItem> countyList = GetCountySelectList();

            model.CountySelectList = new MultiSelectList(countyList, "Value", "Text");

            return View(model);
        }

       [HttpPost]
        public ActionResult Index(HomeViewModel model)
       {
           var zipService = new ZipService();

            List<SelectListItem> countyList = GetCountySelectList(model.SelectedCounties);

            model.CountySelectList = new MultiSelectList(countyList, "Value", "Text");

           var excludeList = new List<string>();
           if (model.ExcludedZips != null)
           {
               excludeList = new List<string>(model.ExcludedZips.Split(new char[]{','}));
           }

           var includeList = new List<string>();
           if (model.IncludeZips != null)
           {
               includeList = new List<string>(model.IncludeZips.Split(new char[] { ',' }));
           }

           if (model.SelectedCounties != null)
           {
               var zipsFound = zipService.GetZips(model.SelectedCounties, excludeList, includeList);
               model.GeneratedZips = string.Join(System.Environment.NewLine, zipsFound);
           }

            return View(model);
        }

        public List<SelectListItem> GetCountySelectList(List<String> SelectedCounties = null)
        {
            var fipsRepository = new FipsRepository();
            var counties = fipsRepository.GetFipsByState("CA");

            var countyList = new List<SelectListItem>();
            foreach (Fips f in counties)
            {                
                var fipCode = string.Format("{0}{1}", f.StateFips, f.CountyFips);
                bool selected = (SelectedCounties != null && SelectedCounties.Contains(fipCode));
                countyList.Add(new SelectListItem { Text = f.County, Value = fipCode, Selected = selected });
            }

            return countyList;
        }

        [HttpGet]
        public ActionResult About()
        {
            return View();
        }
    }
}