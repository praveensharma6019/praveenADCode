using Adani.SuperApp.Airport.Feature.Covid.Models;
using Adani.SuperApp.Airport.Feature.Covid.Repositories;
using Adani.SuperApp.Airport.Feature.Covid.Resolvers;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.StringExtensions;
using System;
using System.Collections.Generic;
using static Adani.SuperApp.Airport.Foundation.Testing.HelperMethods;

namespace Adani.SuperApp.Airport.Feature.Covid.Test
{
    public class CovidContentResolverTests
    {
        Database _db;
        Rendering _rendering;
        Item _datasourceItem;
        IHelper _helper;

        [SetUp]
        public void Setup()
        {
            _db = Substitute.For<Database>();
            _rendering = Substitute.For<Rendering>();
            _datasourceItem = CreateItem(_db);
            _helper = Substitute.For<IHelper>();
            _helper.GetImageURL(_datasourceItem, Templates.Covid.Fields.Image.ToString()).ReturnsForAnyArgs("imageUrl");
            _helper.GetImageAlt(_datasourceItem, Templates.Covid.Fields.Image.ToString()).ReturnsForAnyArgs("imageAlt");
            _helper.GetImageURL(_datasourceItem, Templates.Covid.Fields.MobileImage.ToString()).ReturnsForAnyArgs("mobileImageUrl");
            _helper.GetImageAlt(_datasourceItem, Templates.Covid.Fields.MobileImage.ToString()).ReturnsForAnyArgs("mobileImageAlt");
            _helper.GetImageURL(_datasourceItem, Templates.Covid.Fields.WebImage.ToString()).ReturnsForAnyArgs("webimageUrl");
            _helper.GetImageAlt(_datasourceItem, Templates.Covid.Fields.WebImage.ToString()).ReturnsForAnyArgs("webImageAlt");
            _helper.GetImageURL(_datasourceItem, Templates.Covid.Fields.ThumbnailImage.ToString()).ReturnsForAnyArgs("thumbnailImageUrl");
            _helper.GetImageAlt(_datasourceItem, Templates.Covid.Fields.ThumbnailImage.ToString()).ReturnsForAnyArgs("thumbnailImageAlt");
            _helper.GetImageURL(_datasourceItem, Templates.CovidCrausal.Fields.Image.ToString()).ReturnsForAnyArgs("carouselImage");
            _helper.GetLinkText(_datasourceItem, Templates.Covid.Fields.CTA.ToString()).ReturnsForAnyArgs("linkText");
            _helper.GetLinkURL(_datasourceItem, Templates.Covid.Fields.CTA.ToString()).ReturnsForAnyArgs("linkUrl");
        }

