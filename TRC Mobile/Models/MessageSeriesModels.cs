using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace TRC_Mobile.Models
{
    public class SeriesModel
    {

        public static IList<MessageModel> SeriesMessages(string feedURL)
        {
            XDocument rssFeed = XDocument.Load(feedURL);

            var posts = from item in rssFeed.Descendants("item")
                        select new MessageModel
                        {
                            Title = item.Element("title").Value,
                            Description = item.Element("description").Value,
                            PublishedDate = (item.Element("pubDate").Value).Split(" ".ToCharArray()),
                            MediaURL = item.Element("guid").Value
                        };

            return posts.ToList();
        }

        public static IList<SeriesListModel> SeriesList(string seriesFeedURL)
        {
            XDocument rssFeed = XDocument.Load(seriesFeedURL);

            var posts = from item in rssFeed.Descendants("item")
                        select new SeriesListModel
                        {
                            PublishedDate = (item.Element("pubDate").Value).Split(" ".ToCharArray()),
                            Title = item.Element("title").Value.Split("|".ToCharArray())[0],
                            FBCode = item.Element("title").Value.Split("|".ToCharArray())[1],
                            Description = item.Element("description").Value,
                            DescriptionHTML = ((XElement)item.Element("description").NextNode).Value
                        };

            return posts.ToList();
        }

    }

    public class MessageModel
    {
        public string feedURLBase { get { return "http://feeds.feedburner.com/"; } }
        public string[] PublishedDate { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string MediaURL { get; set; }
        public string ThumbnailURL { get; set; }
    }

    public class SeriesListModel
    {
        public string feedURLBase { get { return "http://feeds.feedburner.com/"; } }
        public string[] PublishedDate { get; set; }
        public string Description { get; set; }
        public string DescriptionHTML { get; set; }
        public string Title { get; set; }
        public string FBCode { get; set; }
        public string ImageURL { get; set; }
        public string FeedURL
        {
            get { return "http://feeds.feedburner.com/" + FBCode; }
        }

    }


}