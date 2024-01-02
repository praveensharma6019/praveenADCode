using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Models
{
    public class DepartureLanding
    {

        public List<Tab> TabContent { get; set; }
    }

    public class Tab
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public SubTitle SubTitles { get; set; }

    }

    public class SubTitle
    {
        public List<DepartureType> SubTitleList { get; set; }
    }
    public class DepartureType
    {
        public string SubTitle { get; set; }
        public string SubSubTitle { get; set; }
        public string SubText { get; set; }
        public ChargeTable Table { get; set; }
        public OfferingList List { get; set; }
        public AdditionalAddOns AddOns { get; set; }
        public GeneralInformation Information { get; set; }
    }

    public class ChargeTable
    {
        public List<TableHeader> Header { get; set; }
        public List<TableRow> Rows { get; set; }
    }
    public class TableHeader
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
    public class TableRow
    {
        public string GuestInfo { get; set; }
        public string Charges { get; set; }
        public string GST { get; set; }
        public string Total { get; set; }
    }
    public class OfferingList
    {
        public string Title { get; set; }
        public List<OfferingItem> Items { get; set; }
    }

    public class OfferingItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class AdditionalAddOns
    {
        public string AddOnsTitle { get; set; }
        public List<OfferingItem> Item { get; set; }
    }

    public class GeneralInformation
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}