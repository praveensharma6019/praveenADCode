using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models
{
    public class CombinedTravelInsurance
    {
        public TravelInsurance TravelInsuranceDetails { get; set; }
        public ZeroCancellation ZeroCancellationDetails { get; set; }
    }

    public class ZeroCancellation
    {
        public string AmountLabel { get; set; }
        public string Icon { get; set; }
        public string ZeroCancellationHeading { get; set; }
        public string TnCLabel { get; set; }
        public string Disclaimer { get; set; }
        public string TnCLabelLink { get; set; }
        public string RefundLabel { get; set; }
        public ZeroCancellationDescription Description { get; set; }
        public ZeroCancellationModelBox ModelBox { get; set; }
        
    }
    public class TravelInsurance
    {
        public string ModelTitle { get; set; }
        public string BrandLogo { get; set; }
        public string Heading { get; set; }
        public string Error { get; set; }
        public string TravelLogo { get; set; }
        public Disclaimer disclaimer { get; set; }
        public Information info { get; set; }
        public List<Benefits> benefits { get; set; }
        public List<Breakups> breakups { get; set; }
        public List<Options> options { get; set; }

    }

    public class ZeroCancellationDescription
    {
        public string Label { get; set; }
        public string LabelText { get; set; }
        public ZeroCancellationPlaceholder Placeholder { get; set; }
    }

    public class ZeroCancellationPlaceholder
    {
        public string HowToWork { get; set; }
        public string HowToWorkLink { get; set; }
    }

    public class ZeroCancellationModelBox
    {
        public string AdditionalBenefit { get; set; }
        public string Heading { get; set; }
        public ZeroBreakup WithZeroBreakup { get; set; }
        public ZeroBreakup WithoutZeroBreakup { get; set; }
        public PleaseNoteModel PleaseNote { get; set; }
        public CancellationProcessModel CancellationProcess { get; set; }
    }

    public class CancellationProcessModel
    {
        public string Heading { get; set; }
        public List<CancellationProcessModelNotesModel> Notes { get; set; }
    }
    public class CancellationProcessModelNotesModel
    {
        public string Label { get; set; }
        public CancellationProcessPlaceholder Placeholder { get; set; }
    }
    public class CancellationProcessPlaceholder
    {
        public string Vendor { get; set; }
        public string VendorLink { get; set; }
    }
    public class PleaseNoteModel
    {
        public string Heading { get; set; }
        public List<NotesModel> Notes { get; set; }
    } 

    public class NotesModel
    {
        public string Label { get; set; }
        public Placeholder Placeholder { get; set; }
    }
    public class ZeroBreakup
    {
        public string Heading { get; set; }
        public string Icon { get; set; }
        public List<BreakupModel> Breakup { get; set; }
    }
    
    public class BreakupModel
    {
        public string Label { get; set; }
        public string Code { get; set; }
        public string Hint { get; set; }
    }
        public class Disclaimer
        {
            public string Label { get; set; }
            public Placeholder Placeholder { get; set; }
        }

        public class Information
        {
            public string Label { get; set; }
            public Placeholder Placeholder { get; set; }
        }

        public class Breakups
        {
            public string Amount { get; set; }
            public string Label { get; set; }
        }

        public class Benefits
        {
            public string Icon { get; set; }
            public string Description { get; set; }
            public string Title { get; set; }
            public string IconUrl { get; set; }
        }

        public class Options
        {
            public string Id { get; set; }
            public string Tag { get; set; }
            public string Label { get; set; }
        }

        public class Placeholder
        {
            public string TNC { get; set; }
            public string TNCLink { get; set; }
            public string Amount { get; set; }
            public string TNCApp { get; set; }
        }
}