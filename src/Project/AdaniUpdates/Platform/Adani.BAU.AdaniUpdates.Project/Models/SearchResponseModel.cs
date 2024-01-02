using System.Collections.Generic;

namespace Adani.BAU.AdaniUpdates.Project.Models
{
    public class SearchResponseModel<TResult>
    {
        public bool NextPage { get; set; }
        public IEnumerable<TResult> Result { get; set; }
    }
}