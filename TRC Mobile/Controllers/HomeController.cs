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
            var SelectedSeries = (from s in TRCSeriesList
                              where s.FBCode == id
                              select s).LastOrDefault();

            //if no Series was found (ie, when "All Messages" was clicked)
            //- create new instance and set some defaults
            if (SelectedSeries == null)
            {
                SelectedSeries = new SeriesListModel();
                SelectedSeries.Title = "All Messages";
                SelectedSeries.Description = "List of All Message...";
                SelectedSeries.DescriptionHTML = "<p></p>";
            }

            ViewData.Add("FBCode", id);
            ViewData.Add("DescriptionHTML", SelectedSeries.DescriptionHTML);
            ViewData.Add("SeriesImageURL", SelectedSeries.ImageURL);
            ViewBag.Title = SelectedSeries.Title + " | TRC Mobile";
            ViewBag.Message = SelectedSeries.Description;

            var sm = SeriesModel.SeriesMessages(SelectedSeries.feedURLBase + id);


            //return View(TRC_Mobile.Models.SeriesModel.SeriesMessages(selSeries.feedURLBase + id));
            return View(sm);
        }

        public ActionResult MessageDetail(string serId, string msgDate)
        {
            TRCSeriesList = TRC_Mobile.Models.SeriesModel.SeriesList(urlSeriesList);
            var SelectedSeries = (from s in TRCSeriesList
                                  where s.FBCode == serId
                                  select s).LastOrDefault();

            //if no Series was found (ie, when "All Messages" was clicked)
            //- create new instance and set some defaults
            if (SelectedSeries == null)
            {
                SelectedSeries = new SeriesListModel();
                SelectedSeries.Title = "All Messages";
                SelectedSeries.Description = "List of All Message...";
                SelectedSeries.DescriptionHTML = "<p></p>";
            }

            ViewData.Add("SerId", serId);
            ViewData.Add("MsgDate", msgDate);
            ViewData.Add("SerTitle", SelectedSeries.Title);

            var mDet = new MessageModel();

            var serMsgs = SeriesModel.SeriesMessages(mDet.feedURLBase + serId);
            foreach (var m in serMsgs)
            {
                m.ThumbnailURL = SelectedSeries.ImageURL;
            }

            mDet = (from md in serMsgs
                          where (md.PublishedDate[2] + md.PublishedDate[1]).ToString() == msgDate
                          select md).First();

            return View(mDet);
        }

    }
}
