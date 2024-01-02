using Project.AdaniInternationalSchool.Website.Helpers;
using Project.AdaniInternationalSchool.Website.Models;
using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Services
{
    public class CardListService : ICardListService
    {
        public List<CardListItemModel> Render(Rendering rendering)
        {
            var cardList = new List<CardListItemModel>();

            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return null;

            try
            {
                foreach (Item listItem in dsItem.Children)
                {
                    cardList.Add(new CardListItemModel
                    {
                        Heading = Utils.GetValue(listItem, BaseTemplates.TitleTemplate.TitleFieldId, listItem.Name),
                        Description = Utils.GetValue(listItem, BaseTemplates.DescriptionTemplate.DescriptionFieldId),
                        ImageAlt = Utils.GetValue(listItem, BaseTemplates.ImageSourceTemplate.ImageAltFieldId),
                        ImageSource = Utils.GetImageURLByFieldId(listItem, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId),
                        ImageSourceTablet = Utils.GetImageURLByFieldId(listItem, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId),
                        ImageSourceMobile = Utils.GetImageURLByFieldId(listItem, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId),
                        ReverseColumn = Utils.GetBoleanValue(listItem, CardListItemTemplate.Fields.ReverseColumnFieldId),
                        Theme = Utils.GetValue(listItem, CardListItemTemplate.Fields.ThemeFieldId)
                    });
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return cardList;
        }
    }
}