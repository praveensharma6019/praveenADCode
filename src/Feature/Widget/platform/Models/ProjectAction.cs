using Sitecore.Data.Items;
using System;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Models
{
    public class ProjectAction
    {
        public PAction ProjectActions { get; set; }

        public static implicit operator Item(ProjectAction v)
        {
            throw new NotImplementedException();
        }
    }
}