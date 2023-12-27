using Adani.SuperApp.Airport.Feature.Navigation.Platform.LayoutService;
using Adani.SuperApp.Airport.Feature.Navigation.Platform.Models;
using Adani.SuperApp.Airport.Feature.Navigation.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Adani.SuperApp.Airport.Feature.Navigation.Platform.Templates;
using static Adani.SuperApp.Airport.Foundation.Testing.HelperMethods;

namespace Adani.SuperApp.Airport.Feature.Navigation.Platform.Test
{
    [TestFixture]
    public class HeaderDataListResolverTests
    {
        static Database _db;
        static Rendering _rendering;
        static Item _headerItem;
        static IHelper _helper;

        [SetUp]
        public void Setup()
        {
            _db = Substitute.For<Database>();
            _rendering = Substitute.For<Rendering>();
            _headerItem = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
            _helper = Substitute.For<IHelper>();
            _helper.LinkUrlText(Substitute.For<Field>(ID.NewID, CreateItem(_db))).ReturnsForAnyArgs("linkText");
            _helper.LinkUrl(Substitute.For<Field>(ID.NewID, CreateItem(_db))).ReturnsForAnyArgs("linkUrl");
            _helper.LinkUrlStyleclass(Substitute.For<Field>(ID.NewID, CreateItem(_db))).ReturnsForAnyArgs("styleClass");
            _helper.GetImageAltbyField(Substitute.For<Field>(ID.NewID, CreateItem(_db))).ReturnsForAnyArgs("imageAlt");
            _helper.GetImageURL(Substitute.For<Field>(ID.NewID, CreateItem(_db))).ReturnsForAnyArgs("imageUrl");
            _helper.GetImageURL(CreateItem(_db), Constant.Image).ReturnsForAnyArgs("imageUrl");
            _helper.GetImageURLbyField(Substitute.For<Field>(ID.NewID, CreateItem(_db))).ReturnsForAnyArgs("imageUrl");
            _helper.GetCheckboxOption(Substitute.For<Field>(ID.NewID, CreateItem(_db))).ReturnsForAnyArgs(x => x.ArgAt<Field>(0).Value == "1" ? true : false);
        }

