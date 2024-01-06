<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthenticatedUserLog.aspx.cs" Inherits="Sitecore.AdaniCare.Website.sitecore.admin.AdaniCare.AuthenticatedUserLog" %>

<!DOCTYPE html>

<html lang="en">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">
    <link rel="icon" type="image/png" href="images/favicon.png" />

    <title>Adani Cares</title>
    <!-- Bootstrap core CSS -->

    <link href="/styles/Electricity/bootstrap.min.css" rel="stylesheet" />
    <link href="/styles/Electricity/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="/styles/Electricity/fontawesome-all.css" rel="stylesheet" />
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
                         <div class="col-md-6">
                            <img src="https://www.adanicares.com/images/AdaniCare/logo.png" class="img-fluid" alt="Adani Electricity">
                        </div>
                        <div class="col-md-6 text-md-right">
                            <h4 class="font-weight-bold mt-2"><span class="txt-orange">Download Authenticated Users Details</span></h4>
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
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Payment From</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="txtFromDate" runat="server" class="form-control" AutoPostBack="True" TextMode="Date"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Payment To</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="txtToDate" runat="server" class="form-control" AutoPostBack="True" TextMode="Date"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <div>
                                                <label for="" class="col-sm-4 col-md-6 col-form-label">&nbsp;</label>
                                                <div class="col-sm-12 col-lg-12">
                                                    <asp:Button ID="btnDownload" runat="server" Text="Download" class="btn btn-primary float-right" OnClick="btnDownload_Click" />
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
