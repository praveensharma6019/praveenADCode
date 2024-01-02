using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class TestimonialModel
    {
        public Testimonial testimonial { get; set; }
    }

    public class Testimonial
    {
        public string heading { get; set; }
        public List<TestimonialDatum> testimonialData { get; set; }
    }

    
}