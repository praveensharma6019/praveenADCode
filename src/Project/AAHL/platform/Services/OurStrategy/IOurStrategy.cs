using Project.AAHL.Website.Models.OurStrategy;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.AAHL.Website.Services.OurStrategy
{
    public interface IOurStrategy
    {
        CardDetailWithImagesModel GetCardDetailWithImages(Rendering rendering);
    }
}
