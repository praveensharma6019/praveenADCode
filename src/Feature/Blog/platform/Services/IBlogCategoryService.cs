using Adani.SuperApp.Realty.Feature.Blog.Platform.Models;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Blog.Platform.Services
{
    public interface IBlogCategoryService
    {
        BlogCategoryModel GetBlogCategoryData(Rendering rendering, string categoryid="", int pageNo=1);
    }
}