using Glass.Mapper.Sc.Configuration.Attributes;
using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models.HomeBuilder
{
    public class HomeBuilderModel
    {
        public virtual string Heading { get; set; }
        [SitecoreChildren]
        public virtual IEnumerable<CardListItem> CardData { get; set; }
    }
}