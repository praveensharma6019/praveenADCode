﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class BaseImageContentModel : BaseContentModel
    {
        public string ImageSource { get; set; }
        public string ImageSourceMobile { get; set; }
        public string ImageSourceTablet { get; set; }
        public string ImageAlt { get; set; }
    }
}