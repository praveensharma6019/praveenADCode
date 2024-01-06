using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Resources.Media;
using Sitecore.Shell.Applications.ContentEditor;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Transmission.Website.sitecore.admin.Transmission
{
    public partial class UploadData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblErroMsg.Text = "";

            if (!IsPostBack)
            {
                UpdateSourceDDL();
                if (Request.QueryString["Sorce"] != null && Request.QueryString["Sorce"] != string.Empty)
                {
                   string querystring= Request.QueryString["Sorce"];
                    if(querystring== "CorporateAnnouncement")
                    { SourceDDL.Items.FindByText("CorporateAnnouncement").Selected = true; }
                    if (querystring == "NewsAdvertisement")
                    { SourceDDL.Items.FindByText("NewsAdvertisement").Selected = true; }
                    if (querystring == "OtherDownloads")
                    { SourceDDL.Items.FindByText("OtherDownloads").Selected = true; }
                    //lblName.Text = Request.QueryString["CorporateAnnouncement"]; 
                }
                
            }
        }

        private void UpdateSourceDDL()
        {
            //SourceDDL
            Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase("master");
            Item items = master.GetItem("/sitecore/content/Transmission/Global/Admin/CreateSubItems");

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
            try
            {
                // uploadFIles();
                //if (!Img.HasFile) return ;
                string linkUrl1 = "";
               
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

                string path = sourceItem.Paths.FullPath.ToString() + "/" + FileName;
                UpdateTitle(item, DocumentTitle.Text);

                PublishItem(item);

                if (SourceDDL.SelectedItem.Text == "CorporateAnnouncement")
                {
                    Item myItem = master.GetItem("/sitecore/content/Transmission/Global/Admin/CreateSubItems/CorporateAnnouncement");
                    Sitecore.Data.Fields.LinkField linkvalue = myItem.Fields["ItemUrl"];
                    var linkUrl = linkvalue.TargetID;
                    linkUrl1 = linkUrl.ToString();
                   
                   
                   
                }
                if (SourceDDL.SelectedItem.Text == "NewsAdvertisement")
                {
                    Item myItem = master.GetItem("/sitecore/content/Transmission/Global/Admin/CreateSubItems/NewsAdvertisements");
                    Sitecore.Data.Fields.LinkField linkvalue = myItem.Fields["ItemUrl"];
                    var linkUrl = linkvalue.TargetID;
                    linkUrl1 = linkUrl.ToString();
                   
                }
                //if (SourceDDL.SelectedItem.Text == "OtherDownloads")
                //{
                //    Item myItem = master.GetItem("/sitecore/content/Ports/Global/Admin/CreateSubItems/OtherDownloads");
                //    Sitecore.Data.Fields.LinkField linkvalue = myItem.Fields["ItemUrl"];
                //    var linkUrl = linkvalue.TargetID;
                //    linkUrl1 = linkUrl.ToString();
                   
                //}

                CreateItem(path,linkUrl1);
                DocumentTitle.Text = "";
                SourceDDL.SelectedIndex = 0;
                lblErroMsg.Visible = true;
                lblErroMsg.Text = "File Upload Sucessfully.";
            }
            catch (System.Exception ex)
            {
                // The update failed, write a message to the log
                Sitecore.Diagnostics.Log.Error(ex.Message, this);
                lblErroMsg.Text = "File Upload Failed : " + ex.Message;
            }

        }

        private void PublishItem(Item item)
        {
            try { 

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
            catch (System.Exception ex)
            {
                // The update failed, write a message to the log
                Sitecore.Diagnostics.Log.Error(ex.Message, this);
                lblErroMsg.Text = "File Upload Failed : " + ex.Message;
            }
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



        protected void CreateItem(string Path, string ItemUrl)
        {
           // UploadButton_Click();
            Sitecore.Data.Database masterDB = Sitecore.Configuration.Factory.GetDatabase("master");
            //  Item parentItem = Sitecore.Context.Item;
            string ItemUrl1 = ItemUrl;
           
            Item parentItem = masterDB.GetItem(new ID(ItemUrl1));
            string name = "TestComment_" + Sitecore.DateUtil
                .IsoNow;
            //TemplateItem template = Sitecore.Context.Database.GetItem(new ID("{E5934ED6-8D4F-4FEB-AA1A-AA06C783195F}"));
            TemplateItem template = masterDB.GetTemplate(new ID("{3281C22F-75FD-49DF-BBB5-5737B4F4F64F}"));
           // TemplateItem template = masterDB.GetTemplate("/sitecore/templates/Project/Ports/Page Types/News");
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                // Item newItem;
                String ItemName= DocumentTitle.Text;
                Item newItem = parentItem.Add(ItemName, template);
             
                try
                {
                    if (newItem != null)
                    {
                        newItem.Editing.BeginEdit();
                        newItem.Fields["Title"].Value = DocumentTitle.Text;
                       

                        var linkField = new LinkField(newItem.Fields["LinkUrl"]);
                        // Link link1 = new Link();
                        // link1. = global::Sitecore.Resources.Media.MediaManager.GetMediaUrl(MediaItem);
                        // link.GetType Type = LinkType.Media;

                        linkField.Url = Path.ToString();
                        Item MediaItem = masterDB.GetItem(linkField.Url);
                        global::Sitecore.Data.Items.MediaItem media = new global::Sitecore.Data.Items.MediaItem(linkField.TargetItem);
                        linkField.Url = global::Sitecore.Resources.Media.MediaManager.GetMediaUrl(media);
                        //   linkField.Url = Path.Replace("/sitecore/media library", "").ToString();
                        string linkType = "media";
                        linkField.LinkType = linkType;
                       
                        linkField.TargetID = MediaItem.ID;
                        // string url = Sitecore.Resources.Media.MediaManager.GetMediaUrl(newItem);
                        //string url = String.Empty;
                        //Sitecore.Data.Items.MediaItem media = new Sitecore.Data.Items.MediaItem(MediaItem);
                        //url = Sitecore.StringUtil.EnsurePrefix('/',
                        //Sitecore.Resources.Media.MediaManager.GetMediaUrl(media));
                        //newItem.Fields["Type"].Value = DropDownList1.SelectedItem.Value.ToString();
                        // Sitecore.Data.ID SourceId = new Sitecore.Data.ID(DropDownList1.SelectedItem.Value.ToString());
                        System.DateTime CurrentDate = System.DateTime.Parse(txtDate.Text);
                        newItem.Fields["Date"].Value = Sitecore.DateUtil.ToIsoDate(CurrentDate);
                      
                        newItem.Fields["isVisible"].Value = "1";
                       
                        newItem.Editing.EndEdit();


                        PublishItem(newItem);
                    }
                   
                }
                catch
                {
                    newItem.Editing.CancelEdit();
                }
            }

        }



    }
}