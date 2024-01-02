﻿using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.Theming.Platform
{
    public class Constant
    {
        public static readonly string widgetId = "widgetId";
        public static readonly string widgetType = "widgetType";
        public static readonly string title = "title";

        public static readonly string subTitle = "subTitle";
        public static readonly string subItemRadius = "subItemRadius";
        public static readonly string subItemWidth = "subItemWidth";

        public static readonly string gridColumn = "gridColumn";
        public static readonly string Title = "Title";
        public static readonly string StanderedImage = "Standered Image";

        public static readonly string Description = "Description";
        public static readonly string CTA = "CTA";
        public static readonly string aspectRatio = "aspectRatio";
        public static readonly string SubTitle = "Sub Title";
        public static readonly string BorderRadius = "borderRadius";

        public static readonly string left = "Ieft";
        public static readonly string right = "right";
        public static readonly string top = "top";
        public static readonly string bottom = "bottom";

        public static readonly string name = "name";
        public static readonly string deeplink = "deeplink";
        public static readonly string actionId = "actionId";

        public static readonly string enlargeCenterPage = "enlargeCenterPage";
        public static readonly string enableInfiniteScroll = "enableInfiniteScroll";
        public static readonly string autoPlay = "autoPlay";
        public static readonly string viewportFraction = "viewportFraction";

        public static readonly string BackgroundColor = "Background Color";
        public static readonly string TextColor = "Text Color";

        public static readonly string backgroundColor = "backgroundColor";
        public static readonly string tabColor = "tabColor";
        public static readonly string tabSelectedColor = "tabSelectedColor";
        public static readonly string tabTextSelectedColor = "tabTextSelectedColor";
        public static readonly string tabTextColor = "tabTextColor";
        public static readonly string gradientColors = "gradientColors";
        public static readonly string gradientBegin = "gradientBegin";
        public static readonly string gradientEnd = "gradientEnd";
        public static readonly string horizontalGrid = "horizontalGrid";
        public static readonly string crossAxisSpacing = "crossAxisSpacing";
        public static readonly string mainAxisSpacing = "mainAxisSpacing";
        public static readonly string itemVisibility = "itemVisibility";

        public static readonly string titleColor = "titleColor";
        public static readonly string subTitleColor = "subTitleColor";
        public static readonly string descriptionColor = "descriptionColor";
        public static readonly string ctaColor = "ctaColor";
        public static readonly string backGroundColor = "backGroundColor";

    }

    public class Templates
    {
        public static class WidgetItemsCollection
        {
            public static readonly ID ActionTitleTemplateID = new ID("{4E9CBDAF-75C6-4FAC-A64F-2CAE989CC441}");
            public static readonly ID ItemMarginTemplateID = new ID("{1BBDF919-84DA-42A0-B540-E92507FB4733}");
            public static readonly ID SubItemMarginTemplateID = new ID("{C20F49F0-91C3-41B2-BBFA-9FB5510636B3}");
            public static readonly ID WidgetTemplateID = new ID("{370C49CE-6BBF-4CCC-863B-2CC3EFAA328D}");
            public static readonly ID TagNameTemplateID = new ID("{78FFA991-474F-490E-A387-75D82DAD6192}");

            public static readonly ID TabConfigurationTemplateID = new ID("{B072D5F3-9661-4744-A427-18A1FB92DEBA}");
            public static readonly ID GradientConfigurationTemplateID = new ID("{41DCAA69-968B-49D3-91AE-43FE31275B20}");
            public static readonly ID GridConfigurationTemplateID = new ID("{56569AE8-551D-40C9-B210-DAC7E0B6675A}");
            public static readonly ID SubItemColorsTemplateID = new ID("{FB3690D1-8F6B-41AC-A4D8-B8B9DE681D97}");
        }
    }
}