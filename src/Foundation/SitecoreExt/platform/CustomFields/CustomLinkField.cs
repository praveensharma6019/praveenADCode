using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.CustomFields
{
    public class CustomLinkField : Link
    {
        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull((object)message, nameof(message));
            base.HandleMessage(message);
            if (message["id"] != this.ID)
                return;
            string name = message.Name;

            switch (name)
            {
                case "contentlink:reactapplink":
                    {
                        string uri = UIUtil.GetUri("control:SearchXYZSystemLinkForm");
                        UrlString urlString = new UrlString(uri);
                        string strUrlString = urlString.ToString();
                        this.Insert(strUrlString);
                        break;
                    }
            }
        }
    }
}