<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="active-coupon.aspx.cs" Inherits="Sitecore.Marathon.Website.sitecore.admin.Marathon.active_coupon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
       <!-- Bootstrap core CSS -->
   <link href="/styles/Marathon/bootstrap.min.css" rel="stylesheet" />
    <link href="/styles/Marathon/font-awesome.min.css" rel="stylesheet" />
    <link href="/styles/Marathon/owl.carousel.min.css" rel="stylesheet" />
    <link href="/styles/Marathon/style.css" rel="stylesheet" />
    <link href="/styles/Marathon/stylesaead.css" rel="stylesheet" />
    <link href="/styles/Marathon/register.css" rel="stylesheet" />
</head>
<body>
    <section>
<div class="container">
    <form id="form1" runat="server">
        <div>
            <section class="multi-step-form">
                <a href="/sitecore/admin/Marathon/ExportData.aspx" class="btn initiatives-btn2">REGISTRATION DASHBOARD</a>
  <div class="container mt-100">
          <div class="row">
                    <div class="col-sm-12 col-md-12 race_table">
                 <h3>Coupon Details</h3>
  

                <div class="size_table_main">
                    <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>          
              </div>

       </div>
       </div>
 </div></section>
        </div>
         <asp:HiddenField ID="getyear" runat="server" Value="2019" />
    </form>
</div>
    </section>
    
    
    <script src="/scripts/marathon/jquery-3.2.1.min.js"></script>
    
    <script src="/scripts/Marathon/bootstrap.min.js"></script>
    <script src="/scripts/Marathon/owl.carousel.min.js"></script>
    <script src="/scripts/Marathon/main.js"></script>
    <script src="/scripts/Marathon/jquery.easypiechart.min.js"></script>
    <script src="/scripts/Marathon/home.js"></script>
    <script src="/scripts/Marathon/modernizr-1.6.min.js"></script>
    <script src="/scripts/Marathon/register.js"></script>
</body>
</html>
