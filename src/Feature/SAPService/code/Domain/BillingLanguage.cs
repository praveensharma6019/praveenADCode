using System.ComponentModel;

namespace SapPiService.Domain
{
    public enum BillingLanguage
    {
        [Description("English")]
        English,
        
        [Description("Hindi")]
        Hindi,
        
        [Description("Gujarati")]
        Gujarati,
        
        [Description("Marathi")]
        Marathi
    }
}