using Project.AdaniInternationalSchool.Website.Helpers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Services.Text
{
    public class TextService : ITextService
    {
        public Dictionary<string, string> Render(Rendering rendering)
        {
            var response = new Dictionary<string, string>();
            var dsItem = Utils.GetRenderingDatasource(rendering);
            if (dsItem == null) return null;

            try
            {
                response.Add(GetValueInCamelCase(dsItem.Name), dsItem.Fields["Value"]?.Value);
            }
            catch (Exception ex)
            {

                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return response;
        }

        private string GetValueInCamelCase(string value)
        {
            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}