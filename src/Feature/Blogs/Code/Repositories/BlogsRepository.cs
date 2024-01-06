namespace Sitecore.Feature.Blogs.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Blogs.Models;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Indexing.Models;
    using Sitecore.Foundation.Indexing.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;


    [Service(typeof(IBlogsRepository))]
    public class BlogsRepository : IBlogsRepository
    {
        private readonly ISearchServiceRepository searchServiceRepository;

        public BlogsRepository(ISearchServiceRepository searchServiceRepository)
        {
            this.searchServiceRepository = searchServiceRepository;
        }

        public BlogItems Get(Item contextItem)
        {
            if (contextItem == null)
            {
                throw new ArgumentNullException(nameof(contextItem));
            }

            if (!contextItem.HasChildren)
            {
                return null;
            }

            var childItems = contextItem.Children.Where(item => this.IncludeArticle(item)).Select(i => this.CreateBlogItem(i)).OrderByDescending(i => i.Date);

            return new BlogItems
            {
                BlogItem = childItems.ToList()
            };
        }
        
        public BlogItems GetFeaturedBlog(Item contextItem)
        {
            if (contextItem == null)
            {
                throw new ArgumentNullException(nameof(contextItem));
            }

            if (!contextItem.HasChildren)
            {
                return null;
            }

            var childItems = contextItem.Children.Where(item => this.IncludeFeatured(item)).Select(i => this.CreateBlogItem(i));

            return new BlogItems
            {
                BlogItem = childItems.ToList()
            };
        }

        public BlogItems GetFeaturedstory(Item contextItem)
        {
            if (contextItem == null)
            {
                throw new ArgumentNullException(nameof(contextItem));
            }

            if (!contextItem.HasChildren)
            {
                return null;
            }

            var childItems = contextItem.Children.Where(item => this.IncludeStory(item)).Select(i => this.CreateBlogItem(i));

            return new BlogItems
            {
                BlogItem = childItems.ToList()
            };
        }

        public BlogItem GetCurrentItem(Item contextItem)
        {
            if (contextItem == null)
            {
                throw new ArgumentNullException(nameof(contextItem));
            }

            return CreateBlogItem(contextItem);
        }

        private BlogItem CreateBlogItem(Item item)
        {

            return new BlogItem
            {
                Item = item,
                Url = item.Url(),
                Title = item.Fields[Templates.BlogsArticle.Fields.Title].ToString(),
                Body = item.Fields[Templates.BlogsArticle.Fields.Body].ToString(),
                Summary = item.Fields[Templates.BlogsArticle.Fields.Summary].ToString(),
                Type = item.Fields[Templates.BlogsArticle.Fields.BlogType].ToString(),
                Date = item.Fields[Templates.BlogsArticle.Fields.Date].ToString(),
                Author = item.Fields[Templates.BlogsArticle.Fields.Author].ToString(),
                Category = item.Fields[Templates.BlogsArticle.Fields.Category].ToString(),
                Image = item.Fields[Templates.BlogsArticle.Fields.Image].ToString()
            };
        }

        private bool IncludeFeatured(Item item, bool forceShowInMenu = false)
        {
            return item.Fields["Blog Type"].Value.ToString().ToLower().Equals("featured");
            //return item.HasContextLanguage() && item.IsDerived(Templates.BlogsArticle.ID);
            //return item.HasContextLanguage() && item.IsDerived(Templates.BlogsArticle.ID) && (forceShowInMenu || MainUtil.GetBool(item[Templates.Navigable.Fields.ShowInNavigation], false));
        }

        private bool IncludeArticle(Item item, bool forceShowInMenu = false)
        {
            return item.HasContextLanguage(); //&& item.IsDerived(Templates.BlogsArticle.ID);           
        }

        public BlogItems GetLatest(Item contextItem, int count)
        {
            return this.Get(contextItem);//.Take(count);
        }
        
        private bool IncludeStory(Item item, bool forceShowInMenu = false)
        {
            return item.HasContextLanguage() && item.IsDerived(Templates.BlogsArticle.ID);
            //return item.HasContextLanguage() && item.IsDerived(Templates.BlogsArticle.ID) && (forceShowInMenu || MainUtil.GetBool(item[Templates.Navigable.Fields.ShowInNavigation], false));
        }
    }
}