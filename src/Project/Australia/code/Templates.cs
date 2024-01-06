using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Australia.Website
{
    public class Templates
    {
        public struct OurTeam
        {
            public static readonly ID ID = new ID("{E6DDFEC8-2209-47A3-9E71-15FB665875C3}");

            public struct Fields
            {
                public static readonly ID Name = new ID("{894BBF9C-A92C-46D0-AFA7-0FE9B522DCBC}");
                public static readonly ID Designation = new ID("{3C29C642-F3B8-4745-881F-229E9A42DB0F}");
                public static readonly ID Description = new ID("{0265D011-7764-4114-8E15-BF2E02901844}");
                public static readonly ID Image = new ID("{E8254731-41A8-4AF2-9238-D0FC29AF4939}");
            }
        }


    }
}