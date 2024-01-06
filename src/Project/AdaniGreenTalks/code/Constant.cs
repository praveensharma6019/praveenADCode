using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website
{
    public static class Constant
    {
      
        public static string logoFieldName = "Logo";
        public static string Link = "Link";
        public static string Image = "Image";
        public static string ShortTitle = "ShortTitle";
        public static string Text = "Text";

        public static string MediaTitle = "MediaTitle";
        public static string MediaThumbnail = "MediaThumbnail";
        public static string MediaDescription = "MediaDescription";
        public static string LinkUrl = "LinkUrl";
        public static string Title = "Title";
        public static string Header = "Header";
        public static string Intro = "Intro";
        public static string thumbnail = "thumbnail";

        public static string Summary = "Summary";
        public static string MediaImage = "MediaImage";
        public static string ButtonLink = "ButtonLink";
        public static string SubTitle = "SubTitle";
        public static string ButtonTitle = "ButtonTitle";
        public static string MediaInfo = "MediaInfo";

        public static string Question = "Question";
        public static string Answer = "Answer";
        public static string Designation = "Designation";
        public static string Thumbnail = "Thumbnail";
        public static string Description = "Description";
        public static string ScrollDiv = "ScrollDiv";
        public static string Class = "Class";
        public static string SVGLeft = "SVGLeft";
        public static string SVGRight = "SVGRight";
        public static string MediaVideoLink = "MediaVideoLink";
        public static string IconImage = "IconImage";
        public static string Body = "Body";
        public static string NewsArticleContent = "NewsArticleContent";
        public static string NewsTitle = "NewsTitle";
        public static string RichTextContent = "RichTextContent";
        public static string DateText = "DateText";
        public static string parentitem = "parentitem";
        public static string Heading = "Heading";
        public static string ShortDescription = "ShortDescription";
        public static string Copyright = "Copyright";
        public static string MainImage = "MainImage";
        public static string cssClass = "cssClass";
        public static readonly string MsgNullCheck = "* Please enter your $val$.";
        public static readonly string MsgValidCheck = "* Please enter valid $val$.";
       // public static readonly string MsgInvalidValidCheck = "* invalid value enter $val$. (<,>,','\',&,'\")";
        public static readonly string MsgInvalidValidCheck = "* invalid value entered.";
        public static readonly string MsgSelectValidCheck = "* Please select valid $val$.";
        public static readonly string MsgValidLengthCheck = "* Please enter valid length.";
        public static readonly string MsgValidFilePDFCheck = "* Please upload valid PDF.";
        public static readonly string MsgValidFilePhotoCheck = "* Please upload valid Image file (gif,png,jpeg,jpg)";
        public static readonly string MsgValidFileNameCheck = "* Please upload valid file name (The following characters are not allowed: \\ /:* ? \" < > | # % ! @)";
        public static readonly string MsgInValidFilePhotoextensionCheck = "* Please upload valid file with single extension only";
        public static readonly string MsgInValidFilepdfextensionCheck = "* Please upload valid file with single extension only";
        public static readonly string message = "Please try aftersome time, we are facing some issue.";
        public static readonly string recaptchaVerificationFailed = "Captcha failed";
        public static readonly string errorOccurred = "Some error occurred";
    }
    public static class MsgStatusCode
    {
        public static readonly string FirstNameCode = "405";
        public static readonly string LastNameCode = "406";
        public static readonly string EmailCode = "407";
        public static readonly string ContactNoCode = "408";
        public static readonly string MessageCode = "409";
        public static readonly string CityCode = "410";
        public static readonly string Goal = "411";
        public static readonly string FellowName = "412";
        public static readonly string MoblieNoCode = "413";      
        public static readonly string FirstNameNomineeCode = "414";
        public static readonly string LastNameNomineeCode = "415";
        public static readonly string EmailNomineeCode = "416";
        public static readonly string CountryCode = "417";
        public static readonly string GenderCode = "418";
        public static readonly string UploadPhotoCodeCode = "419";
        public static readonly string UploadoriginalconceptCode = "420";
        public static readonly string UploadBiographyCode = "421";
        public static readonly string TakeawayCode = "422";
        public static readonly string linkforarticleCode = "423";
        public static readonly string linkaudioorvideo = "424";
        //public static readonly string fileUploadPhotoName = "425";

        

    }
    public static class SitecoreConstant
    {
        public static readonly ID Goals = new ID("{144A4C83-B96B-4460-997B-90EC44E197DF}");
        public static readonly ID Fellows = new ID("{31D1CC8A-58EF-47BC-8432-594B1FC7443E}");
        public static readonly ID NameValueCollection = new ID("{2ECB55C7-A206-4F91-9E3B-D5B4F6EE15E7}");
    }

    public struct AdaniGreenTalkGTMModel
    {
        public static string strGTMScriptHeadBodyTag = "{350E231E-5DA5-4676-BC39-D30FE175702C}";

    }
}
