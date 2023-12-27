using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Project.AdaniOneSEO.Website.Templates;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace Project.AdaniOneSEO.Website.Services.FlightsToDestination
{
    public class PopularFlights : IPopularFlights
    {
        //private readonly ISitecoreService _sitecoreService;
        //public PopularFlights(ISitecoreService sitecoreService)
        //{
        //    _sitecoreService = sitecoreService;
        //}
        //public PopularFlightsModel GetPopularFlights(Rendering rendering)
        //{
        //    var datasource = Utils.GetRenderingDatasource(rendering);
        //    if (datasource == null) return null;

        //    var data = _sitecoreService.GetItem<PopularFlightsModel>(datasource);
        //    return data;
        //}
        public PopularFlightsModel GetPopularFlights(Rendering rendering)
        {
            PopularFlightsModel popularFlightsDataModel = new PopularFlightsModel();
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;
                
                var multiListItems = Utils.GetMultiListValueItem(datasource, PopularFlightsTemplate.PopularFlightMultiListFieldId);
                TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
                if (multiListItems != null)
                {
                    List<PopularFlightsList> popularFlightsList = new List<PopularFlightsList>();

                    foreach (Item multiListItem in multiListItems)
                    {
                        var treeListItems = Utils.GetMultiListValueItem(multiListItem, PopularFlightsTemplate.PopularFlightTreeListFieldId);
                        var popularFlights = new PopularFlightsList
                        {
                            Heading = Utils.GetValue(multiListItem, PopularFlightsTemplate.HeadingFieldId),
                            PopularFlightsItems = new List<PopularFlightsItemsList>()
                        };

                        if (treeListItems != null)
                        {
                            
                            foreach (Item treeListItem in treeListItems)
                            {
                                string itemId = "";
                                string itemPath = "";
                                if (datasource != null)
                                {
                                     itemId = datasource.ID.ToString();
                                     itemPath = datasource.Paths.FullPath;
                                }
                                string UAT = "https://sitecorecdaks.uat.adanione.com";
                                string Production = "https://sitecorecdaks.adanione.com";
                                string Dev = "https://sitecorecddevaks.adanione.cloud";

                                PopularFlightsItemsList PopularFlightsItemObject = new PopularFlightsItemsList();
                                PopularFlightsItemsListComponent popularFlightsLinkObject = new PopularFlightsItemsListComponent();
                                LinkField linkfield = treeListItem.Fields[PopularFlightsTemplate.LinkFieldId];
                                string itemname = treeListItem.Name.Replace("-", " ");
                                string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
                                string url = "";
                                string folder = "";
                                #region Folder Check
                                if (itemPath != null && itemPath.Contains("domestic-flights"))
                                {
                                    folder = "/domestic-flights";
                                }
                                else if(itemPath.Contains("international-flights"))
                                {
                                    folder = "/international-flights";
                                }
                                #endregion
                                if (domainName == Dev) 
                                {
                                    domainName = "https://aksdev.adanione.cloud";
                                    url = domainName + folder + "/" + treeListItem.Name;
                                }
                                else if (domainName == UAT)
                                {
                                    domainName = "https://www.uat.adanione.com";
                                    url = domainName + folder + "/" + treeListItem.Name;
                                }
                                else if (domainName == Production)
                                {
                                    domainName = "https://www.adanione.com";
                                    url = domainName + folder + "/" + treeListItem.Name;
                                }
                                else
                                {
                                    url = domainName + folder + "/" + treeListItem.Name;
                                }
                                
                                #region old code
                                //popularFlights.PopularFlightsItems.Add(popularFlightsItem);
                                //popularFlightsLinkObject.Url = Utils.GetLinkURL(treeListItem.Fields[PopularFlightsTemplate.LinkFieldId]);
                                //popularFlightsLinkObject.Text = linkfield.Text;
                                #endregion
                                popularFlightsLinkObject.Url = url;
                                popularFlightsLinkObject.Text = textInfo.ToTitleCase(itemname);
                                popularFlightsLinkObject.Target = linkfield.Target;
                                
                                PopularFlightsItemObject.Link = popularFlightsLinkObject;
                                popularFlights.PopularFlightsItems.Add(PopularFlightsItemObject);
                            }
                        }


                        popularFlightsList.Add(popularFlights);
                    }

                    popularFlightsDataModel.PopularFlights = popularFlightsList;
                }
            }
            catch (Exception ex)
            {
               
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return popularFlightsDataModel;
        }
    }



}