        [Test]
        public void DataSourceNull_ReturnsException()
        {
            NullReferenceException nullException = new NullReferenceException();
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                nullException = Assert.Throws<NullReferenceException>(() => resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>()));
            }
            Assert.AreEqual("HeaderDataListResolver => Rendering Datasource is Empty", nullException.Message);
        }

        [Test]
        public void NoDataSourceChildren_ReturnsEmptyData()
        {
            var resultContents = new Object();
            _rendering.Item.Returns(_headerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            HeaderData expectedResult = new HeaderData();
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void TopNavigationOnly_ReturnsTopNavigation()
        {
            var resultContents = new Object();
            var rendering = Substitute.For<Rendering>();
            var _headerItem = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
            var topNavigation = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.TopNavigationID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1, Constant.Link_Title, "section1");
            SetItemField(section1, Constant.Link_leftIcon, "i-section1");
            SetCheckBoxField(section1, "IsActive", true);
            SetItemField(section1, Constant.Link_rightIcon, string.Empty);

            var section1Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link1, Constant.Link_Title, "section1Link1");
            SetItemField(section1Link1, Constant.Link_leftIcon, "i-section1Link1");
            SetItemField(section1Link1, Constant.Link_rightIcon, "i-section1Link1");
            SetCheckBoxField(section1Link1, "IsActive", true);

            var section1Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link2, Constant.Link_Title, "section1Link2");
            SetItemField(section1Link2, Constant.Link_leftIcon, "i-section1Link2");
            SetItemField(section1Link2, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section1Link2, "IsActive", true);

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section1Link3, "IsActive", true);

            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2");
            SetCheckBoxField(section2, "IsActive", true);

            var section2Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link1, Constant.Link_Title, "section2Link1");
            SetItemField(section2Link1, Constant.Link_leftIcon, "i-section2Link1");
            SetItemField(section2Link1, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section2Link1, "IsActive", true);

            var section2Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link2, Constant.Link_Title, "section2Link2");
            SetItemField(section2Link2, Constant.Link_leftIcon, "i-section2Link2");
            SetItemField(section2Link2, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section2Link2, "IsActive", false);

            var section2Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link3, Constant.Link_Title, "section2Link3");
            SetItemField(section2Link3, Constant.Link_leftIcon, "i-section2Link3");
            SetItemField(section2Link2, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section2Link3, "IsActive", false);

            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3, Constant.Link_Title, "section3");
            SetItemField(section3, Constant.Link_leftIcon, "i-section3");
            SetItemField(section3, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section3, "IsActive", false);

            var section3Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link1, Constant.Link_Title, "section3Link1");
            SetItemField(section3Link1, Constant.Link_leftIcon, "i-section3Link1");
            SetItemField(section3Link1, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section3Link1, "IsActive", true);

            var section3Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link2, Constant.Link_Title, "section3Link2");
            SetItemField(section3Link2, Constant.Link_leftIcon, "i-section3Link2");
            SetItemField(section3Link2, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section3Link2, "IsActive", false);

            var section3Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link3, Constant.Link_Title, "section3Link3");
            SetItemField(section3Link3, Constant.Link_leftIcon, "i-section3Link3");
            SetItemField(section3Link3, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section3Link3, "IsActive", true);

            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4, "IsActive", true);

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4Link1, "IsActive", false);

            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5, Constant.Link_Title, "section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-section5");
            SetItemField(section5, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section5, "IsActive", true);
            #endregion

            topNavigation.Children.Returns(new ChildList(topNavigation, new ItemList() { section1, section2, section3, section4, section5 }));
            _headerItem.Children.Returns(new ChildList(_headerItem, new ItemList() { topNavigation }));

            _rendering.Item.Returns(_headerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            HeaderData expectedResult = new HeaderData()
            {
                HeaderDetails = new List<HeaderDetails>()
                    {
                        new HeaderDetails(){
                            TopNavigation = new List<TopNavigation>()
                            {
                                new TopNavigation()
                                {
                                    headerText = "section1", headerLeftIcon = "i-section1", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                    items = new List<ChildNavigation>()
                                    {
                                        new ChildNavigation() { itemLink = "linkUrl", itemText = "section1Link1", itemLeftIcon = "i-section1Link1", itemRightIcon = "i-section1Link1" },
                                        new ChildNavigation() { itemLink = "linkUrl", itemText = "section1Link2", itemLeftIcon = "i-section1Link2", itemRightIcon = string.Empty },
                                        new ChildNavigation() { itemLink = "linkUrl", itemText = "section1Link3", itemLeftIcon = "i-section1Link3", itemRightIcon = string.Empty }
                                    }
                                },
                                new TopNavigation()
                                {
                                    headerText = "section2", headerLeftIcon = "i-section2", headerLink = "linkUrl", headerRightIcon ="i-section2",
                                    items = new List<ChildNavigation>()
                                    {
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section2Link1", itemLeftIcon = "i-section2Link1", itemRightIcon=String.Empty }
                                    }
                                },
                                new TopNavigation()
                                {
                                    headerText = "section4", headerLeftIcon = "i-section4", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                },
                                new TopNavigation()
                                {
                                    headerText = "section5", headerLeftIcon = "i-section5", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                }
                            }
                        }
                    }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void HamburgerMenuOnly_ReturnsHamburgerMenu()
        {
            var resultContents = new Object();
            var hamburgerMenu = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.HamburgerMenuID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkTemplateID, AirportNavigation.AdaniAirportID);
            SetItemField(section1, Constant.Link_Title, "section1");
            SetItemField(section1, Constant.Link_leftIcon, "i-section1");
            SetItemField(section1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1, "IsActive", "1");

            var section1Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link1, Constant.Link_Title, "section1Link1");
            SetItemField(section1Link1, Constant.Link_leftIcon, "i-section1Link1");
            SetItemField(section1Link1, Constant.Link_rightIcon, "i-section1Link1");
            SetItemField(section1Link1, "IsActive", "1");

            var section1Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link2, Constant.Link_Title, "section1Link2");
            SetItemField(section1Link2, Constant.Link_leftIcon, "i-section1Link2");
            SetItemField(section1Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1Link2, "IsActive", "0");

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, "i-section1Link3");
            SetItemField(section1Link3, "IsActive", "1");

            var section1Link3SubLink1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3SubLink1, Constant.Link_Title, "section1Link3SubLink1");
            SetItemField(section1Link3SubLink1, Constant.Link_leftIcon, "i-section1Link3SubLink1");
            SetItemField(section1Link3SubLink1, Constant.Link_rightIcon, "i-section1Link3SubLink1");
            SetItemField(section1Link3SubLink1, "IsActive", "1");

            var section1Link3SubLink2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3SubLink2, Constant.Link_Title, "section1Link3SubLink2");
            SetItemField(section1Link3SubLink2, Constant.Link_leftIcon, "i-section1Link3SubLink2");
            SetItemField(section1Link3SubLink2, Constant.Link_rightIcon, "i-section1Link3SubLink2");
            SetItemField(section1Link3SubLink2, "IsActive", "1");
            section1Link3.Children.Returns(new ChildList(section1Link3, new ItemList() { section1Link3SubLink1, section1Link3SubLink2 }));

            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID, AirportNavigation.AdaniBusinessesID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2");
            SetItemField(section2, "IsActive", "1");

            var section2Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link1, Constant.Link_Title, "section2Link1");
            SetItemField(section2Link1, Constant.Link_leftIcon, "i-section2Link1");
            SetItemField(section2Link1, Constant.Link_rightIcon, "i-section2Link1");
            SetItemField(section2Link1, "IsActive", "1");

            var section2Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link2, Constant.Link_Title, "section2Link2");
            SetItemField(section2Link2, Constant.Link_leftIcon, "i-section2Link2");
            SetItemField(section2Link2, Constant.Link_rightIcon, "i-section2Link2");
            SetItemField(section2Link2, "IsActive", "0");

            var section2Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link3, Constant.Link_Title, "section2Link3");
            SetItemField(section2Link3, Constant.Link_leftIcon, "i-section2Link3");
            SetItemField(section2Link3, Constant.Link_rightIcon, "i-section2Link3");
            SetItemField(section2Link3, "IsActive", "0");

            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkTemplateID, AirportNavigation.OtherID);
            SetItemField(section3, Constant.Link_Title, "section3");
            SetItemField(section3, Constant.Link_leftIcon, "i-section3");
            SetItemField(section3, Constant.Link_rightIcon, "i-section3");
            SetItemField(section3, "IsActive", "1");

            var section3Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link1, Constant.Link_Title, "section3Link1");
            SetItemField(section3Link1, Constant.Link_leftIcon, "i-section3Link1");
            SetItemField(section3Link1, Constant.Link_rightIcon, "i-section3Link1");
            SetItemField(section3Link1, "IsActive", "1");

            var section3Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link2, Constant.Link_Title, "section3Link2");
            SetItemField(section3Link2, Constant.Link_leftIcon, "i-section3Link2");
            SetItemField(section3Link2, Constant.Link_rightIcon, "i-section3Link2");
            SetItemField(section3Link2, "IsActive", "0");

            var section3Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link3, Constant.Link_Title, "section3Link3");
            SetItemField(section3Link3, Constant.Link_leftIcon, "i-section3Link3");
            SetItemField(section3Link3, Constant.Link_rightIcon, "i-section3Link3");
            SetItemField(section3Link3, "IsActive", "1");

            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, "i-section4");
            SetItemField(section4, "IsActive", "1");

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, "i-section4Link1");
            SetItemField(section4Link1, "IsActive", "0");

            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5, Constant.Link_Title, "section5");
            SetItemField(section5, Constant.Link_rightIcon, "i-section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-section5");
            SetItemField(section5, "IsActive", "1");
            #endregion

            hamburgerMenu.Children.Returns(new ChildList(hamburgerMenu, new ItemList() { section1, section2, section3, section4, section5 }));
            _headerItem.Children.Returns(new ChildList(_headerItem, new ItemList { hamburgerMenu }));

            _rendering.Item.Returns(_headerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            HeaderData expectedResult = new HeaderData()
            {
                HeaderDetails = new List<HeaderDetails>()
                    {
                        new HeaderDetails(){
                            HamburgerMenu = new List<HamburgerMenu>()
                            {
                                new HamburgerMenu()
                                {
                                    AdaniAirport = new List<AdaniAirport>()
                                    {
                                        new AdaniAirport()
                                        {
                                            headerText = "section1", headerLeftIcon = "i-section1", headerLink = "linkUrl", headerRightIcon =String.Empty,
                                            items = new List<ChildNavigation>()
                                            {
                                                new ChildNavigation() {
                                                    itemLink = "linkUrl", itemText ="section1Link1", itemLeftIcon = "i-section1Link1", itemRightIcon="i-section1Link1"
                                                },
                                                new ChildNavigation() {
                                                    itemLink = "linkUrl", itemText ="section1Link3", itemLeftIcon = "i-section1Link3", itemRightIcon="i-section1Link3",
                                                    collapseItems = new List<ChildNavigation>()
                                                    {
                                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link3SubLink1", itemLeftIcon = "i-section1Link3SubLink1", itemRightIcon="i-section1Link3SubLink1"},
                                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link3SubLink2", itemLeftIcon = "i-section1Link3SubLink2", itemRightIcon="i-section1Link3SubLink2"}
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    AdaniBusinesses = new List<AdaniBusinesses>()
                                    {
                                        new AdaniBusinesses()
                                        {
                                            headerText = "section2", headerLeftIcon = "i-section2", headerLink = "linkUrl", headerRightIcon ="i-section2",
                                            collapseItems = new List<ChildNavigation>()
                                            {
                                                new ChildNavigation() { itemLink = "linkUrl", itemText ="section2Link1", itemLeftIcon = "i-section2Link1", itemRightIcon="i-section2Link1"},
                                            }
                                        }
                                    },
                                    Others = new List<Others>
                                    {
                                        new Others()
                                        {
                                            headerText = "section3", headerLeftIcon = "i-section3", headerLink = "linkUrl", headerRightIcon ="i-section3",
                                            items = new List<ChildNavigation>()
                                            {
                                                new ChildNavigation() { itemLink = "linkUrl", itemText ="section3Link1", itemLeftIcon = "i-section3Link1", itemRightIcon="i-section3Link1"},
                                                new ChildNavigation() { itemLink = "linkUrl", itemText ="section3Link3", itemLeftIcon = "i-section3Link3", itemRightIcon="i-section3Link3"},
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
            };

            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void LogoDropdownOnly_ReturnsLogoDropdown()
        {
            var resultContents = new Object();
            var logoDropdown = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.LogoDropdownID);

            var link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(link1, Constant.Link_Title, "link1");
            SetItemField(link1, Constant.Link_leftIcon, "i-link1");
            SetItemField(link1, Constant.Link_rightIcon, "i-link1");
            SetItemField(link1, "IsActive", "1");

            var link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(link2, Constant.Link_Title, "link2");
            SetItemField(link2, Constant.Link_leftIcon, "i-link2");
            SetItemField(link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(link2, "IsActive", "0");

            var link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(link3, Constant.Link_Title, "link3");
            SetItemField(link3, Constant.Link_leftIcon, "i-link3");
            SetItemField(link3, Constant.Link_rightIcon, "i-link3");
            SetItemField(link3, "IsActive", "1");

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID, AirportNavigation.AdaniBusinessesID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2");
            SetItemField(section2, "IsActive", "1");

            var section2Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link1, Constant.Link_Title, "section2Link1");
            SetItemField(section2Link1, Constant.Link_leftIcon, "i-section2Link1");
            SetItemField(section2Link1, Constant.Link_rightIcon, "i-section2Link1");
            SetItemField(section2Link1, "IsActive", "1");

            var section2Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link2, Constant.Link_Title, "section2Link2");
            SetItemField(section2Link2, Constant.Link_leftIcon, "i-section2Link2");
            SetItemField(section2Link2, Constant.Link_rightIcon, "i-section2Link2");
            SetItemField(section2Link2, "IsActive", "0");

            var section2Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link3, Constant.Link_Title, "section2Link3");
            SetItemField(section2Link3, Constant.Link_leftIcon, "i-section2Link3");
            SetItemField(section2Link3, Constant.Link_rightIcon, "i-section2Link3");
            SetItemField(section2Link3, "IsActive", "0");
            SetItemField(section2Link3, Constant.Link_Url, "<link text=\"Adani Airport\" linktype=\"external\" url=\"linkUrl\" anchor=\"\" class=\"dropdown-item active\" target=\"_blank\" />");

            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            logoDropdown.Children.Returns(new ChildList(logoDropdown, new ItemList() { link1, link2, link3, section2 }));
            _headerItem.Children.Returns(new ChildList(_headerItem, new ItemList { logoDropdown }));

            _rendering.Item.Returns(_headerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            HeaderData expectedResult = new HeaderData()
            {
                HeaderDetails = new List<HeaderDetails>()
                    {
                        new HeaderDetails(){
                            HeaderLogoDropdown = new List<HeaderLogoDropdown>()
                            {
                                new HeaderLogoDropdown()
                                {
                                    itemLink = "linkUrl", itemText ="link1", itemLeftIcon = "i-link1", itemRightIcon="i-link1"
                                },
                                new HeaderLogoDropdown(){
                                    itemLink = "linkUrl", itemText ="link3", itemLeftIcon = "i-link3", itemRightIcon="i-link3"
                                },
                                new HeaderLogoDropdown(){
                                    itemLink = "linkUrl", itemText ="section2", itemLeftIcon = "i-section2", itemRightIcon="i-section2",
                                    items = new List<ChildNavigation>()
                                    {
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section2Link1", itemLeftIcon = "i-section2Link1", itemRightIcon="i-section2Link1"},
                                    }
                                }
                            }
                        }
                    }
            };

            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void PrimaryHeaderMenuOnly_ReturnsPrimaryHeaderMenu()
        {
            var resultContents = new Object();
            var primaryHeaderMenu = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.PrimaryHeaderMenuID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkTemplateID, AirportNavigation.AdaniAirportID);
            SetItemField(section1, Constant.Link_Title, "section1");
            SetItemField(section1, Constant.Link_leftIcon, "i-section1");
            SetItemField(section1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1, "IsActive", "1");

            var section1Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link1, Constant.Link_Title, "section1Link1");
            SetItemField(section1Link1, Constant.Link_leftIcon, "i-section1Link1");
            SetItemField(section1Link1, Constant.Link_rightIcon, "i-section1Link1");
            SetItemField(section1Link1, "IsActive", "1");

            var section1Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link2, Constant.Link_Title, "section1Link2");
            SetItemField(section1Link2, Constant.Link_leftIcon, "i-section1Link2");
            SetItemField(section1Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1Link2, "IsActive", "0");

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, "i-section1Link3");
            SetItemField(section1Link3, "IsActive", "1");

            var section1Link3SubLink1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3SubLink1, Constant.Link_Title, "section1Link3SubLink1");
            SetItemField(section1Link3SubLink1, Constant.Link_leftIcon, "i-section1Link3SubLink1");
            SetItemField(section1Link3SubLink1, Constant.Link_rightIcon, "i-section1Link3SubLink1");
            SetItemField(section1Link3SubLink1, "IsActive", "1");

            var section1Link3SubLink2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3SubLink2, Constant.Link_Title, "section1Link3SubLink2");
            SetItemField(section1Link3SubLink2, Constant.Link_leftIcon, "i-section1Link3SubLink2");
            SetItemField(section1Link3SubLink2, Constant.Link_rightIcon, "i-section1Link3SubLink2");
            SetItemField(section1Link3SubLink2, "IsActive", "1");
            section1Link3.Children.Returns(new ChildList(section1Link3, new ItemList() { section1Link3SubLink1, section1Link3SubLink2 }));

            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID, AirportNavigation.AdaniBusinessesID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2");
            SetItemField(section2, "IsActive", "1");

            var section2Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link1, Constant.Link_Title, "section2Link1");
            SetItemField(section2Link1, Constant.Link_leftIcon, "i-section2Link1");
            SetItemField(section2Link1, Constant.Link_rightIcon, "i-section2Link1");
            SetItemField(section2Link1, "IsActive", "1");

            var section2Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link2, Constant.Link_Title, "section2Link2");
            SetItemField(section2Link2, Constant.Link_leftIcon, "i-section2Link2");
            SetItemField(section2Link2, Constant.Link_rightIcon, "i-section2Link2");
            SetItemField(section2Link2, "IsActive", "0");

            var section2Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link3, Constant.Link_Title, "section2Link3");
            SetItemField(section2Link3, Constant.Link_leftIcon, "i-section2Link3");
            SetItemField(section2Link3, Constant.Link_rightIcon, "i-section2Link3");
            SetItemField(section2Link3, "IsActive", "0");

            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkTemplateID, AirportNavigation.OtherID);
            SetItemField(section3, Constant.Link_Title, "section3");
            SetItemField(section3, Constant.Link_leftIcon, "i-section3");
            SetItemField(section3, Constant.Link_rightIcon, "i-section3");
            SetItemField(section3, "IsActive", "1");

            var section3Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link1, Constant.Link_Title, "section3Link1");
            SetItemField(section3Link1, Constant.Link_leftIcon, "i-section3Link1");
            SetItemField(section3Link1, Constant.Link_rightIcon, "i-section3Link1");
            SetItemField(section3Link1, "IsActive", "1");

            var section3Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link2, Constant.Link_Title, "section3Link2");
            SetItemField(section3Link2, Constant.Link_leftIcon, "i-section3Link2");
            SetItemField(section3Link2, Constant.Link_rightIcon, "i-section3Link2");
            SetItemField(section3Link2, "IsActive", "0");

            var section3Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link3, Constant.Link_Title, "section3Link3");
            SetItemField(section3Link3, Constant.Link_leftIcon, "i-section3Link3");
            SetItemField(section3Link3, Constant.Link_rightIcon, "i-section3Link3");
            SetItemField(section3Link3, "IsActive", "1");

            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, "i-section4");
            SetItemField(section4, "IsActive", "1");

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, "i-section4Link1");
            SetItemField(section4Link1, "IsActive", "0");

            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5, Constant.Link_Title, "section5");
            SetItemField(section5, Constant.Link_rightIcon, "i-section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-section5");
            SetItemField(section5, "IsActive", "1");
            #endregion

            primaryHeaderMenu.Children.Returns(new ChildList(primaryHeaderMenu, new ItemList() { section1, section2, section3, section4, section5 }));
            _headerItem.Children.Returns(new ChildList(_headerItem, new ItemList { primaryHeaderMenu }));

            _rendering.Item.Returns(_headerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            HeaderData expectedResult = new HeaderData()
            {
                HeaderDetails = new List<HeaderDetails>()
                    {
                        new HeaderDetails() {
                            PrimaryHeaderMenus = new List<PrimaryHeaderMenu>()
                            {
                                new PrimaryHeaderMenu()
                                {
                                    headerText = "section1", headerLeftIcon = "i-section1", headerLink = "linkUrl", headerRightIcon = string.Empty,
                                    items = new List<ChildNavigation>() {
                                         new ChildNavigation() {
                                            itemLink = "linkUrl", itemText ="section1Link1", itemLeftIcon = "i-section1Link1", itemRightIcon="i-section1Link1",
                                        },
                                        new ChildNavigation() {
                                            itemLink = "linkUrl", itemText ="section1Link3", itemLeftIcon = "i-section1Link3", itemRightIcon="i-section1Link3",
                                            collapseItems = new List<ChildNavigation>()
                                            {
                                                new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link3SubLink1", itemLeftIcon = "i-section1Link3SubLink1", itemRightIcon="i-section1Link3SubLink1"},
                                                new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link3SubLink2", itemLeftIcon = "i-section1Link3SubLink2", itemRightIcon="i-section1Link3SubLink2"}
                                            }
                                        },
                                    }
                                },
                                new PrimaryHeaderMenu()
                                {
                                    headerText = "section2", headerLeftIcon = "i-section2", headerLink = "linkUrl", headerRightIcon = "i-section2",
                                    items = new List<ChildNavigation>() {
                                         new ChildNavigation() {
                                            itemLink = "linkUrl", itemText ="section2Link1", itemLeftIcon = "i-section2Link1", itemRightIcon="i-section2Link1"
                                        }
                                    }
                                },
                                new PrimaryHeaderMenu()
                                {
                                    headerText = "section3", headerLeftIcon = "i-section3", headerLink = "linkUrl", headerRightIcon = "i-section3",
                                    items = new List<ChildNavigation>() {
                                         new ChildNavigation() { itemLink = "linkUrl", itemText ="section3Link1", itemLeftIcon = "i-section3Link1", itemRightIcon="i-section3Link1" },
                                         new ChildNavigation() { itemLink = "linkUrl", itemText ="section3Link3", itemLeftIcon = "i-section3Link3", itemRightIcon="i-section3Link3" }
                                    }
                                },
                                new PrimaryHeaderMenu()
                                {
                                    headerText = "section4", headerLeftIcon = "i-section4", headerLink = "linkUrl", headerRightIcon = "i-section4"
                                },
                                new PrimaryHeaderMenu()
                                {
                                    headerText = "section5", headerLeftIcon = "i-section5", headerLink = "linkUrl", headerRightIcon = "i-section5"
                                }
                            }
                        }
                    }
            };

            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));

        }

        [Test]
        public void SearchSuggesterOnly_ReturnsSearchSuggester()
        {
            var resultContents = new Object();
            var searchSuggester = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.SearchSuggesterID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1, Constant.Link_Title, "section1");
            SetItemField(section1, Constant.Link_leftIcon, "i-section1");
            SetItemField(section1, "IsActive", "1");
            SetItemField(section1, Constant.Link_rightIcon, string.Empty);

            var section1Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link1, Constant.Link_Title, "section1Link1");
            SetItemField(section1Link1, Constant.Link_leftIcon, "i-section1Link1");
            SetItemField(section1Link1, Constant.Link_rightIcon, "i-section1Link1");
            SetItemField(section1Link1, "IsActive", "1");

            var section1Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link2, Constant.Link_Title, "section1Link2");
            SetItemField(section1Link2, Constant.Link_leftIcon, "i-section1Link2");
            SetItemField(section1Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1Link2, "IsActive", "0");

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1Link3, "IsActive", "1");

            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2");
            SetItemField(section2, "IsActive", "1");

            var section2Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link1, Constant.Link_Title, "section2Link1");
            SetItemField(section2Link1, Constant.Link_leftIcon, "i-section2Link1");
            SetItemField(section2Link1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section2Link1, "IsActive", "1");

            var section2Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link2, Constant.Link_Title, "section2Link2");
            SetItemField(section2Link2, Constant.Link_leftIcon, "i-section2Link2");
            SetItemField(section2Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section2Link2, "IsActive", "0");

            var section2Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link3, Constant.Link_Title, "section2Link3");
            SetItemField(section2Link3, Constant.Link_leftIcon, "i-section2Link3");
            SetItemField(section2Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section2Link3, "IsActive", "0");

            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3, Constant.Link_Title, "section3");
            SetItemField(section3, Constant.Link_leftIcon, "i-section3");
            SetItemField(section3, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3, "IsActive", "0");

            var section3Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link1, Constant.Link_Title, "section3Link1");
            SetItemField(section3Link1, Constant.Link_leftIcon, "i-section3Link1");
            SetItemField(section3Link1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3Link1, "IsActive", "1");

            var section3Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link2, Constant.Link_Title, "section3Link2");
            SetItemField(section3Link2, Constant.Link_leftIcon, "i-section3Link2");
            SetItemField(section3Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3Link2, "IsActive", "0");

            var section3Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link3, Constant.Link_Title, "section3Link3");
            SetItemField(section3Link3, Constant.Link_leftIcon, "i-section3Link3");
            SetItemField(section3Link3, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3Link3, "IsActive", "1");

            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, string.Empty);
            SetItemField(section4, "IsActive", "1");

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section4Link1, "IsActive", "0");

            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5, Constant.Link_Title, "section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-section5");
            SetItemField(section5, Constant.Link_rightIcon, string.Empty);
            SetItemField(section5, "IsActive", "1");
            #endregion

            searchSuggester.Children.Returns(new ChildList(searchSuggester, new ItemList() { section1, section2, section3, section4, section5 }));
            _headerItem.Children.Returns(new ChildList(_headerItem, new ItemList() { searchSuggester }));

            _rendering.Item.Returns(_headerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            HeaderData expectedResult = new HeaderData()
            {
                HeaderDetails = new List<HeaderDetails>()
                    {
                        new HeaderDetails(){
                            searchSuggesters = new List<SearchSuggester>()
                            {
                                new SearchSuggester()
                                {
                                    headerText = "section1", headerLeftIcon = "i-section1", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                    items = new List<ChildNavigation>()
                                    {
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link1", itemLeftIcon = "i-section1Link1", itemRightIcon="i-section1Link1" },
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link3", itemLeftIcon = "i-section1Link3", itemRightIcon=string.Empty }
                                    }
                                },
                                new SearchSuggester()
                                {
                                    headerText = "section2", headerLeftIcon = "i-section2", headerLink = "linkUrl", headerRightIcon ="i-section2",
                                    items = new List<ChildNavigation>()
                                    {
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section2Link1", itemLeftIcon = "i-section2Link1", itemRightIcon=String.Empty }
                                    }
                                },
                                new SearchSuggester()
                                {
                                    headerText = "section4", headerLeftIcon = "i-section4", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                },
                                new SearchSuggester()
                                {
                                    headerText = "section5", headerLeftIcon = "i-section5", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                }
                            }
                        }
                    }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void UserAccountOnly_ReturnsUserAccount()
        {
            var resultContents = new Object();
            var userAccount = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.UserAccountID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1, Constant.Link_Title, "section1");
            SetItemField(section1, Constant.Link_leftIcon, "i-section1");
            SetItemField(section1, "IsActive", "1");
            SetItemField(section1, Constant.Link_rightIcon, string.Empty);

            var section1Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link1, Constant.Link_Title, "section1Link1");
            SetItemField(section1Link1, Constant.Link_leftIcon, "i-section1Link1");
            SetItemField(section1Link1, Constant.Link_rightIcon, "i-section1Link1");
            SetItemField(section1Link1, "IsActive", "1");

            var section1Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link2, Constant.Link_Title, "section1Link2");
            SetItemField(section1Link2, Constant.Link_leftIcon, "i-section1Link2");
            SetItemField(section1Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1Link2, "IsActive", "0");

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1Link3, "IsActive", "1");

            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2");
            SetItemField(section2, "IsActive", "1");

            var section2Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link1, Constant.Link_Title, "section2Link1");
            SetItemField(section2Link1, Constant.Link_leftIcon, "i-section2Link1");
            SetItemField(section2Link1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section2Link1, "IsActive", "1");

            var section2Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link2, Constant.Link_Title, "section2Link2");
            SetItemField(section2Link2, Constant.Link_leftIcon, "i-section2Link2");
            SetItemField(section2Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section2Link2, "IsActive", "0");

            var section2Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link3, Constant.Link_Title, "section2Link3");
            SetItemField(section2Link3, Constant.Link_leftIcon, "i-section2Link3");
            SetItemField(section2Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section2Link3, "IsActive", "0");
            SetItemField(section2Link3, Constant.Link_Url, "<link text=\"Adani Airport\" linktype=\"external\" url=\"linkUrl\" anchor=\"\" class=\"dropdown-item active\" target=\"_blank\" />");

            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3, Constant.Link_Title, "section3");
            SetItemField(section3, Constant.Link_leftIcon, "i-section3");
            SetItemField(section3, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3, "IsActive", "0");

            var section3Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link1, Constant.Link_Title, "section3Link1");
            SetItemField(section3Link1, Constant.Link_leftIcon, "i-section3Link1");
            SetItemField(section3Link1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3Link1, "IsActive", "1");

            var section3Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link2, Constant.Link_Title, "section3Link2");
            SetItemField(section3Link2, Constant.Link_leftIcon, "i-section3Link2");
            SetItemField(section3Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3Link2, "IsActive", "0");

            var section3Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link3, Constant.Link_Title, "section3Link3");
            SetItemField(section3Link3, Constant.Link_leftIcon, "i-section3Link3");
            SetItemField(section3Link3, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3Link3, "IsActive", "1");

            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, string.Empty);
            SetItemField(section4, "IsActive", "1");

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section4Link1, "IsActive", "0");

            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5, Constant.Link_Title, "section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-section5");
            SetItemField(section5, Constant.Link_rightIcon, string.Empty);
            SetItemField(section5, "IsActive", "1");
            #endregion

            userAccount.Children.Returns(new ChildList(userAccount, new ItemList() { section1, section2, section3, section4, section5 }));
            _headerItem.Children.Returns(new ChildList(_headerItem, new ItemList() { userAccount }));

            _rendering.Item.Returns(_headerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            HeaderData expectedResult = new HeaderData()
            {
                HeaderDetails = new List<HeaderDetails>()
                    {
                        new HeaderDetails(){
                            userAccounts = new List<UserAccount>()
                            {
                                new UserAccount()
                                {
                                    headerText = "section1", headerLeftIcon = "i-section1", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                    items = new List<ChildNavigation>()
                                    {
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link1", itemLeftIcon = "i-section1Link1", itemRightIcon="i-section1Link1" },
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link3", itemLeftIcon = "i-section1Link3", itemRightIcon=string.Empty }
                                    }
                                },
                                new UserAccount()
                                {
                                    headerText = "section2", headerLeftIcon = "i-section2", headerLink = "linkUrl", headerRightIcon ="i-section2",
                                    items = new List<ChildNavigation>()
                                    {
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section2Link1", itemLeftIcon = "i-section2Link1", itemRightIcon=String.Empty }
                                    }
                                },
                                new UserAccount()
                                {
                                    headerText = "section4", headerLeftIcon = "i-section4", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                },
                                new UserAccount()
                                {
                                    headerText = "section5", headerLeftIcon = "i-section5", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                }
                            }
                        }
                    }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void MyAccountsOnly_ReturnsMyAccounts()
        {
            var resultContents = new Object();
            var myAccount = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.MyAccountsID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1, Constant.Link_Title, "section1");
            SetItemField(section1, Constant.Link_leftIcon, "i-section1");
            SetItemField(section1, "IsActive", "1");
            SetItemField(section1, Constant.Link_rightIcon, string.Empty);

            var section1Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link1, Constant.Link_Title, "section1Link1");
            SetItemField(section1Link1, Constant.Link_leftIcon, "i-section1Link1");
            SetItemField(section1Link1, Constant.Link_rightIcon, "i-section1Link1");
            SetItemField(section1Link1, "IsActive", "1");

            var section1Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link2, Constant.Link_Title, "section1Link2");
            SetItemField(section1Link2, Constant.Link_leftIcon, "i-section1Link2");
            SetItemField(section1Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1Link2, "IsActive", "0");

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, string.Empty);
            SetItemField(section1Link3, "IsActive", "1");

            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2");
            SetItemField(section2, "IsActive", "1");

            var section2Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link1, Constant.Link_Title, "section2Link1");
            SetItemField(section2Link1, Constant.Link_leftIcon, "i-section2Link1");
            SetItemField(section2Link1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section2Link1, "IsActive", "1");

            var section2Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link2, Constant.Link_Title, "section2Link2");
            SetItemField(section2Link2, Constant.Link_leftIcon, "i-section2Link2");
            SetItemField(section2Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section2Link2, "IsActive", "0");

            var section2Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2Link3, Constant.Link_Title, "section2Link3");
            SetItemField(section2Link3, Constant.Link_leftIcon, "i-section2Link3");
            SetItemField(section2Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section2Link3, "IsActive", "0");

            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3, Constant.Link_Title, "section3");
            SetItemField(section3, Constant.Link_leftIcon, "i-section3");
            SetItemField(section3, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3, "IsActive", "0");

            var section3Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link1, Constant.Link_Title, "section3Link1");
            SetItemField(section3Link1, Constant.Link_leftIcon, "i-section3Link1");
            SetItemField(section3Link1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3Link1, "IsActive", "1");

            var section3Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link2, Constant.Link_Title, "section3Link2");
            SetItemField(section3Link2, Constant.Link_leftIcon, "i-section3Link2");
            SetItemField(section3Link2, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3Link2, "IsActive", "0");

            var section3Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section3Link3, Constant.Link_Title, "section3Link3");
            SetItemField(section3Link3, Constant.Link_leftIcon, "i-section3Link3");
            SetItemField(section3Link3, Constant.Link_rightIcon, string.Empty);
            SetItemField(section3Link3, "IsActive", "1");

            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, string.Empty);
            SetItemField(section4, "IsActive", "1");

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, string.Empty);
            SetItemField(section4Link1, "IsActive", "0");

            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5, Constant.Link_Title, "section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-section5");
            SetItemField(section5, Constant.Link_rightIcon, string.Empty);
            SetItemField(section5, "IsActive", "1");
            #endregion

            myAccount.Children.Returns(new ChildList(myAccount, new ItemList() { section1, section2, section3, section4, section5 }));
            _headerItem.Children.Returns(new ChildList(_headerItem, new ItemList() { myAccount }));

            _rendering.Item.Returns(_headerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            HeaderData expectedResult = new HeaderData()
            {
                HeaderDetails = new List<HeaderDetails>()
                    {
                        new HeaderDetails(){
                            MyAccounts = new List<MyAccounts>()
                            {
                                new MyAccounts()
                                {
                                    headerText = "section1", headerLeftIcon = "i-section1", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                    items = new List<ChildNavigation>()
                                    {
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link1", itemLeftIcon = "i-section1Link1", itemRightIcon="i-section1Link1" },
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section1Link3", itemLeftIcon = "i-section1Link3", itemRightIcon=string.Empty }
                                    }
                                },
                                new MyAccounts()
                                {
                                    headerText = "section2", headerLeftIcon = "i-section2", headerLink = "linkUrl", headerRightIcon ="i-section2",
                                    items = new List<ChildNavigation>()
                                    {
                                        new ChildNavigation() { itemLink = "linkUrl", itemText ="section2Link1", itemLeftIcon = "i-section2Link1", itemRightIcon=String.Empty }
                                    }
                                },
                                new MyAccounts()
                                {
                                    headerText = "section4", headerLeftIcon = "i-section4", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                },
                                new MyAccounts()
                                {
                                    headerText = "section5", headerLeftIcon = "i-section5", headerLink = "linkUrl", headerRightIcon =string.Empty,
                                }
                            }
                        }
                    }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void AirportListOnly_ReturnsAirportList()
        {
            var resultContents = new Object();
            var airportList = CreateItem(_db, AirportNavigation.ImageWithLinkTemplateID, AirportNavigation.AirportListID);

            var link1 = CreateItem(_db, AirportNavigation.ImageWithLinkTemplateID);
            SetItemField(link1, Constant.AirportCode, "link1");
            SetCheckBoxField(link1, "IsActive", true);

            var link2 = CreateItem(_db, AirportNavigation.ImageWithLinkTemplateID);
            SetItemField(link2, Constant.AirportCode, "link2");
            SetCheckBoxField(link2, "IsActive", true);

            var link3 = CreateItem(_db, AirportNavigation.ImageWithLinkTemplateID);
            SetItemField(link3, Constant.AirportCode, "link3");
            SetCheckBoxField(link3, "IsActive", true);

            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2");
            SetCheckBoxField(section2, "IsActive", true);

            airportList.Children.Returns(new ChildList(airportList, new ItemList() { link1, link2, link3, section2 }));
            SetItemField(airportList, Constant.AirportCode, "airportList");
            SetCheckBoxField(airportList, "IsActive", true);
            _headerItem.Children.Returns(new ChildList(_headerItem, new ItemList { airportList }));

            _rendering.Item.Returns(_headerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                HeaderDataListResolver resolver = new HeaderDataListResolver(Substitute.For<HeaderDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            HeaderData expectedResult = new HeaderData()
            {
                HeaderDetails = new List<HeaderDetails>()
                    {
                        new HeaderDetails(){
                            AirportList = new List<AirportList>()
                            {
                                new AirportList()
                                {
                                    headerLink = "linkUrl", headerText ="linkText", headerImage = "imageUrl",
                                    items = new List<ChildNavigationwithImage>()
                                    {
                                        new ChildNavigationwithImage(){ airportcode = "link1", itemImage = "imageUrl", itemLink = "linkUrl", itemText = "linkText" },
                                        new ChildNavigationwithImage(){ airportcode = "link2", itemImage = "imageUrl", itemLink = "linkUrl", itemText = "linkText" },
                                        new ChildNavigationwithImage(){ airportcode = "link3", itemImage = "imageUrl", itemLink = "linkUrl", itemText = "linkText" }
                                    }
                                }
                            }
                        }
                    }
            };

            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }
    }
}