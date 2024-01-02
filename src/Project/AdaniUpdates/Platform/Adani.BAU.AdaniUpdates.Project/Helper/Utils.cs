using Adani.BAU.AdaniUpdates.Project.Models;
using Sitecore;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Adani.BAU.AdaniUpdates.Project.Helper
{
    public class Utils
    {
        public static string GetSettings(string key)
        {
            var Item = Context.Database.GetItem(new ID("{43CB3C5D-939F-413F-800D-451B04763AA5}"));
            return Item?.Children
                .FirstOrDefault(x => x.Fields["Key"].Value.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                ?.Fields["Phrase"]?.Value
                ?? string.Empty;
        }

        public static SearchResponseModel<T> GetSearchResult<T>(string requestPath)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"https://{HttpContext.Current.Request.Url.Host}/api/adani-updates/search{requestPath}");
                if (response.Result.IsSuccessStatusCode)
                {
                    return response.Result.Content.ReadAsAsync<SearchResponseModel<T>>().Result;
                }
            }
            return new SearchResponseModel<T>();
        }

        public static IEnumerable<IGrouping<string, NewsHighlightsItemModel>> GetGroupedNewsHighlights(IEnumerable<NewsHighlightsItemModel> items, out NewsHighlightsItemModel firstItem)
        {
            firstItem = items.FirstOrDefault();
            var grpedItems = items.Skip(1).GroupBy(x => x.Date.ToLocalTime().ToString("MMM yyyy"));
            return grpedItems;
        }
    }
}