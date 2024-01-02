using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class WidgetModel
    {
        public List<BenefitItemModel> fields { get; set; }=new List<BenefitItemModel>();
        public NotesModel notes { get; set; }=new NotesModel();
    }
}