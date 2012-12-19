using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TRC_Mobile.Models;

namespace TRC_Mobile.Controllers
{
    public class HomeController : Controller
    {
        private IList<SeriesListModel> TRCSeriesList;
        private string urlSeriesList = "http://trcmedia.mainewebpress.net/category/trcb-series/feed/";

        public ActionResult Index()
        {
            ViewBag.Message = "Home | TRC Mobile App";
            
            TRCSeriesList = TRC_Mobile.Models.SeriesModel.SeriesList(urlSeriesList);
            return View(TRCSeriesList);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Series(string id)
        {
            TRCSeriesList = TRC_Mobile.Models.SeriesModel.SeriesList(urlSeriesList);
            var selSeries = (from s in TRCSeriesList
                              where s.FBCode == id
                              select s).LastOrDefault();

            ViewData.Add("FBCode", id);
            ViewData.Add("DescriptionHTML", (selSeries == null ? "<p></p>" : selSeries.DescriptionHTML));
            ViewBag.Title = (selSeries == null ? "All" : selSeries.Title) + " | TRC Mobile";
            ViewBag.Message = (selSeries == null ? "List of All Messages..." : selSeries.Description);

            string urlBase = "http://feeds.feedburner.com/";

            return View(TRC_Mobile.Models.SeriesModel.SeriesMessages(urlBase + id));
        }


    }
}
