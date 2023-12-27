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
    public class FooterDataListResolverTests
    {
        static Database _db;
        static Rendering _rendering;
        static Item _footerItem;
        static IHelper _helper;

        [SetUp]
        public void Setup()
        {
            _db = Substitute.For<Database>();
            _rendering = Substitute.For<Rendering>();
            _footerItem = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
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
                FooterDataListResolver resolver = new FooterDataListResolver(Substitute.For<FooterDataList>(Substitute.For<ILogRepository>(), _helper));
                nullException = Assert.Throws<NullReferenceException>(() => resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>()));
            }
            Assert.AreEqual("FooterDataListResolver => Rendering Datasource is Empty", nullException.Message);
        }

        [Test]
        public void NoDataSourceChildren_ReturnsEmptyData()
        {
            var resultContents = new Object();
            _rendering.Item.Returns(_footerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                FooterDataListResolver resolver = new FooterDataListResolver(Substitute.For<FooterDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<DefaultRenderingConfiguration>());
            }

            FooterData expectedResult = new FooterData();
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void SeoContentOnly_ReturnsSeoContent()
        {
            var resultContents = new Object();
            var seoContentItem = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.seoContentItemID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.servicesItemID);

            var section1Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link1, Constant.Link_Title, "section1Link1");
            SetItemField(section1Link1, Constant.Link_leftIcon, "i-section1Link1");
            SetItemField(section1Link1, Constant.Link_rightIcon, "i-section1Link1");
            SetCheckBoxField(section1Link1, "IsActive", true);

            var section1Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link2, Constant.Link_Title, "section1Link2");
            SetItemField(section1Link2, Constant.Link_leftIcon, "i-section1Link2");
            SetItemField(section1Link2, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section1Link2, "IsActive", false);

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section1Link3, "IsActive", true);

            SetCheckBoxField(section1, "IsActive", true);
            section1.DisplayName.Returns("Display Name1");
            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.brandItemID);

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

            SetCheckBoxField(section2, "IsActive", true);
            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);

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

            SetCheckBoxField(section3, "IsActive", true);
            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4Link1, "IsActive", false);

            SetCheckBoxField(section4, "IsActive", true);
            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5, Constant.Link_Title, "section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-section5");
            SetItemField(section5, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section5, "IsActive", true);
            #endregion

            SetCheckBoxField(seoContentItem, "IsActive", true);
            seoContentItem.Children.Returns(new ChildList(seoContentItem, new ItemList() { section1, section2, section3, section4, section5 }));
            _footerItem.Children.Returns(new ChildList(_footerItem, new ItemList() { seoContentItem }));

            _rendering.Item.Returns(_footerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                FooterDataListResolver resolver = new FooterDataListResolver(Substitute.For<FooterDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<IRenderingConfiguration>());
            }

            FooterData expectedResult = new FooterData()
            {
                footerDetails = new List<FooterDetails>()
                {
                    new FooterDetails(){
                        seoContents = new List<SeoContents>()
                        {
                            new SeoContents()
                            {
                                Heading = "Display Name1",
                                items = new List<LinkTitlelist> {
                                    new LinkTitlelist(){ LinkTitle = "section1Link1", LinkUrl = "linkUrl" },
                                    new LinkTitlelist(){ LinkTitle = "section1Link3", LinkUrl = "linkUrl" }
                                }
                            },
                            new SeoContents()
                            {
                                Heading = "",
                                items = new List<LinkTitlelist> {
                                    new LinkTitlelist(){ LinkTitle = "section2Link1", LinkUrl = "linkUrl" }
                                }
                            },
                            new SeoContents()
                            {
                                Heading = "",
                                items = new List<LinkTitlelist> {
                                    new LinkTitlelist(){ LinkTitle = "section3Link1", LinkUrl = "linkUrl" },
                                    new LinkTitlelist(){ LinkTitle = "section3Link3", LinkUrl = "linkUrl" }
                                }
                            }
                        }
                    }
                }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void MainNavigationOnly_ReturnsMainNavigations()
        {
            var resultContents = new Object();
            var mainNavigation = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.MainNavigationItemID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.TravelHelpItemID);

            var section1Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link1, Constant.Link_Title, "section1Link1");
            SetItemField(section1Link1, Constant.Link_leftIcon, "i-section1Link1");
            SetItemField(section1Link1, Constant.Link_rightIcon, "i-section1Link1");
            SetCheckBoxField(section1Link1, "IsActive", true);

            var section1Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link2, Constant.Link_Title, "section1Link2");
            SetItemField(section1Link2, Constant.Link_leftIcon, "i-section1Link2");
            SetItemField(section1Link2, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section1Link2, "IsActive", false);

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section1Link3, "IsActive", true);

            var section1Link3SubLink1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3SubLink1, Constant.Link_Title, "section1Link3SubLink1");
            SetItemField(section1Link3SubLink1, Constant.Link_leftIcon, "i-section1Link3SubLink1");
            SetItemField(section1Link3SubLink1, Constant.Link_rightIcon, "i-section1Link3SubLink1");
            SetCheckBoxField(section1Link3SubLink1, "IsActive", true);

            var section1Link3SubLink2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3SubLink2, Constant.Link_Title, "section1Link3SubLink2");
            SetItemField(section1Link3SubLink2, Constant.Link_leftIcon, "i-section1Link3SubLink2");
            SetItemField(section1Link3SubLink2, Constant.Link_rightIcon, "i-section1Link3SubLink2");
            SetCheckBoxField(section1Link3SubLink2, "IsActive", true);
            section1Link3.Children.Returns(new ChildList(section1Link3, new ItemList() { section1Link3SubLink1, section1Link3SubLink2 }));

            SetCheckBoxField(section1, "IsActive", true);
            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.FlightInformationItemID);

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

            SetCheckBoxField(section2, "IsActive", true);
            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.AdaniBusinessesItemID);

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

            SetCheckBoxField(section3, "IsActive", true);
            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.CompanyItemID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4, "IsActive", true);

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4Link1, "IsActive", true);

            SetCheckBoxField(section4, "IsActive", true);
            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.HelpAndSupportItemID);

            var section5Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5Link1, Constant.Link_Title, "section5Link1");
            SetItemField(section5Link1, Constant.Link_leftIcon, "i-section5Link1");
            SetItemField(section5Link1, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section5Link1, "IsActive", true);

            SetCheckBoxField(section5, "IsActive", true);
            section5.Children.Returns(new ChildList(section5, new ItemList() { section5Link1 }));
            #endregion

            #region Section6
            var section6 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);

            var section6Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section6Link1, Constant.Link_Title, "section6Link1");
            SetItemField(section6Link1, Constant.Link_leftIcon, "i-section6Link1");
            SetItemField(section6Link1, Constant.Link_rightIcon, "i-section6Link1");
            SetCheckBoxField(section6Link1, "IsActive", true);

            SetCheckBoxField(section6, "IsActive", true);
            section6.Children.Returns(new ChildList(section2, new ItemList() { section6Link1 }));
            #endregion

            #region Section7
            var section7 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section7, Constant.Link_Title, "section7");
            SetItemField(section7, Constant.Link_leftIcon, "i-section7");
            SetItemField(section7, Constant.Link_rightIcon, "i-section7");
            SetCheckBoxField(section7, "IsActive", true);

            var section7Link1 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
            SetItemField(section7Link1, Constant.Link_Title, "section7Link1");
            SetItemField(section7Link1, Constant.Link_leftIcon, "i-section7Link1");
            SetItemField(section7Link1, Constant.Link_rightIcon, "i-section7Link1");
            SetCheckBoxField(section7Link1, "IsActive", false);

            var section7Link2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section7Link2, Constant.Link_Title, "section7Link2");
            SetItemField(section7Link2, Constant.Link_leftIcon, "i-section7Link2");
            SetItemField(section7Link2, Constant.Link_rightIcon, "i-section7Link2");
            SetCheckBoxField(section7Link2, "IsActive", false);

            SetCheckBoxField(section7, "IsActive", true);
            section7.Children.Returns(new ChildList(section2, new ItemList() { section7Link1, section7Link2 }));
            #endregion

            SetCheckBoxField(mainNavigation, "IsActive", true);
            mainNavigation.Children.Returns(new ChildList(mainNavigation, new ItemList() { section1, section2, section3, section4, section5, section6, section7 }));
            _footerItem.Children.Returns(new ChildList(_footerItem, new ItemList() { mainNavigation }));

            _rendering.Item.Returns(_footerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                FooterDataListResolver resolver = new FooterDataListResolver(Substitute.For<FooterDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<IRenderingConfiguration>());
            }

            FooterData expectedResult = new FooterData()
            {
                footerDetails = new List<FooterDetails>()
                {
                    new FooterDetails(){
                        MainNavigations = new List<MainNavigations>()
                        {
                            new MainNavigations()
                            {
                                Heading = "",
                                items = new List<LinkTitlelist> {
                                    new LinkTitlelist(){ LinkTitle = "section1Link1", LinkUrl = "linkUrl" },
                                    new LinkTitlelist(){ LinkTitle = "section1Link3", LinkUrl = "linkUrl" }
                                }
                            },
                            new MainNavigations()
                            {
                                Heading = "",
                                items = new List<LinkTitlelist> {
                                    new LinkTitlelist(){ LinkTitle = "section2Link1", LinkUrl = "linkUrl" }
                                }
                            },
                            new MainNavigations()
                            {
                                Heading = "",
                                items = new List<LinkTitlelist> {
                                    new LinkTitlelist(){ LinkTitle = "section3Link1", LinkUrl = "linkUrl" },
                                    new LinkTitlelist(){ LinkTitle = "section3Link3", LinkUrl = "linkUrl" }
                                }
                            },
                            new MainNavigations()
                            {
                                Heading = "",
                                items = new List<LinkTitlelist> {
                                    new LinkTitlelist(){ LinkTitle = "section4Link1", LinkUrl = "linkUrl" }
                                }
                            },
                            new MainNavigations()
                            {
                                Heading = "",
                                items = new List<LinkTitlelist> {
                                    new LinkTitlelist(){ LinkTitle = "section5Link1", LinkUrl = "linkUrl" }
                                }
                            },
                            new MainNavigations()
                            {
                                Heading = "",
                                items = new List<LinkTitlelist> {
                                    new LinkTitlelist(){ LinkTitle = "section6Link1", LinkUrl = "linkUrl" }
                                }
                            },
                        }
                    }
                }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void PaymentItemOnly_ReturnsPaymentItem()
        {
            var resultContents = new Object();
            var paymentItem = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.paymentItemID);

            var link1 = CreateItem(_db, AirportNavigation.ImageWithLinkTemplateID);
            SetItemField(link1, Constant.AirportCode, "link1");
            SetCheckBoxField(link1, "IsActive", true);

            var link2 = CreateItem(_db, AirportNavigation.ImageWithLinkTemplateID);
            SetItemField(link2, Constant.AirportCode, "link2");
            SetCheckBoxField(link2, "IsActive", true);

            var link3 = CreateItem(_db, AirportNavigation.ImageWithLinkTemplateID);
            SetItemField(link3, Constant.AirportCode, "link3");
            SetCheckBoxField(link3, "IsActive", false);

            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2");
            SetCheckBoxField(section2, "IsActive", true);

            var section2link1 = CreateItem(_db, AirportNavigation.ImageWithLinkTemplateID);
            SetItemField(link1, Constant.AirportCode, "link1");
            SetCheckBoxField(link1, "IsActive", true);

            section2.Children.Returns(new ChildList(section2, new ItemList() { section2link1 }));

            SetCheckBoxField(paymentItem, "IsActive", true);
            paymentItem.Children.Returns(new ChildList(paymentItem, new ItemList() { link1, link2, link3, section2 }));
            _footerItem.Children.Returns(new ChildList(_footerItem, new ItemList() { paymentItem }));

            _rendering.Item.Returns(_footerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                FooterDataListResolver resolver = new FooterDataListResolver(Substitute.For<FooterDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<IRenderingConfiguration>());
            }

            FooterData expectedResult = new FooterData()
            {
                footerDetails = new List<FooterDetails>()
                {
                    new FooterDetails(){
                        Payments = new List<Payments>()
                        {
                            new Payments()
                            {
                                Heading = "",
                                items = new List<ImageLink>() {
                                    new ImageLink() { Image = "imageUrl", Link = "linkUrl" },
                                    new ImageLink() { Image = "imageUrl", Link = "linkUrl" }
                                }
                            }
                        }
                    }
                }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void FooterSocialLinksOnly_ReturnsFooterSocialLinks()
        {
            var resultContents = new Object();
            var footerSocialLinksItem = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.FooterSocialLinksItemID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1, Constant.Link_Title, "section1");
            SetItemField(section1, Constant.Link_leftIcon, "i-Instagram i-22");
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
            SetCheckBoxField(section1Link2, "IsActive", false);

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section1Link3, "IsActive", true);

            SetCheckBoxField(section1, "IsActive", true);
            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-twitter i-22");
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

            SetCheckBoxField(section2, "IsActive", true);
            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
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

            SetCheckBoxField(section3, "IsActive", true);
            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4, "IsActive", true);

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4Link1, "IsActive", false);

            SetCheckBoxField(section4, "IsActive", true);
            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5, Constant.Link_Title, "section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-facebook i-22");
            SetItemField(section5, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section5, "IsActive", true);
            #endregion

            SetCheckBoxField(footerSocialLinksItem, "IsActive", true);
            footerSocialLinksItem.Children.Returns(new ChildList(footerSocialLinksItem, new ItemList() { section1, section2, section3, section4, section5 }));
            _footerItem.Children.Returns(new ChildList(_footerItem, new ItemList() { footerSocialLinksItem }));

            _rendering.Item.Returns(_footerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                FooterDataListResolver resolver = new FooterDataListResolver(Substitute.For<FooterDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<IRenderingConfiguration>());
            }

            FooterData expectedResult = new FooterData()
            {
                footerDetails = new List<FooterDetails>()
                {
                    new FooterDetails(){
                        SocialLinks = new List<SocialLinks>()
                        {
                            new SocialLinks(){
                                Heading = "",
                                items = new List<LinkURlIcon>
                                {
                                    new LinkURlIcon() { LinkTitle = "section1", LinkUrl = "linkUrl", itemicon = "i-Instagram i-22" },
                                    new LinkURlIcon() { LinkTitle = "section2", LinkUrl = "linkUrl", itemicon = "i-section2" },
                                    new LinkURlIcon() { LinkTitle = "section5", LinkUrl = "linkUrl", itemicon = "i-facebook i-22" }
                                }
                            }
                        }
                    }
                }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void DownloadOnly_ReturnsDownload()
        {
            var resultContents = new Object();
            var downloadItem = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.DownloadItemID);

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

            SetCheckBoxField(downloadItem, "IsActive", true);
            downloadItem.Children.Returns(new ChildList(downloadItem, new ItemList() { link1, link2, link3, section2 }));
            _footerItem.Children.Returns(new ChildList(_footerItem, new ItemList() { downloadItem }));

            _rendering.Item.Returns(_footerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                FooterDataListResolver resolver = new FooterDataListResolver(Substitute.For<FooterDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<IRenderingConfiguration>());
            }

            FooterData expectedResult = new FooterData()
            {
                footerDetails = new List<FooterDetails>()
                {
                    new FooterDetails(){
                        Download = new List<Download>()
                        {
                            new Download()
                            {
                                Heading = "",
                                items = new List<ImageLink> {
                                    new ImageLink() { Image = "imageUrl", Link = "linkUrl" },
                                    new ImageLink() { Image = "imageUrl", Link = "linkUrl" },
                                    new ImageLink() { Image = "imageUrl", Link = "linkUrl" }
                                }
                            }
                        }
                    }
                }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void FooterCopyrightOnly_ReturnsFooterCopyright()
        {
            var resultContents = new Object();
            var footerCopyright = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.FooterCopyRightItemID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1, Constant.Link_Title, "section1");
            SetItemField(section1, Constant.Link_leftIcon, "i-section1 i-22");
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
            SetCheckBoxField(section1Link2, "IsActive", false);

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section1Link3, "IsActive", true);

            SetCheckBoxField(section1, "IsActive", true);
            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section2, Constant.Link_Title, "section2");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2 i-22");
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

            SetCheckBoxField(section2, "IsActive", true);
            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
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

            SetCheckBoxField(section3, "IsActive", true);
            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4, "IsActive", true);

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4Link1, "IsActive", false);

            SetCheckBoxField(section4, "IsActive", true);
            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section5, Constant.Link_Title, "section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-section5 i-22");
            SetItemField(section5, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section5, "IsActive", true);
            #endregion

            SetCheckBoxField(footerCopyright, "IsActive", true);
            footerCopyright.Children.Returns(new ChildList(footerCopyright, new ItemList() { section1, section2, section3, section4, section5 }));
            _footerItem.Children.Returns(new ChildList(_footerItem, new ItemList() { footerCopyright }));

            _rendering.Item.Returns(_footerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                FooterDataListResolver resolver = new FooterDataListResolver(Substitute.For<FooterDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<IRenderingConfiguration>());
            }

            FooterData expectedResult = new FooterData()
            {
                footerDetails = new List<FooterDetails>()
                {
                    new FooterDetails(){
                        CopyRight = new List<copyright>()
                        {
                            new copyright()
                            {
                                Heading = "",
                                items = new List<LinkTitlelist>
                                {
                                    new LinkTitlelist() { LinkTitle = "section1", LinkUrl = "linkUrl" },
                                    new LinkTitlelist() { LinkTitle = "section2", LinkUrl = "linkUrl" },
                                    new LinkTitlelist() { LinkTitle = "section5", LinkUrl = "linkUrl" }
                                }
                            }
                        }
                    }
                }
            };
            Assert.AreEqual(JsonConvert.SerializeObject(expectedResult), JsonConvert.SerializeObject(resultContents));
        }

        [Test]
        public void BottomOnly_ReturnsBottomNav()
        {
            var resultContents = new Object();
            var bottomNav = CreateItem(_db, AirportNavigation.LinkFolderTemplateID, AirportNavigation.BottomNavItemID);

            #region Section1
            var section1 = CreateItem(_db, AirportNavigation.footerBottomNavID);
            SetItemField(section1, Constant.Title, "section1");
            SetItemField(section1, Constant.Link_leftIcon, "i-section1 i-22");
            SetCheckBoxField(section1, "IsActive", true);
            SetItemField(section1, Constant.Image_Path, "imagePath");
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
            SetCheckBoxField(section1Link2, "IsActive", false);

            var section1Link3 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section1Link3, Constant.Link_Title, "section1Link3");
            SetItemField(section1Link3, Constant.Link_leftIcon, "i-section1Link3");
            SetItemField(section1Link3, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section1Link3, "IsActive", true);

            SetCheckBoxField(section1, "IsActive", true);
            section1.Children.Returns(new ChildList(section1, new ItemList() { section1Link1, section1Link2, section1Link3 }));
            #endregion

            #region Section2
            var section2 = CreateItem(_db, AirportNavigation.footerBottomNavID);
            SetItemField(section2, Constant.Title, "section2");
            SetItemField(section2, Constant.Image_Path, "imagePath");
            SetItemField(section2, Constant.Link_leftIcon, "i-section2");
            SetItemField(section2, Constant.Link_rightIcon, "i-section2 i-22");
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

            SetCheckBoxField(section2, "IsActive", true);
            section2.Children.Returns(new ChildList(section2, new ItemList() { section2Link1, section2Link2, section2Link3 }));
            #endregion

            #region section3
            var section3 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
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

            SetCheckBoxField(section3, "IsActive", true);
            section3.Children.Returns(new ChildList(section3, new ItemList() { section3Link1, section3Link2, section3Link3 }));
            #endregion

            #region section4
            var section4 = CreateItem(_db, AirportNavigation.LinkFolderTemplateID);
            SetItemField(section4, Constant.Link_Title, "section4");
            SetItemField(section4, Constant.Link_leftIcon, "i-section4");
            SetItemField(section4, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4, "IsActive", true);

            var section4Link1 = CreateItem(_db, AirportNavigation.LinkTemplateID);
            SetItemField(section4Link1, Constant.Link_Title, "section4Link1");
            SetItemField(section4Link1, Constant.Link_leftIcon, "i-section4Link1");
            SetItemField(section4Link1, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section4Link1, "IsActive", false);

            SetCheckBoxField(section4, "IsActive", true);
            section4.Children.Returns(new ChildList(section4, new ItemList() { section4Link1 }));
            #endregion

            #region section5
            var section5 = CreateItem(_db, AirportNavigation.footerBottomNavID);
            SetItemField(section5, Constant.Image_Path, "imagePath");
            SetItemField(section5, Constant.Title, "section5");
            SetItemField(section5, Constant.Link_leftIcon, "i-section5 i-22");
            SetItemField(section5, Constant.Link_rightIcon, string.Empty);
            SetCheckBoxField(section5, "IsActive", true);
            #endregion

            SetCheckBoxField(bottomNav, "IsActive", true);
            bottomNav.Children.Returns(new ChildList(bottomNav, new ItemList() { section1, section2, section3, section4, section5 }));
            _footerItem.Children.Returns(new ChildList(_footerItem, new ItemList() { bottomNav }));

            _rendering.Item.Returns(_footerItem);
            using (RenderingContext.EnterContext(_rendering))
            {
                FooterDataListResolver resolver = new FooterDataListResolver(Substitute.For<FooterDataList>(Substitute.For<ILogRepository>(), _helper));
                resultContents = resolver.ResolveContents(_rendering, Substitute.For<IRenderingConfiguration>());
            }

            FooterData expectedResult = new FooterData()
            {
                footerDetails = new List<FooterDetails>()
                {
                    new FooterDetails(){
                        BottomNav = new List<BottomNav>()
                        {
                            new BottomNav()
                            {
                                Heading = "",
                                items = new List<BottomNavFields>{ 
                                    new BottomNavFields() { ActiveImage = "imageUrl", Title = "section1", ImagePath = "imagePath", Link = "linkUrl" },
                                    new BottomNavFields() { ActiveImage = "imageUrl", Title = "section2", ImagePath = "imagePath", Link = "linkUrl" },
                                    new BottomNavFields() { ActiveImage = "imageUrl", Title = "section5", ImagePath = "imagePath", Link = "linkUrl" }
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
