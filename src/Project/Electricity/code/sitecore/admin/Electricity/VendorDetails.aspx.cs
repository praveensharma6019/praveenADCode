using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Electricity.Website.sitecore.admin.Electricity
{
    public partial class VendorDetails : System.Web.UI.Page
    {
        TenderDataContext dbcontext = new TenderDataContext();
        
//       // string strConnString = ConfigurationManager.ConnectionStrings["ElectricityTenderConnectionString"].ConnectionString;

//             "Data Source=servername;" +
//"Initial Catalog=databasename;User id=uasername;Password=password;";
//SqlConnection connection;
//        SqlCommand com;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblErroMsg.Visible = false;
            this.lblSuccessMsg.Visible = false;
           
            if (!IsPostBack)
            {
                // State_ddl.Items.Clear();
                var State = from s in dbcontext.States select new { s.ID, s.StateName };



                State_ddl.DataSource = State.ToList();

                State_ddl.Enabled = true;

                State_ddl.DataValueField = "Id";

                State_ddl.DataTextField = "StateName";

                State_ddl.DataBind();

                //  State_ddl.Items.Insert(0, "--Select--");



                //var state = from s in dbcontext.SolarApplicationVendorLists where s.Address.Equals(Address) select new { s.Id, s.Address };
                //District_ddl.Items.Clear();
                var District = from d in dbcontext.Districts select new { d.ID, d.DistrictName };



                District_ddl.DataSource = District.ToList();

                District_ddl.Enabled = true;

                District_ddl.DataValueField = "Id";

                District_ddl.DataTextField = "DistrictName";

                District_ddl.DataBind();

                //  District_ddl.Items.Insert(0, "--Select--");
            }





        }

        protected void btn_insert_Click(object sender, EventArgs e)
        {
            try
            { 
            SolarApplicationVendorList entity = new SolarApplicationVendorList();
            {
                    //Dim checkBox = TryCast(row.FindControl("myCheckBox"), CheckBox);
                    if (myCheckBox.Checked == true)
                    { entity.IsActive = true; }
                    else { entity.IsActive = false; }
                  //  entity.IsActive = myCheckBox.Checked;
                entity.Id = Guid.NewGuid();
                entity.Address = Address.Text;
               // entity.NameOfVendorAgency = Name_of_Agency.Text;
               entity.NameOfVendorAgency = Name_of_Agency.Text;
                entity.ContactName = Contact_Name.Text;
                entity.EmailAddress = Email.Text;
                entity.MobileNumber = Mobile.Text;
               entity.PublicMobileNumber = Public_Mobile.Text;
                entity.STDLandlineNumber = STD_Landline_Phone.Text;
                entity.STDFaxNumber = STD_Fax.Text;
                 entity.WebsiteAddress = Website.Text;
                 entity.PANNumber = PAN.Text;
                    entity.State = State_ddl.Text;
                    entity.District = District_ddl.Text;
                    entity.RatingAgency = Rating_Agency_Ddl.Text;
                entity.VendorCode = Vendor_Code.Text;
            };
            dbcontext.SolarApplicationVendorLists.InsertOnSubmit(entity);
                dbcontext.SubmitChanges();
                lblSuccessMsg.Visible = true;
            lblSuccessMsg.Text = "Data entered successfully!!!";
            }
            catch (Exception ex)
            {
                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                Diagnostics.Log.Error("Admin Payment History data - Error  " + ex.Message, this);
            }
        }


        protected void btn_District_Change(object sender, EventArgs e)
        {
            var StateID = State_ddl.SelectedItem.Value;
           // District_ddl.Items.Clear();
            var List = from d in dbcontext.Districts where d.StateID.Equals(StateID) select new { d.ID, d.DistrictName };

            

            District_ddl.DataSource = List.ToList();

            District_ddl.Enabled = true;

            District_ddl.DataValueField = "Id";

            District_ddl.DataTextField = "DistrictName";

            District_ddl.DataBind();

          //  District_ddl.Items.Insert(0, "--Select--");

        }





        }
}