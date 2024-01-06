<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VendorDetails.aspx.cs" Inherits="Sitecore.Electricity.Website.sitecore.admin.Electricity.VendorDetails" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="icon" type="image/png" href="images/favicon.png" />

    <title>Adani Electricity</title>

    <!-- Bootstrap core CSS -->

    <link href="/styles/Electricity/bootstrap.min.css" rel="stylesheet" />
    <link href="/styles/Electricity/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="/styles/Electricity/fontawesome-all.css" rel="stylesheet" />
   <%-- <link href="/styles/Electricity/owl.carousel.css" rel="stylesheet" />
    <link href="/styles/Electricity/owl.theme.default.css" rel="stylesheet" />--%>
    <link href="/styles/Electricity/adani-electricity.css" rel="stylesheet" />
    <link href="/styles/Electricity/adani-electricity-responsive.css" rel="stylesheet" />

</head>
<body>

      <!-- Header -->
    <header class="headerSec">
    </header>
    <!-- /Header -->
    <!--  Main content Section-->
    <main role="main">
        <div class="overlay"></div>
        <div class="contentSection account">

            <!-- Page Content -->
            <div class="pageContent">
                <div class="container">
                    <!-- Welcome Message-->
                    <div class="row">
                        <div class="col-12">
                            <br>
                            <h4 class="float-left"><span class="txt-orange">Vendor Details</span></h4>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                    <!-- /Welcome Message -->
                    <div class="py-2"></div>
                    <!-- Section -->
                    <div class="row">
                        <!-- Left Panel -->

                        <!-- /Left Panel -->
                        <!-- Main Content Panel -->

                        <div class="col-md-12 col-lg-12" id="mainContent">
                            <!-- Toggle button to show/hide side nav -->
                            <div class="d-block d-md-none sideNavBtn">
                                <button type="button" id="sidebarCollapse" class="btn btn-secondary">
                                    <i class="fas fa-align-left"></i>
                                    <span>&nbsp;Menu</span>
                                </button>
                            </div>
                            <!-- /Toggle button to show/hide side nav -->
                            <div class="clearfix"></div>
                            <!-- Update Profile Page Content-->
                            <div class="panel">
                                <form id="form1" class="pt-3" runat="server">
                                     <div class="form-group row">
                                        <div class="col-sm-12">
                                            <strong>
                                                <asp:Label ID="lblSuccessMsg" runat="server"  class="txt-orange col-sm-5 col-md-5 col-form-label" Visible="false" Text="Records downloaded Successfully"></asp:Label></strong>
                                            <strong>
                                                <asp:Label ID="lblErroMsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" Visible="false"></asp:Label></strong>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Name of Agency</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="Name_of_Agency" class="form-control" runat="server" MaxLength="60"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Address</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="Address" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
             
              <div class="form-group row">
                                        
                                           <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">State</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:DropDownList ID="State_ddl" runat="server" AppendDataBoundItems="True" ></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                          <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">District</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                   <asp:DropDownList ID="District_ddl" runat="server"  AppendDataBoundItems="True"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
             
             <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Contact Name</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="Contact_Name" class="form-control" runat="server" MaxLength="50"></asp:TextBox>
                                                 </div>
                                            </div>
                                        </div>
                                      <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Email</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="Email" class="form-control" runat="server" MaxLength="60"></asp:TextBox>
                                                      
                                                </div>
                                            </div>
                                        </div>
                                    </div>
            
              <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Mobile</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="Mobile" class="form-control" runat="server" MaxLength="10"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
     ControlToValidate="Mobile" runat="server"
    ErrorMessage="Only Numbers allowed"
    ValidationExpression="\d+">
</asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                  <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Public Mobile</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="Public_Mobile" class="form-control" runat="server" MaxLength="10"></asp:TextBox>
                                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
    ControlToValidate="Public_Mobile" runat="server"
    ErrorMessage="Only Numbers allowed"
    ValidationExpression="\d+">
</asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
             
            <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">STD Landline Phone</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="STD_Landline_Phone" class="form-control" runat="server" MaxLength="15"></asp:TextBox>
                                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator3"
    ControlToValidate="STD_Landline_Phone" runat="server"
    ErrorMessage="Only Numbers allowed"
    ValidationExpression="\d+">
</asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                    <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">STD FAX</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="STD_Fax" class="form-control" runat="server" MaxLength="15"></asp:TextBox>
                                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator4"
    ControlToValidate="STD_Fax" runat="server"
    ErrorMessage="Only Numbers allowed"
    ValidationExpression="\d+">
</asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
          
              <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Website</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="Website" class="form-control" runat="server" MaxLength="35"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                    <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">PAN</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="PAN" class="form-control" runat="server" MaxLength="10"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
            

             <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Rating Agency</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
            <asp:DropDownList id="Rating_Agency_Ddl" runat="server"  AppendDataBoundItems="True" >
                 <asp:ListItem Text="Brickwork Rating India Pvt. Ltd." Value="BrickworkRatingInd" />
                 <asp:ListItem Text="Credit Analysis & Research Ltd. (CARE)" Value="CreditAnalysis" />
                 <asp:ListItem Text="CRISIL" Value="CRISIL" />
                 <asp:ListItem Text="Fitch Rating India Private Ltd." Value="FitchRatingIndia" />
                 <asp:ListItem Text="ICRA Limited" Value="ICRALimited" />
                 <asp:ListItem Text="Onicare" Value="Onicare" />
                <asp:ListItem Text="SME Rating Agency of India Ltd. (SMERA)" Value="SMERatingAgency" />
</asp:DropDownList>  
   </div>
                                            </div>
                                        </div>
                   <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Vendor Code</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="Vendor_Code" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                   <div class="form-group row">
                                         <div class="col-sm-6">
                                            <div>
                                                <label for="" class="col-sm-4 col-md-6 col-form-label">&nbsp;</label>
                                                <div class="col-sm-12 col-lg-12">
                                                   <asp:CheckBox ID="myCheckBox" runat="server" Text="IsActive"/>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <div>
                                                <label for="" class="col-sm-4 col-md-6 col-form-label">&nbsp;</label>
                                                <div class="col-sm-12 col-lg-12">
                                                   <asp:Button ID="btnUploadFile" runat="server" Text="Upload"  OnClick="btn_insert_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

          


             

                                </form>
                            </div>
                        </div>
                        <!-- /Main Content Panel -->
                    </div>
                    <!-- /Section -->


                </div>
            </div>
            <!-- /Page Content -->
        </div>

    </main>
    <!--  /Main content Section-->
    <!-- Footer -->
    <footer class="">
    </footer>
    <!-- /Footer -->

    <%--    <!-- Bootstrap core JavaScript
    ================================================== -->

    <script src="js/jquery-slim.min.js"></script>
    <script src="js/popper.min.js"></script>
    <script src="js/bootstrap.min.js"></script>

    <!-- Owl Carousel -->
    <script src="js/owl.carousel.min.js"></script>

    <!-- Custom JavaScript for Adani Electricity
    ================================================== -->
    <script src="js/adani-ele-custom.js"></script>--%>
</body>
</html>
