<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadData.aspx.cs" Inherits="Sitecore.Marathon.Website.sitecore.admin.Marathon.DownloadData" %>

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
         <div class="container">
             <div class="row">
                 <h2 class="txt-center"><b>Select a date range to export the data </b></h2>
                 <form id="form2" runat="server">
                    <div class="row">
                        <%--From--%>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="d-block">From</label>
                                <asp:TextBox ID="TextBoxFrom" runat="server" TextMode="Date" class="form-control input w-100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="validateFrom" runat="server" ControlToValidate="TextBoxFrom" ErrorMessage="(From) Date is Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <%--To--%>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label class="d-block">To</label>
                                <asp:TextBox ID="TextBoxTo" runat="server" TextMode="Date" class="form-control input w-100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="validateTo" runat="server" ControlToValidate="TextBoxTo" ErrorMessage="(To) Date is Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
							    <label class="d-block">Payment Status</label>
                                <asp:DropDownList ID="PaymentStatus" runat="server" class="form-control">
                                      <asp:ListItem Value="">Payment Status</asp:ListItem>
                                      <asp:ListItem  Value="pending">Pending</asp:ListItem>
                                      <asp:ListItem  Value="complementary">Complementary</asp:ListItem>
                                      <asp:ListItem  Value="successful">Successful</asp:ListItem>
                                      <asp:ListItem  Value="no">No</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="row txt-center">
                        <p class="txt-center w-100 d-block">
                            <strong class="txt-red">
                                <asp:Label ID="lblErroMsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" Visible="false"></asp:Label></strong>
                        </p>
                    </div>

                     <%--Download Button--%>
                    <div class="row">
                        <p class="txt-center w-100 d-block">
                            <asp:Button ID="DownloadData" runat="server" Text="Download Excel" OnClick="DownloadDataClick" class="btn btn-primary" style="width: 366px;"/>
                        </p>
                    </div>				 
               </form>
            </div>
         </div>
      </div>
	</body>
</html>
