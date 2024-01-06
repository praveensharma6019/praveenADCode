<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeleteUser.aspx.cs" Inherits="Sitecore.Marathon.Website.sitecore.admin.Marathon.DeleteUser" %>

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
	  <div class="body container">
		<form id="form1" runat="server">
			<div>
				<h2 class="txt-center"><b>User Delete Confirmation</b></h2>
				<div class="col-12">
					<asp:placeholder id="UserDataGrid" runat="server"></asp:placeholder>
				 </div>
				<div class="col-12">
                    <h3 >Are you sure you want to delete this user?</h3>
					<asp:Button ID="UserDelete" runat="server" Text="Delete User" OnClick="DeleteUserConfirmation" class="btn btn-danger"/>
					<asp:Button ID="returnToUserUpdate" runat="server" Text="No" OnClick="returnToUserUpdatePage" class="btn btn-light"/>
				</div>
			</div>
		</form>
	</div>

</body>
</html>
	