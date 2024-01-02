using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public partial class CallBackFormInput
    {
        public string app { get; set; }

        public string category { get; set; }

        public string occasion { get; set; }

        public Data1 data { get; set; }

        public List<string> to { get; set; }

    }
    public class Data1
    {
        public Body body { get; set; }

        public Subject subject { get; set; }

        public Data1()
        {
            subject = new Subject();
        }

    }

    public class Body
    {
        [Required]
        public string name { get; set; }

        [Required]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Please enter valid EmailId")]
        public string email { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please enter valid Mobile number")]
        public string contactNumber { get; set; }

        [Required]
        public string organization { get; set; }
    }

    public class Subject
    {
        public string text { get; set; }
    }

}

