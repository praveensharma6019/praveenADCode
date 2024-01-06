<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UpdateUserInfo.aspx.cs" Inherits="Sitecore.Marathon.Website.sitecore.admin.Marathon.UpdateUserInfo" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
   <head runat="server">
      <title></title>
            <link href="https://www.ahmedabadmarathon.com/-/media/project/Marathon/Assests/CSS/bootstrap-min" rel="stylesheet" />
      <link href="https://www.ahmedabadmarathon.com/styles/Marathon/font-awesome.min.css" rel="stylesheet" />
      <link href="https://www.ahmedabadmarathon.com/styles/Marathon/fontawesome-all.css" rel="stylesheet" />
      <link href="https://www.ahmedabadmarathon.com/-/media/project/Marathon/Assests/CSS/style" rel="stylesheet" />
      <link href="https://www.ahmedabadmarathon.com/-/media/project/Marathon/Assests/CSS/stylesaead" rel="stylesheet" />
      <link href="https://www.ahmedabadmarathon.com/-/media/project/Marathon/Assests/CSS/register" rel="stylesheet" />
	  <style>
	  form .form-control
	  {
		padding:0px 20px;
		border:1px solid black;
	  }
	  .body{
		margin-top:100px;
	  }
      header nav ul li.menu-item-has-children > a:after, header nav ul li.menu-item-has-children > a:before 
      {
          background:transparent !important;
      }
	  </style>
   </head>
   <body>
      <header>
         <div class="menu-mobile-overlay"></div>
         <div class="header-primary">
            <div class="menu-trigger"><span></span></div>
            <div class="header-top-links">
               <p></p>
               <div class="menu-social-links-container">
               </div>
            </div>
            <nav class="menu-main-menu-container">
               <ul id="menu-main-menu" class="menu">
               <li id="menu-item" class="menu-item menu-item-type-custom menu-item-object-custom menu-item-has-children menu-item">
                  <a href="/sitecore/admin/Marathon/DownloadData.aspx">
                  Download Data
                  </a>
               </li>
               <li id="menu-item" class="menu-item menu-item-type-custom menu-item-object-custom menu-item-has-children menu-item">
                  <a href="/sitecore/admin/Marathon/UpdateParticipate.aspx">
                  Update User Info
                  </a>
               </li>
                </ul>
            </nav>
         </div>
         <div class="header-logo">
            <a href="https://www.ahmedabadmarathon.com/" target="_blank">
            <img alt="" src="/-/media/8C32BA80FFB945DBA6468C03E1609895.ashx" />
            <span class="skew-bg"><span></span></span>
            </a>
         </div>
         <div class="shantigram-logo hidden-sm hidden-xs">
            <a href="#" target="_blank">
            <img alt="" src="/-/media/1A712213A0F84544A90951BB079D2912.ashx" />
            <span class="skew-bg"><span></span></span>
            </a>
         </div>
      </header>
      <div class="body">
         <section class="first-section container">
            <form id="form1" runat="server">
               <div>
                  <h1 class="txt-center"><b>Update Participate</b></h1>
                  <div class="row">
                    <div class="col-sm-6">
						<div class="form-group">
                            <label class="d-block">First Name</label>
							<asp:TextBox ID="FirstName" runat="server" TextMode="SingleLine" class="form-control input w-100"></asp:TextBox>
                        </div>
                    </div>
                      <div class="col-sm-6">
						<div class="form-group">
                            <label class="d-block">Last Name</label>
							<asp:TextBox ID="LastName" runat="server" TextMode="SingleLine" class="form-control input w-100"></asp:TextBox>
                        </div>
                    </div>
                      <div class="col-sm-6">
						<div class="form-group">
                            <label class="d-block">Email</label>
							<asp:TextBox ID="Email" runat="server" TextMode="SingleLine" class="form-control input w-100"></asp:TextBox>
                        </div>
                    </div>
                      <div class="col-sm-6">
						<div class="form-group">
                            <label class="d-block">Gender</label>
                            <asp:DropDownList ID="Gender" runat="server"  class="form-control input w-100">
                              <asp:ListItem>Male</asp:ListItem>
                              <asp:ListItem>Female</asp:ListItem>
                              <asp:ListItem>Transgender</asp:ListItem>
                           </asp:DropDownList>                       
						</div>
                    </div>
					<div class="col-sm-6">
						<div class="form-group">
                            <label class="d-block">Race Category</label>
							<asp:DropDownList ID="RaceCategory" runat="server"  class="form-control input w-100">
                              <asp:ListItem Value="42.195KM">42.195KM</asp:ListItem>
                              <asp:ListItem Value="21.097KM">21.097KM</asp:ListItem>
                              <asp:ListItem Value="10KM">10KM</asp:ListItem>
                              <asp:ListItem Value="5KM">5KM</asp:ListItem>
                           </asp:DropDownList>
                        </div>
                    </div>
                     <div class="col-sm-6">
                        <div class="form-group">
                           <label class="d-block">ContactNumber</label>
                           <asp:TextBox ID="contactNumberId" runat="server" TextMode="SingleLine" class="form-control input w-100"></asp:TextBox>
                        </div>
                     </div>
                     <div class="col-sm-6">
                        <div class="form-group">
                           <label class="d-block">T-Shirt Size</label>
                           <asp:DropDownList ID="TShirtSize" runat="server" class="form-control input w-100">
                              <asp:ListItem Value="32">Kids (32 Inch)</asp:ListItem>
                              <asp:ListItem Value="34">Kids (34 Inch)</asp:ListItem>
                              <asp:ListItem Value="36">X-Small (36 Inch)</asp:ListItem>
                              <asp:ListItem Value="38">Small (38 Inch)</asp:ListItem>
                              <asp:ListItem Value="40">Medium (40 Inch)</asp:ListItem>
                              <asp:ListItem Value="42">Large (42 Inch)</asp:ListItem>
                              <asp:ListItem Value="44">X-Large (44 Inch)</asp:ListItem>
                              <asp:ListItem Value="46">XX-Large (46 Inch)</asp:ListItem>
                           </asp:DropDownList>
                        </div>
                     </div>
                     <div class="col-sm-6">
                        <div class="form-group">
                           <label class="d-block">Payment Status</label>
                           <asp:DropDownList ID="PaymentStatus" runat="server" class="form-control input w-100">
                              <asp:ListItem Value="pending">pending</asp:ListItem>
                              <asp:ListItem Value="complementary">complementary</asp:ListItem>
                              <asp:ListItem Value="successful">successful</asp:ListItem>
                              <asp:ListItem Value="no">no</asp:ListItem>
                           </asp:DropDownList>
                        </div>
                     </div>
                     <div class="col-6">
                     </div>
                  </div>
                  <div class="row" id="confirmationsection" runat="server">
                      <p class="txt-center w-100 d-block">
                            <asp:Button ID="Button1" runat="server" Text="Update" OnClick="UpdateUserInformation" class="btn btn-primary" style="width: 366px;"/>
                          	<asp:Button ID="Button2" runat="server" Text="No" OnClick="returnToUserUpdatePage" class="btn btn-light"/>     
                      </p>
                  </div>
               </div>
            </form>
         </section>
      </div>
   </body>
</html>