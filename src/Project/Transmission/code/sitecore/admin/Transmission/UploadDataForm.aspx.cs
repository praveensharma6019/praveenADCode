using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Sitecore.Transmission.Website.sitecore.admin.Transmission
{
    public partial class UploadDataForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                UpdateSourceDDL();
            }
        }

        private void UpdateSourceDDL()
        {
            //SourceDDL
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
            Item items = master.GetItem("/sitecore/content/Transmission/Global/Admin/MediaLibraryUpload");

            SourceDDL.DataBind();
            int i = 0;
            foreach (var item in items.Children.ToList())
            {
                var Title = item.Fields["Title"].Value;

                Sitecore.Data.Fields.LinkField linkvalue = item.Fields["LinkUrl"];
                var linkUrl = linkvalue.TargetID;
                SourceDDL.Items.Insert(i, new ListItem(Title, linkUrl.ToString()));
                i++;
            }

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            // uploadFIles();
            //if (!Img.HasFile) return ;
            string FileName = Img.FileName;
            if (FileName.Contains('.'))
            {
                FileName = FileName.Split('.')[0];
            }
            // Get the master database
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");

            Sitecore.Data.ID SourceId = new Sitecore.Data.ID(SourceDDL.SelectedItem.Value.ToString());

            Item sourceItem = master.GetItem(SourceId);

            var options = new Sitecore.Resources.Media.MediaCreatorOptions
            {
                AlternateText = DocumentTitle.Text,
                FileBased = false,
                IncludeExtensionInItemName = false,
                //KeepExisting = false,
                Versioned = false,
                Destination = sourceItem.Paths.FullPath.ToString() + "/" + FileName,
                Database = Sitecore.Configuration.Factory.GetDatabase("master")
            };

            string filename = Server.MapPath(Img.FileName);
            var creator = new MediaCreator();
            var mediaItem = creator.CreateFromStream(Img.PostedFile.InputStream, filename, options);




            // Get the place in the site tree where the new item must be inserted
            Item item = master.GetItem(sourceItem.Paths.FullPath.ToString() + "/" + FileName);

            UpdateTitle(item, DocumentTitle.Text);

            PublishItem(item);


        }

        private void PublishItem(Item item)
        {

            Sitecore.Publishing.PublishOptions publishOptions =
   new Sitecore.Publishing.PublishOptions(item.Database,
                                          Database.GetDatabase("web"),
                                          Sitecore.Publishing.PublishMode.SingleItem,
                                          item.Language,
                                          System.DateTime.Now);  // Create a publisher with the publishoptions
            Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);

            // Choose where to publish from
            publisher.Options.RootItem = item;

            // Publish children as well?
            publisher.Options.Deep = true;

            // Do the publish!
            publisher.Publish();

        }

        private void UpdateTitle(Item item, string FileName)
        {

            item.Editing.BeginEdit();
            try
            {
                // Assign values to the fields of the new item
                item.Fields["Title"].Value = FileName;

                // End editing will write the new values back to the Sitecore
                // database (It's like commit transaction of a database)
                item.Editing.EndEdit();
            }
            catch (System.Exception ex)
            {
                // The update failed, write a message to the log
                Sitecore.Diagnostics.Log.Error("Could not update item " + item.Paths.FullPath + ": " + ex.Message, this);

                // Cancel the edit (not really needed, as Sitecore automatically aborts
                // the transaction on exceptions, but it wont hurt your code)
                item.Editing.CancelEdit();
            }

        }
    }
}