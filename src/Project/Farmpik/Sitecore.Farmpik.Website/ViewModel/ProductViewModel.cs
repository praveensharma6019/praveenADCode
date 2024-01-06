using Farmpik.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Farmpik.Website.ViewModel
{
    public class ProductViewModel
    {
        public List<ProductDetailsDto> Products { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public int PageCount { get { return TotalCount % PageSize == 0 ? (int)(TotalCount / PageSize) : (int)(TotalCount / PageSize) + 1; } }
    }
}