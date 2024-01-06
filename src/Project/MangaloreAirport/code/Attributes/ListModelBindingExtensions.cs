using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Sitecore.MangaloreAirport.Website.Attributes
{
    public static class ListModelBindingExtensions
    {
        public static Regex _stripIndexerRegex;

        static ListModelBindingExtensions()
        {
            ListModelBindingExtensions._stripIndexerRegex = new Regex("\\[(?<index>\\d+)\\]", RegexOptions.Compiled);
        }

        public static int GetIndex(this TemplateInfo templateInfo)
        {
            int num;
            string fieldName = templateInfo.GetFullHtmlFieldName("Index");
            Match match = ListModelBindingExtensions._stripIndexerRegex.Match(fieldName);
            num = (!match.Success ? 0 : int.Parse(match.Groups["index"].Value));
            return num;
        }

        public static string GetIndexerFieldName(this TemplateInfo templateInfo)
        {
            string fieldName = templateInfo.GetFullHtmlFieldName("Index");
            fieldName = ListModelBindingExtensions._stripIndexerRegex.Replace(fieldName, string.Empty);
            if (fieldName.StartsWith("."))
            {
                fieldName = fieldName.Substring(1);
            }
            return fieldName;
        }

        public static MvcHtmlString HiddenIndexerInputForModel<TModel>(this HtmlHelper<TModel> html)
        {
            string name = html.ViewData.TemplateInfo.GetIndexerFieldName();
            object value = html.ViewData.TemplateInfo.GetIndex();
            string markup = string.Format("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", name, value);
            return MvcHtmlString.Create(markup);
        }
    }
}