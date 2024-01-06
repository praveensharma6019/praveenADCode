using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Sitecore.Marathon.Website;

namespace Sitecore.Marathon.Website
{
    public static class CustomHelpers
    {
        public class CustomSelectItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
            public string Class { get; set; }
            public int MaxAllowedCount { get; set; }
            public bool Disabled { get; set; }
            public bool SelectedValue { get; set; }
        }

        public static MvcHtmlString CustomDropdownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<CustomSelectItem> list, bool selectedValue, string optionLabel, object htmlAttributes = null)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            string name = ExpressionHelper.GetExpressionText((LambdaExpression)expression);
            return CustomDropdownList(htmlHelper, metadata, name, optionLabel, list, selectedValue, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }


        private static MvcHtmlString CustomDropdownList(this HtmlHelper htmlHelper, ModelMetadata metadata, string name, string optionLabel, IEnumerable<CustomSelectItem> list, bool selectedValue, IDictionary<string, object> htmlAttributes)
        {
            string fullName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (String.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException("name");
            }

            TagBuilder dropdown = new TagBuilder("select");
            dropdown.Attributes.Add("name", fullName);
            dropdown.MergeAttribute("data-val", "true");
            dropdown.MergeAttribute("data-val-required", "Mandatory field");
            dropdown.MergeAttribute("data-val-number", "The field must be a number.");
            dropdown.MergeAttributes(htmlAttributes); //dropdown.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            dropdown.MergeAttributes(htmlHelper.GetUnobtrusiveValidationAttributes(name, metadata));

            StringBuilder options = new StringBuilder();

            // Make optionLabel the first item that gets rendered.
            if (optionLabel != null)
                options.Append("<option value='" + String.Empty + "'>" + optionLabel + "</option>");

            foreach (var item in list)
            {
                if (item.SelectedValue == true && item.Disabled == true)
                    options.Append("<option value='" + item.Value + "' class='" + item.Class + "' selected disabled>" + item.Text + "</option>");
                else if (item.SelectedValue != true && item.Disabled == true)
                    options.Append("<option value='" + item.Value + "' class='" + item.Class + "' disabled>" + item.Text + "</option>");
                else if (item.SelectedValue == true && item.Disabled != true)
                    options.Append("<option value='" + item.Value + "' class='" + item.Class + "' selected>" + item.Text + "</option>");
                else
                    options.Append("<option value='" + item.Value + "' class='" + item.Class + "'>" + item.Text + "</option>");
            }
            dropdown.InnerHtml = options.ToString();
            return MvcHtmlString.Create(dropdown.ToString(TagRenderMode.Normal));
        }


    }

}