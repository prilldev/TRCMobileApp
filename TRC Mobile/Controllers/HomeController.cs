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

            //if no Series was found (ie, when "All Messages" was clicked)
            //- create new instance and set some defaults
            if (selSeries == null)
            {
                selSeries = new SeriesListModel();
                selSeries.Title = "All Messages";
                selSeries.Description = "List of All Message...";
                selSeries.DescriptionHTML = "<p></p>";
            }

            ViewData.Add("FBCode", id);
            ViewData.Add("DescriptionHTML", selSeries.DescriptionHTML);
            ViewBag.Title = selSeries.Title + " | TRC Mobile";
            ViewBag.Message = selSeries.Description;

            return View(TRC_Mobile.Models.SeriesModel.SeriesMessages(selSeries.feedURLBase + id));
        }

        public ActionResult MessageDetail(string serId, string msgDate)
        {
            ViewData.Add("SerId", serId);

            var mDet = new MessageModel();
            var serMsgs = SeriesModel.SeriesMessages(mDet.feedURLBase + serId);
            mDet = (from md in serMsgs
                          where (md.PublishedDate[2] + "-" + md.PublishedDate[1]).ToString() == msgDate
                          select md).First();

            return View(mDet);
        }

    }
}
