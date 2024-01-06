using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniConneX.Website.Templates
{
    public class EmailTemplate
    {
        public class Datasource
        {
            public readonly static ID ThankyouTemplate = new ID("{509869D0-40C8-405B-B32A-AF53DE28FFF5}");
            public readonly static ID LeadDataTemplate = new ID("{976B648E-8BBA-404E-A3DC-FCB912F965EA}");
            public readonly static ID EmailFormTypeList = new ID("{B7DF1866-526F-45EB-95BE-FF0CFFF10442}");
            public readonly static ID EmailSendValidation = new ID("{06E9853C-2644-414E-AF72-50AC2E6476EB}");
        }
        public class DatasourceFields
        {
            public readonly static ID To = new ID("{D3F8593E-ABAE-4C7A-9559-3BBB18CDCADB}");
            public readonly static ID From = new ID("{8BAF2A72-2B88-45DC-A3F4-B1DEF560AC27}");
            public readonly static ID CC = new ID("{1390EB1C-5441-47C9-AA5F-02DEC2C457D2}");
            public readonly static ID BCC = new ID("{605C3A11-A94F-4E31-8C45-058BA5C544CB}");
            public readonly static ID Subject = new ID("{A3800AE3-FDB7-41A8-BD35-EF5BF4BB063F}");
            public readonly static ID Body = new ID("{B3853377-370C-4F43-8C73-82B172AEFCF8}");

            public readonly static ID SendEmail = new ID("{99A9169A-CC51-4BE9-9165-9A1F5B243629}");
            public readonly static ID Form = new ID("{A48F02EC-3116-4840-83B8-22CADE73CF91}");

            public readonly static ID SendFormCheckList = new ID("{1326C5B4-F98B-4D72-8645-8315499FB1D3}");
        }
    }
}
