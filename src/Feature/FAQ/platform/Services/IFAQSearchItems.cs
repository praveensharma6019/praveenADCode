using Adani.SuperApp.Airport.Feature.FAQ.Models;
using Sitecore.ContentSearch.SearchTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Feature.FAQ.Services
{
    public interface IFAQSearchItems
    {
        FAQResponseData GetSolrFAQData(ref Filters filter);
    }
}
