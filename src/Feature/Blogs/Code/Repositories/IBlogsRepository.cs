namespace Sitecore.Feature.Blogs.Repositories
{
    using System.Collections.Generic;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Blogs.Models;

    public interface IBlogsRepository
    {
        BlogItems Get(Item contextItem);
        BlogItems GetFeaturedBlog(Item contextItem);
        BlogItem GetCurrentItem(Item contextItem);
        BlogItems GetLatest(Item contextItem, int count);
        BlogItems GetFeaturedstory(Item contextItem);
    }
}
