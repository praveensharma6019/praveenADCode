using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public class Airports
    {
        public string AirportCode { get; set; }

        public string AirportName { get; set; }

        public string AirportAddress { get; set; }

        public string AirportMobile { get; set; }

        public string AirportEmail { get; set; }

        public string SupportEmail { get; set; }

        public string EmailText { get; set; }
        public string NodalName { get; set; }

        public string NodalMobile { get; set; }

        public string NodalEmail { get; set; }

        public string NodalEmail1 { get; set; }

        public string AppellateName { get; set; }

        public string AppellateMobile { get; set; }

        public string AppellateEmail { get; set; }

        public string TerminalContent { get; set; }

        public List<Terminal> TerminalDetails { get; set; }

        public string FAQImage { get; set; }

        public string FAQTitle { get; set; }

        public string FAQContent { get; set; }

        public string FAQLink { get; set; }

        public string FAQLinkText { get; set; }

        public List<AuthoritiesContacts> AuthoritiesContacts { get; set; }
      
        public string TerminalContentTitle { get; set; }

        public string TerminalContentMain{ get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Keywords { get; set; }
        public string Canonical { get; set; }
        public string Viewport { get; set; }
        public string Robots { get; set; }
        public string OG_Title { get; set; }
        public string OG_Image { get; set; }
        public string OG_Description { get; set; }

    }

    public class AuthoritiesContacts
    {
        public string Title { get; set; }

        public string Name { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }
    }


}