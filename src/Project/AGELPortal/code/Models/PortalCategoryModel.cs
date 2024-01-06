using System;
using System.Linq;
using System.Web;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Sitecore.AGELPortal.Website.Models
{
    [Serializable]
    public class PortalCategoryModel
    {


        [Required(ErrorMessage = "Name is Required")]
        //[StringLength(20, MinimumLength = 40, ErrorMessage = "Must be at least 4 characters long.")]

        public string name
        {
            get;
            set;
        }

        
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("AGELPortal/Form/Invalid Name", "Please enter a valid categoryName.");
            }
        }
        public Guid Id
        {
            get;
            set;
        }
        public string UserValidation { get; set; }

        public string status { get; set; }
        public string validation { get; set; }
        public List<AGElPortalCategory> categories { get; set; }
        public int totalRecord { get; set; }
        public int page { get; set; }
        public string mType { get; set; }
        

        public dynamic GetContentCount(Guid Id,string contetn_type) {

            AGELPortalDataContext rdb = new AGELPortalDataContext();
            var count = rdb.AGELPortalContents.Where(x => x.category_id == Id && x.contetn_type == contetn_type).Count();
            return count;
        }
    }
}