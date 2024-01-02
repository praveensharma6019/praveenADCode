using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.Theming.Platform.Models
{
    public class WidgetItem
    {
        public int widgetId { get; set; }
        public string widgetType { get; set; }
        public string title { get; set; }
        public string subTitle { get; set; }
        public double subItemRadius { get; set; }
        public double subItemWidth { get; set; }
        public int gridColumn { get; set; }
        public double aspectRatio { get; set; }
        public double borderRadius { get; set; }
        public string backgroundColor { get; set; }
        public ItemCSS itemMargin { get; set; }
        public ItemCSS subItemMargin { get; set; }

        public ActionTitle actionTitle { get; set; }
        public CarouselParam carouselParam { get; set; }

        public TabConfiguration tabConfiguration { get; set; }
        public GradientConfiguration gradientConfiguration { get; set; }
        public GridConfiguration gridConfiguration { get; set; }
        public SubItemColors subItemColors { get; set; }
        public List<Object> widgetItems { get; set; }

        //public object widgetItem { get; set; }

    }
    public class ItemCSS
    {
        public double left { get; set; }
        public double right { get; set; }
        public double bottom { get; set; }
        public double top { get; set; }
    }

    public class ActionTitle
    {
        public int actionId { get; set; }
        public string deeplink { get; set; }
        public string name { get; set; }
    }

    public class CarouselParam
    {
        public string enlargeCenterPage { get; set; }
        public string enableInfiniteScroll { get; set; }
        public string autoPlay { get; set; }
        public string viewportFraction { get; set; }
    }
    public class WidgetModel
    {
        public WidgetItem widget { get; set; }
    }

    public class TabConfiguration 
    {
        public string tabColor { get; set; }
        public string tabSelectedColor { get; set; }
        public string tabTextSelectedColor { get; set; }
        public string tabTextColor { get; set; }
    }

    public class GradientConfiguration 
    {
        public List<string> gradientColors { get; set; }
        public string gradientBegin { get; set; }
        public string gradientEnd { get; set; }
    }

    public class GridConfiguration 
    {
        public bool horizontalGrid { get; set; }
        public double crossAxisSpacing { get; set; }
        public double mainAxisSpacing { get; set; }
        public double itemVisibility { get; set; }
    }

    public class SubItemColors
    {
        public string titleColor { get; set; }
        public string subTitleColor { get; set; }
        public string descriptionColor { get; set; }
        public string ctaColor { get; set; }
        public string backGroundColor { get; set; }
    }


}