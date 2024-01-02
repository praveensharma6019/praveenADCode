using Adani.BAU.AdaniUpdates.Project.Models;
using Project.AdaniFacts.Website.Models;
using Sitecore;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using Sitecore.Configuration;

namespace Adani.BAU.AdaniUpdates.Project.Controllers
{
    public class AdaniUpdatesSearchApiController : Controller
    {
        private readonly string _searchIndex;

        public AdaniUpdatesSearchApiController()
        {
            _searchIndex = Settings.GetSetting("AU_SearchIndex");
        }

        [ActionName("news-highlights")]
        public ActionResult GetNewsHighlights(string dt, string pg = "1", bool mb = false)
        {
            var result = SearchNewsHighlights(dt, pg, mb, out bool nextPage);
            var response = new SearchResponseModel<NewsHighlightsItemModel>()
            {
                NextPage = nextPage,
                Result = result
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [ActionName("media-news")]
        public ActionResult GetMediaNews(string q, string cat, string dt, string pg = "1", bool mb = false)
        {
            var result = Results("/sitecore/content/adani updates/global/medianews/", q, cat, dt, pg, mb, out bool nextPage);
            var response = new SearchResponseModel<CardItemModel>()
            {
                NextPage = nextPage,
                Result = result
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [ActionName("media-release")]
        public ActionResult GetMediaReleases(string q, string cat, string dt, string pg = "1", bool mb = false)
        {
            var result = Results("/sitecore/content/adani updates/global/mediareleases/", q, cat, dt, pg, mb, out bool nextPage);
            var response = new SearchResponseModel<CardItemModel>()
            {
                NextPage = nextPage,
                Result = result
            };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Summary(string query, int type = 0)
        {

            if (!string.IsNullOrEmpty(query))
            {
                using (var context = ContentSearchManager.GetIndex(_searchIndex).CreateSearchContext())
                {

                    var predicate = PredicateBuilder.True<FactSearchResultItem>();
                    // must have this (.and)
                    if (type == 1)
                    {
                        predicate = predicate.And(p => p.Path.Contains("/sitecore/content/Adani Updates/Global/MediaNews/"));
                    }
                    else if (type == 2)
                    {
                        predicate = predicate.And(p => p.Path.Contains("/sitecore/content/Adani Updates/Global/MediaReleases/"));
                    }
                    else
                    {
                        predicate = predicate.And(p => p.Path.Contains("/sitecore/content/Adani Updates/Global/MediaNews/") || p.Path.Contains("/sitecore/content/Adani Updates/Global/MediaReleases/"));
                    }

                    predicate = predicate.And(p => p.Content.Contains(query));


                    var result = context.GetQueryable<FactSearchResultItem>().Where(predicate).ToList().Select
                     (X => new
                     {
                         title = X.Title,
                         //linkurl = X.Fields["ctalink"],
                         linkurl = GetLinkValue(X, "ctalink").Item1,
                         summary = X.Path.Contains("/medianews/") ? "in Media News" : "in Media Releases"
                     });
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new List<object>(), JsonRequestBehavior.AllowGet);
        }

        IEnumerable<NewsHighlightsItemModel> SearchNewsHighlights(string dateRange, string page, bool mobile, out bool hasNextPage)
        {
            var selectedCategories = new string[0];
            //string index = string.Format("sitecore_{0}_index", Sitecore.Context.Database.Name);
            string path = "/sitecore/content/Adani Updates/Global/NewsHighlights/";

            using (var context = ContentSearchManager.GetIndex(_searchIndex).CreateSearchContext())
            {
                var predicate = PredicateBuilder.True<FactSearchResultItem>();
                predicate = predicate.And(p => p.Path.Contains(path) && p.TemplateName == "NewsHighlights");

                var result = context.GetQueryable<FactSearchResultItem>().Where(predicate).ToList()
                    .Where(DateRangeFilterPredicate(dateRange))
                    .OrderByDescending(x => x.Datetime);

                if (string.IsNullOrEmpty(page) || !int.TryParse(page, out int currentPage))
                {
                    currentPage = 1;
                }

                int pageSize = 20;// mobile ? 2 : 2; 
                int skip = (currentPage - 1) * pageSize;
                var pageResult = result.Skip(skip).Take(pageSize).Select(item =>
                {
                    var link = GetLinkValue(item, "pdf_path", true);
                    return new NewsHighlightsItemModel
                    {
                        Title = GetValue(item, "title"),
                        Summary = GetValue(item, "description"),
                        Date = GetDate(item, "date"),
                        DateString = GetValue(item, "date"),
                        LinkUrl = link.Item1,
                        LinkTarget = link.Item2,
                    };
                });
                hasNextPage = result.Skip(skip + pageSize).Any();
                return pageResult;
            }
        }

        IEnumerable<CardItemModel> Results(string path, string query, string categories, string dateRange, string page, bool mobile, out bool hasNextPage)
        {
            var selectedCategories = new string[0];

            using (var context = ContentSearchManager.GetIndex(_searchIndex).CreateSearchContext())
            {
                var predicate = PredicateBuilder.True<FactSearchResultItem>();
                predicate = predicate.And(p => p.Path.Contains(path) && p.TemplateName == "MediaRelease");

                if (!string.IsNullOrEmpty(query))
                {
                    predicate = predicate.And(p => p.Content.Contains(query));
                }

                var result = context.GetQueryable<FactSearchResultItem>().Where(predicate).ToList()
                                    .Where(DateRangeFilterPredicate(dateRange))
                                    .Where(CategoryFilterPredicate(categories))
                                    .OrderByDescending(x => x.Datetime);

                if (string.IsNullOrEmpty(page) || !int.TryParse(page, out int currentPage))
                {
                    currentPage = 1;
                }

                int pageSize = mobile ? 8 : 12;
                int skip = (currentPage - 1) * pageSize;
                var pageResult = result.Skip(skip).Take(pageSize).Select(item =>

                {
                    return new CardItemModel
                    {
                        Title = GetValue(item, "title"),
                        Summary = GetValue(item, "description"),
                        IsVideo = GetValue(item, "isvideo") == "True",
                        Date = GetDate(item, "date"),
                        DateString = GetValue(item, "date"),
                        ImageAlt = GetValue(item, "image"),
                        ImageSrc = GetValue(item, "imagesrc"),
                        LinkUrl = GetLinkValue(item, "ctalink").Item1,
                        LinkTarget = GetLinkValue(item, "ctalink").Item2,
                    };
                });
                hasNextPage = result.Skip(skip + pageSize).Any();
                return pageResult;
            }
        }

        Func<FactSearchResultItem, bool> CategoryFilterPredicate(string categories)
        {
            return x =>
            {
                if (string.IsNullOrEmpty(categories)) return true;

                var categoryList = categories.Split(',');
                return x.Category?.Any(c => categoryList.Contains(c)) ?? false;
            };
        }

        Func<FactSearchResultItem, bool> DateRangeFilterPredicate(string dateRange)
        {
            return x =>
            {
                if (string.IsNullOrEmpty(dateRange)) return true;

                var dateparam = dateRange.Split('-');
                if (dateparam.Length == 2)
                {
                    var isFromDateParsed = DateTime.TryParseExact(dateparam[0], "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime from);//.Date;
                    var isToDateParsed = DateTime.TryParseExact(dateparam[1], "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime to); //.Date;

                    if (isFromDateParsed && isToDateParsed)
                    {
                        return x.Datetime >= from && x.Datetime <= to;
                    }
                }
                return false;
            };
        }

        public string GetValue(FactSearchResultItem item, string key)
        {
            if (!item.Fields.ContainsKey(key)) return string.Empty;
            return item.Fields[key]?.ToString() ?? string.Empty;
        }

        private DateTime GetDate(FactSearchResultItem item, string key)
        {
            var dateInString = GetValue(item, key);
            if (String.IsNullOrEmpty(dateInString))
            {
                return DateTime.MinValue;
            }

            DateTime.TryParse(dateInString, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal, out var date);
            return date;
        }

        (string, string) GetLinkValue(FactSearchResultItem item, string key, bool includeServerUrl = false)
        {
            var rawValue = GetValue(item, key);
            if (string.IsNullOrEmpty(rawValue)) return (string.Empty, string.Empty);

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(rawValue);

            XmlElement root = doc.DocumentElement;
            var linktype = root.GetAttribute("linktype");

            switch (linktype)
            {
                case "media":
                    var mTargetId = root.GetAttribute("id");
                    var mTargetItem = Context.Database.GetItem(new ID(mTargetId));
                    return mTargetItem != null ? (MediaManager.GetMediaUrl(mTargetItem, new MediaUrlBuilderOptions
                    {
                        //AlwaysIncludeServerUrl = includeServerUrl,
                        IncludeExtension = false,
                    }), root.GetAttribute("target")) : (string.Empty, string.Empty);
                case "internal":
                    var lTargetId = root.GetAttribute("id");
                    var lTargetItem = Context.Database.GetItem(new ID(lTargetId));
                    return (LinkManager.GetItemUrl(lTargetItem), root.GetAttribute("target"));
                case "external":
                    return (root.GetAttribute("url"), root.GetAttribute("target"));
                default:
                    return (string.Empty, string.Empty);
            }
        }
    }
}