        [Test]
        public void DataSourceNull_ReturnsEmptyData()
        {
            var resultContents = new Object();
            using (RenderingContext.EnterContext(_rendering))
            {
                var logRepository = Substitute.For<ILogRepository>();
                CovidContentResolver resolver = new CovidContentResolver(Substitute.For<CovidRepository>(logRepository, Substitute.For<WidgetService>(_helper, logRepository), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            Covidwidgets expectedResult = new Covidwidgets() { };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void NoWidgetItems_ReturnsWidgetDataOnly()
        {
            var widgetRoot = CreateItem(_db);
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.widgetId, "8");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.widgetType, "covidBanner");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.title, "title");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.subTitle, "subTitle");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.subItemRadius, "4");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.subItemWidth, "0.7");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.gridColumn, "2");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.aspectRatio, "1.4");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.BorderRadius, "1");

            var itemMargin = CreateItem(_db, Foundation.Theming.Platform.Templates.WidgetItemsCollection.ItemMarginTemplateID);
            SetItemField(itemMargin, Foundation.Theming.Platform.Constant.left, "0");
            SetItemField(itemMargin, Foundation.Theming.Platform.Constant.right, "0");
            SetItemField(itemMargin, Foundation.Theming.Platform.Constant.top, "0");
            SetItemField(itemMargin, Foundation.Theming.Platform.Constant.bottom, "2");

            var subItemMargin = CreateItem(_db, Foundation.Theming.Platform.Templates.WidgetItemsCollection.SubItemMarginTemplateID);
            SetItemField(subItemMargin, Foundation.Theming.Platform.Constant.left, "2");
            SetItemField(subItemMargin, Foundation.Theming.Platform.Constant.right, "16");
            SetItemField(subItemMargin, Foundation.Theming.Platform.Constant.top, "0");
            SetItemField(subItemMargin, Foundation.Theming.Platform.Constant.bottom, "0");

            var actionTitle = CreateItem(_db, Foundation.Theming.Platform.Templates.WidgetItemsCollection.ActionTitleTemplateID);
            SetItemField(actionTitle, Foundation.Theming.Platform.Constant.name, "action name 1");
            SetItemField(actionTitle, Foundation.Theming.Platform.Constant.actionId, "1");

            widgetRoot.GetChildren().Returns(new ChildList(widgetRoot, new ItemList() { itemMargin, subItemMargin, actionTitle }));

            _rendering.Parameters = new RenderingParameters("widget={0}".FormatWith(widgetRoot.ID.ToString()));
            //Sitecore.Context.Database.GetItem(widgetRoot.ID.ToString()).Returns(widgetRoot);
            var resultContents = new Object();
            using (RenderingContext.EnterContext(_rendering))
            using (new DatabaseSwitcher(_db))
            {
                var logRepository = Substitute.For<ILogRepository>();
                CovidContentResolver resolver = new CovidContentResolver(Substitute.For<CovidRepository>(logRepository, Substitute.For<WidgetService>(_helper, logRepository), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            Covidwidgets expectedResult = new Covidwidgets()
            {
                widget = new Foundation.Theming.Platform.Models.WidgetItem()
                {
                    widgetId = 8,
                    widgetType = "covidBanner",
                    title = "title",
                    subTitle = "subTitle",
                    subItemRadius = 4.0,
                    subItemWidth = 0.7,
                    gridColumn = 2,
                    aspectRatio = 1.4,
                    borderRadius = 1.0,
                    itemMargin = new Foundation.Theming.Platform.Models.ItemCSS() { left = 0.0, right = 0.0, bottom = 2.0, top = 0.0 },
                    subItemMargin = new Foundation.Theming.Platform.Models.ItemCSS() { left = 2.0, right = 16.0, bottom = 0.0, top = 0.0 },
                    actionTitle = new Foundation.Theming.Platform.Models.ActionTitle() { actionId = 1, deeplink = "", name = "action name 1" }
                }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void DatasourceConfiguration_ReturnsWidgetItemsData()
        {
            var resultContents = new Object();
            SetItemField(_datasourceItem, Templates.Covid.Fields.Summary, "summary");
            SetItemField(_datasourceItem, Templates.Covid.Fields.Details, "summary");

            var carouselItem1 = CreateItem(_db);
            SetItemField(carouselItem1, Templates.CovidCrausal.Fields.Summary, "carouselItem1");
            SetItemField(carouselItem1, Templates.CovidCrausal.Fields.Details, "carouselItem1");

            var carouselItem2 = CreateItem(_db);
            SetItemField(carouselItem2, Templates.CovidCrausal.Fields.Summary, "carouselItem2");
            SetItemField(carouselItem2, Templates.CovidCrausal.Fields.Details, "carouselItem2");

            SetItemField(_datasourceItem, Templates.Covid.Fields.CovidCarousal, "{0}|{1}".FormatWith(carouselItem1.ID.ToString(), carouselItem2.ID.ToString()));

            _rendering.Parameters = new RenderingParameters("");
            _rendering.Item.Returns(_datasourceItem);
            using (RenderingContext.EnterContext(_rendering))
            using (new DatabaseSwitcher(_db))
            {
                var logRepository = Substitute.For<ILogRepository>();
                CovidContentResolver resolver = new CovidContentResolver(Substitute.For<CovidRepository>(logRepository, Substitute.For<WidgetService>(_helper, logRepository), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            Covidwidgets expectedResult = new Covidwidgets()
            {
                widget = new Foundation.Theming.Platform.Models.WidgetItem()
                {
                    widgetItems = new List<object>()
                    {
                       new CovidModel()
                       {
                           title = "summary",
                           text = "summary",
                           src = "carouselImage",
                           alt = "thumbnailImageAlt",
                           btn = "linkText",
                           btnUrl = "linkUrl",
                           CarousalItems = new List<CovidCarousel>(){
                               new CovidCarousel(){
                                   Summary = "carouselItem1",
                                   Details = "carouselItem1",
                                   Image = "carouselImage"
                               },
                               new CovidCarousel(){
                                   Summary = "carouselItem2",
                                   Details = "carouselItem2",
                                   Image = "carouselImage"
                               }
                           },
                           mobileImage = "carouselImage",
                           mobileImageAlt = "carouselImage",
                           webImage = "carouselImage",
                           webImageAlt = "carouselImage",
                           thumbnailImage = "carouselImage",
                           thumbnailImageAlt = "carouselImage"
                       }
                    }
                }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }
    }
}