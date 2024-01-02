using Adani.SuperApp.Airport.Feature.Navigation.Platform.LayoutService;
using Adani.SuperApp.Airport.Feature.Navigation.Platform.Models;
using Adani.SuperApp.Airport.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.StringExtensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using static Adani.SuperApp.Airport.Foundation.Testing.HelperMethods;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Test
{
    public class DFHomepageContentResolverTests
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
            _helper.GetLinkURL(CreateItem(_db), Constant.Link).ReturnsForAnyArgs("linkUrl");
            _helper.GetImageURL(CreateItem(_db), Constant.Image).ReturnsForAnyArgs("imageUrl");
            _helper.GetImageURL(CreateItem(_db), Constant.MainImage).ReturnsForAnyArgs("mainImageUrl");
            _helper.GetImageURL(CreateItem(_db), Constant.Thumbnailimage).ReturnsForAnyArgs("thumbnailImageUrl");
            _helper.GetImageURL(CreateItem(_db), Constant.IconImages).ReturnsForAnyArgs("iconImageUrl");
            _helper.ToTitleCase(string.Empty).ReturnsForAnyArgs(x => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.ArgAt<string>(0).ToLower()));
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", "sc_restricted=false"), new HttpResponse(null));
        }

        [Test]
        public void DataSourceNull_ReturnsEmptyData()
        {
            var resultContents = new Object();
            _rendering.Parameters = new RenderingParameters("");
            using (RenderingContext.EnterContext(_rendering))
            {
                var logRepository = Substitute.For<ILogRepository>();
                DFHomepageContentResolver resolver = new DFHomepageContentResolver(Substitute.For<DutyFreeHeader>(logRepository, Substitute.For<WidgetService>(_helper, logRepository), _helper), logRepository);
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            var expectedResult = new DFHeaderlwidgets() { widget = new Foundation.Theming.Platform.Models.WidgetItem() { widgetItems = new List<Object>() } };
            Assert.AreEqual(JsonConvert.SerializeObject(resultContents), JsonConvert.SerializeObject(expectedResult));
        }

        [Test]
        public void NoDataSourceChildren_ReturnsEmptyData()
        {
            var resultContents = new Object();
            _rendering.Parameters = new RenderingParameters("");
            _rendering.Item = _datasourceItem;
            using (RenderingContext.EnterContext(_rendering))
            {
                var logRepository = Substitute.For<ILogRepository>();
                DFHomepageContentResolver resolver = new DFHomepageContentResolver(Substitute.For<DutyFreeHeader>(logRepository, Substitute.For<WidgetService>(_helper, logRepository), _helper), logRepository);
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            var expectedResult = new DFHeaderlwidgets() { widget = new Foundation.Theming.Platform.Models.WidgetItem() { widgetItems = new List<Object>() } };
            Assert.AreEqual(JsonConvert.SerializeObject(resultContents), JsonConvert.SerializeObject(expectedResult));
        }

        [Test]
        public void NotAgeRestricted_ReturnsFullData()
        {
            var resultContents = new Object();

            var section1 = CreateItem(_db, new ID(Constant.MaterialGroupTemplate));
            SetItemField(section1, Constant.Title, "section1");
            SetItemField(section1, Constant.MaterialGroupCode, "section1");
            SetItemField(section1, Constant.CDNPath, "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500");
            SetItemField(section1, "Age Restricted", "0");

            var brand1 = CreateItem(_db);
            SetItemField(brand1, Constant.Brand_Name, "brand1");
            SetItemField(brand1, Constant.Brand_Code, "brand1");
            SetItemField(brand1, Constant.Brand_Material_Group, "liquor");
            SetItemField(brand1, Constant.Brand_Description, "brand1");
            SetItemField(brand1, Constant.Brand_CDN_Image, "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500");

            var brand2 = CreateItem(_db);
            SetItemField(brand2, Constant.Brand_Name, "brand2");
            SetItemField(brand2, Constant.Brand_Code, "brand2");
            SetItemField(brand2, Constant.Brand_Material_Group, "MaterialGroup1");
            SetItemField(brand2, Constant.Brand_Description, "brand2");
            SetItemField(brand2, Constant.Brand_CDN_Image, "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500");

            var category1 = CreateItem(_db);
            SetItemField(category1, Constant.Code, "category1");
            SetItemField(category1, Constant.Name, "category1");
            SetItemField(category1, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(category1, Constant.Brands, "{0}|{1}".FormatWith(brand1.ID.ToString(), brand2.ID.ToString()));

            var subCategory1 = CreateItem(_db, new ID(Constant.SubCategoryTemplate));
            SetItemField(subCategory1, Constant.Code, "subCategory1");
            SetItemField(subCategory1, Constant.Name, "subCategory1");
            SetItemField(subCategory1, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(subCategory1, Constant.Brands, "{0}|{1}".FormatWith(brand1.ID.ToString(), brand2.ID.ToString()));

            category1.Children.Returns(new ChildList(category1, new ItemList() { subCategory1 }));

            var category2 = CreateItem(_db);
            SetItemField(category2, Constant.Code, "category2");
            SetItemField(category2, Constant.Name, "category2");
            SetItemField(category2, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(category2, Constant.Brands, "{0}".FormatWith(brand2.ID.ToString()));

            section1.Children.Returns(new ChildList(section1, new ItemList() { category1, category2 }));
            section1.HasChildren.Returns(true);

            var subCategory2 = CreateItem(_db, new ID(Constant.SubCategoryTemplate));
            SetItemField(subCategory2, Constant.Code, "subCategory2");
            SetItemField(subCategory2, Constant.Name, "subCategory2");
            SetItemField(subCategory2, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(subCategory2, Constant.Brands, "{0}|{1}".FormatWith(brand1.ID.ToString(), brand2.ID.ToString()));

            _datasourceItem.Children.Returns(new ChildList(_datasourceItem, new ItemList() { section1, subCategory2 }));

            var widgetRoot = CreateItem(_db);
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.widgetId, "1");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.widgetType, "gridScrollTile");
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

            widgetRoot.GetChildren().Returns(new ChildList(category2, new ItemList() { itemMargin, subItemMargin, actionTitle }));

            _rendering.Parameters = new RenderingParameters("restricted=true&widgetName=dutyfreenavigation&widget={0}".FormatWith(widgetRoot.ID.ToString()));
            _rendering.DataSource.Returns(_datasourceItem.ID.ToString());
            _rendering.RenderingItem.Returns(_datasourceItem);
            _rendering.RenderingItem.Database.GetItem(_datasourceItem.ID.ToString()).Returns(_datasourceItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                var logRepository = Substitute.For<ILogRepository>();
                DFHomepageContentResolver resolver = new DFHomepageContentResolver(Substitute.For<DutyFreeHeader>(logRepository, Substitute.For<WidgetService>(_helper, logRepository), _helper), logRepository);
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            var expectedResult = new DFHeaderlwidgets()
            {
                widget = new Foundation.Theming.Platform.Models.WidgetItem()
                {
                    widgetId = 1,
                    widgetType = "gridScrollTile",
                    title = "title",
                    subTitle = "subTitle",
                    subItemRadius = 4.0,
                    subItemWidth = 0.7,
                    gridColumn = 2,
                    aspectRatio = 1.4,
                    borderRadius = 1.0,
                    itemMargin = new Foundation.Theming.Platform.Models.ItemCSS() { left = 0.0, right = 0.0, bottom = 2.0, top = 0.0 },
                    subItemMargin = new Foundation.Theming.Platform.Models.ItemCSS() { left = 2.0, right = 16.0, bottom = 0.0, top = 0.0 },
                    actionTitle = new Foundation.Theming.Platform.Models.ActionTitle() { actionId = 1, deeplink = "", name = "action name 1" },
                    widgetItems = new List<Object>()
                    {
                        new MaterialGroup()
                        {
                            title = "section1", code = "section1", linkUrl = "linkUrl", imageSrc = "iconImageUrl", restricted = false, materialGroup = "section1",
                            cdnPath = "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500",
                            children = new List<Category>()
                            {
                                new Category()
                                {
                                    title = "category1", code = "category1", linkUrl = "linkUrl", category = "category1", brand = null, restricted = false,
                                    imageSrc ="Image",
                                    cdnPath = "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80",
                                    children = new List<Brand>()
                                    {
                                        new Brand()
                                        {
                                            title = "Brand1", code = "brand1", imageSrc = "iconImageUrl", description = "brand1", brand = "brand1", restricted = false,
                                            cdnPath = "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500"
                                        },
                                        new Brand()
                                        {
                                            title = "Brand2", code = "brand2", imageSrc = "iconImageUrl", description = "brand2", brand = "brand2", restricted = false,
                                            cdnPath = "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500"
                                        }
                                    }
                                },
                                new Category()
                                {
                                    title = "category2", code = "category2", linkUrl = "linkUrl", category = "category2", brand = null, restricted = false,
                                    imageSrc = "Image",
                                    cdnPath = "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80",
                                    children = new List<Brand>()
                                    {
                                        new Brand()
                                        {
                                            title = "Brand2", code = "brand2", imageSrc = "iconImageUrl", description = "brand2", brand = "brand2", restricted = false,
                                            cdnPath = "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500"
                                        }
                                    }
                                }
                            }
                        },
                        new MaterialGroup()
                        {
                            title = "subCategory2", code = "subCategory2", linkUrl = null, imageSrc = "iconImageUrl", cdnPath = null, restricted = false, materialGroup = null,
                            children = new List<Category>()
                            {
                                new Category(){ title = "brand1", code = "brand1", linkUrl = null, imageSrc = null, cdnPath = null, category = null, brand = "brand1", restricted = true, children = null },
                                new Category(){ title = "brand2", code = "brand2", linkUrl = null, imageSrc = null, cdnPath = null, category = null, brand = "brand2", restricted = false, children = null },
                            }
                        }
                    }
                }
            };

            Assert.AreEqual(JsonConvert.SerializeObject(resultContents), JsonConvert.SerializeObject(expectedResult));
        }

        [Test]
        public void AgeRestricted_ReturnsPartialData()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", "sc_restricted=true"), new HttpResponse(null));

            var resultContents = new Object();

            var section1 = CreateItem(_db, new ID(Constant.MaterialGroupTemplate));
            SetItemField(section1, Constant.Title, "section1");
            SetItemField(section1, Constant.MaterialGroupCode, "section1");
            SetItemField(section1, Constant.CDNPath, "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500");
            SetItemField(section1, "Age Restricted", "1");

            var brand1 = CreateItem(_db);
            SetItemField(brand1, Constant.Brand_Name, "brand1");
            SetItemField(brand1, Constant.Brand_Code, "brand1");
            SetItemField(brand1, Constant.Brand_Material_Group, "liquor");
            SetItemField(brand1, Constant.Brand_Description, "brand1");
            SetItemField(brand1, Constant.Brand_CDN_Image, "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500");

            var brand2 = CreateItem(_db);
            SetItemField(brand2, Constant.Brand_Name, "brand2");
            SetItemField(brand2, Constant.Brand_Code, "brand2");
            SetItemField(brand2, Constant.Brand_Material_Group, "MaterialGroup1");
            SetItemField(brand2, Constant.Brand_Description, "brand2");
            SetItemField(brand2, Constant.Brand_CDN_Image, "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500");

            var category1 = CreateItem(_db);
            SetItemField(category1, Constant.Code, "category1");
            SetItemField(category1, Constant.Name, "category1");
            SetItemField(category1, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(category1, Constant.Brands, "{0}|{1}".FormatWith(brand1.ID.ToString(), brand2.ID.ToString()));

            var subCategory1 = CreateItem(_db, new ID(Constant.SubCategoryTemplate));
            SetItemField(subCategory1, Constant.Code, "subCategory1");
            SetItemField(subCategory1, Constant.Name, "subCategory1");
            SetItemField(subCategory1, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(subCategory1, Constant.Brands, "{0}|{1}".FormatWith(brand1.ID.ToString(), brand2.ID.ToString()));

            category1.Children.Returns(new ChildList(category1, new ItemList() { subCategory1 }));

            var category2 = CreateItem(_db);
            SetItemField(category2, Constant.Code, "category2");
            SetItemField(category2, Constant.Name, "category2");
            SetItemField(category2, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(category2, Constant.Brands, "{0}".FormatWith(brand2.ID.ToString()));

            section1.Children.Returns(new ChildList(section1, new ItemList() { category1, category2 }));
            section1.HasChildren.Returns(true);

            var subCategory2 = CreateItem(_db, new ID(Constant.SubCategoryTemplate));
            SetItemField(subCategory2, Constant.Code, "subCategory2");
            SetItemField(subCategory2, Constant.Name, "subCategory2");
            SetItemField(subCategory2, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(subCategory2, Constant.Brands, "{0}|{1}".FormatWith(brand1.ID.ToString(), brand2.ID.ToString()));
            SetItemField(subCategory2, "Age Restricted", "0");

            _datasourceItem.Children.Returns(new ChildList(_datasourceItem, new ItemList() { section1, subCategory2 }));

            var widgetRoot = CreateItem(_db);
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.widgetId, "1");
            SetItemField(widgetRoot, Foundation.Theming.Platform.Constant.widgetType, "gridScrollTile");
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

            _rendering.Parameters = new RenderingParameters("restricted=true&widgetName=dutyfreenavigation&widget={0}".FormatWith(widgetRoot.ID.ToString()));
            _rendering.DataSource.Returns(_datasourceItem.ID.ToString());
            _rendering.RenderingItem.Returns(_datasourceItem);
            _rendering.RenderingItem.Database.GetItem(_datasourceItem.ID.ToString()).Returns(_datasourceItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                var logRepository = Substitute.For<ILogRepository>();
                DFHomepageContentResolver resolver = new DFHomepageContentResolver(Substitute.For<DutyFreeHeader>(logRepository, Substitute.For<WidgetService>(_helper, logRepository), _helper), logRepository);
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            var expectedResult = new DFHeaderlwidgets()
            {
                widget = new Foundation.Theming.Platform.Models.WidgetItem()
                {
                    widgetId = 1,
                    widgetType = "gridScrollTile",
                    title = "title",
                    subTitle = "subTitle",
                    subItemRadius = 4.0,
                    subItemWidth = 0.7,
                    gridColumn = 2,
                    aspectRatio = 1.4,
                    borderRadius = 1.0,
                    itemMargin = new Foundation.Theming.Platform.Models.ItemCSS() { left = 0.0, right = 0.0, bottom = 2.0, top = 0.0 },
                    subItemMargin = new Foundation.Theming.Platform.Models.ItemCSS() { left = 2.0, right = 16.0, bottom = 0.0, top = 0.0 },
                    actionTitle = new Foundation.Theming.Platform.Models.ActionTitle() { actionId = 1, deeplink = "", name = "action name 1" },
                    widgetItems = new List<Object>()
                    {
                        new MaterialGroup()
                        {
                            title = "subCategory2", code = "subCategory2", linkUrl = null, imageSrc = "iconImageUrl", cdnPath = null, restricted = false, materialGroup = null,
                            children = new List<Category>()
                            {
                                new Category(){ title = "brand1", code = "brand1", linkUrl = null, imageSrc = null, cdnPath = null, category = null, brand = "brand1", restricted = true, children = null },
                                new Category(){ title = "brand2", code = "brand2", linkUrl = null, imageSrc = null, cdnPath = null, category = null, brand = "brand2", restricted = false, children = null },
                            }
                        }
                    }
                }
            };

            Assert.AreEqual(JsonConvert.SerializeObject(resultContents), JsonConvert.SerializeObject(expectedResult));
        }

        [Test]
        public void AgeRestrictedNoWidgetConfigurationSelected_ReturnsPartialData()
        {
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://tempuri.org", "sc_restricted=true"), new HttpResponse(null));

            var resultContents = new Object();

            var section1 = CreateItem(_db, new ID(Constant.MaterialGroupTemplate));
            SetItemField(section1, Constant.Title, "section1");
            SetItemField(section1, Constant.MaterialGroupCode, "section1");
            SetItemField(section1, Constant.CDNPath, "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500");
            SetItemField(section1, "Age Restricted", "1");

            var brand1 = CreateItem(_db);
            SetItemField(brand1, Constant.Brand_Name, "brand1");
            SetItemField(brand1, Constant.Brand_Code, "brand1");
            SetItemField(brand1, Constant.Brand_Material_Group, "liquor");
            SetItemField(brand1, Constant.Brand_Description, "brand1");
            SetItemField(brand1, Constant.Brand_CDN_Image, "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500");

            var brand2 = CreateItem(_db);
            SetItemField(brand2, Constant.Brand_Name, "brand2");
            SetItemField(brand2, Constant.Brand_Code, "brand2");
            SetItemField(brand2, Constant.Brand_Material_Group, "MaterialGroup1");
            SetItemField(brand2, Constant.Brand_Description, "brand2");
            SetItemField(brand2, Constant.Brand_CDN_Image, "https://images.pexels.com/photos/135620/pexels-photo-135620.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500");

            var category1 = CreateItem(_db);
            SetItemField(category1, Constant.Code, "category1");
            SetItemField(category1, Constant.Name, "category1");
            SetItemField(category1, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(category1, Constant.Brands, "{0}|{1}".FormatWith(brand1.ID.ToString(), brand2.ID.ToString()));

            var subCategory1 = CreateItem(_db, new ID(Constant.SubCategoryTemplate));
            SetItemField(subCategory1, Constant.Code, "subCategory1");
            SetItemField(subCategory1, Constant.Name, "subCategory1");
            SetItemField(subCategory1, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(subCategory1, Constant.Brands, "{0}|{1}".FormatWith(brand1.ID.ToString(), brand2.ID.ToString()));

            category1.Children.Returns(new ChildList(category1, new ItemList() { subCategory1 }));

            var category2 = CreateItem(_db);
            SetItemField(category2, Constant.Code, "category2");
            SetItemField(category2, Constant.Name, "category2");
            SetItemField(category2, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(category2, Constant.Brands, "{0}".FormatWith(brand2.ID.ToString()));

            section1.Children.Returns(new ChildList(section1, new ItemList() { category1, category2 }));
            section1.HasChildren.Returns(true);

            var subCategory2 = CreateItem(_db, new ID(Constant.SubCategoryTemplate));
            SetItemField(subCategory2, Constant.Code, "subCategory2");
            SetItemField(subCategory2, Constant.Name, "subCategory2");
            SetItemField(subCategory2, Constant.CDNPath, "https://images.unsplash.com/photo-1638913971251-832d29947de6?ixlib=rb-1.2.1&ixid=MnwxMjA3fDF8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=3732&q=80");
            SetItemField(subCategory2, Constant.Brands, "{0}|{1}".FormatWith(brand1.ID.ToString(), brand2.ID.ToString()));
            SetItemField(subCategory2, "Age Restricted", "0");

            _datasourceItem.Children.Returns(new ChildList(_datasourceItem, new ItemList() { section1, subCategory2 }));

            _rendering.Parameters = new RenderingParameters("restricted=true&widgetName=dutyfreenavigation");
            _rendering.DataSource.Returns(_datasourceItem.ID.ToString());
            _rendering.RenderingItem.Returns(_datasourceItem);
            _rendering.RenderingItem.Database.GetItem(_datasourceItem.ID.ToString()).Returns(_datasourceItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                var logRepository = Substitute.For<ILogRepository>();
                DFHomepageContentResolver resolver = new DFHomepageContentResolver(Substitute.For<DutyFreeHeader>(logRepository, Substitute.For<WidgetService>(_helper, logRepository), _helper), logRepository);
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            var expectedResult = new DFHeaderlwidgets()
            {
                widget = new Foundation.Theming.Platform.Models.WidgetItem()
                {
                    widgetItems = new List<Object>()
                    {
                        new MaterialGroup()
                        {
                            title = "subCategory2", code = "subCategory2", linkUrl = null, imageSrc = "iconImageUrl", cdnPath = null, restricted = false, materialGroup = null,
                            children = new List<Category>()
                            {
                                new Category(){ title = "brand1", code = "brand1", linkUrl = null, imageSrc = null, cdnPath = null, category = null, brand = "brand1", restricted = true, children = null },
                                new Category(){ title = "brand2", code = "brand2", linkUrl = null, imageSrc = null, cdnPath = null, category = null, brand = "brand2", restricted = false, children = null },
                            }
                        }
                    }
                }
            };

            Assert.AreEqual(JsonConvert.SerializeObject(resultContents), JsonConvert.SerializeObject(expectedResult));
        }
    }
}
