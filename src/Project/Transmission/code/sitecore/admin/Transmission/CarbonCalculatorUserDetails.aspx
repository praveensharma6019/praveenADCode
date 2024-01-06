<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarbonCalculatorUserDetails.aspx.cs" Inherits="Sitecore.Transmission.Website.sitecore.admin.Transmission.CarbonCalculatorUserDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <meta http-equiv="Content-type" content="text/html; charset=utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <!-- Bootstrap core CSS -->
    <link href="/styles/Transmission/bootstrap.min.css" rel="stylesheet" />
    <link href="/styles/Transmission/font-awesome.min.css" rel="stylesheet" />
    <link href="/styles/Transmission/owl.carousel.min.css" rel="stylesheet" />
    <link href="https://www.adanitransmission.com/styles/Transmission/adani-Transmission-min.css" rel="stylesheet" />    
    <link href="/styles/Transmission/style.css" rel="stylesheet" />
    <link href="/styles/Transmission/stylesaead.css" rel="stylesheet" />
    <link href="/styles/Transmission/register.css" rel="stylesheet" />
    <style>
        html, body {
            background: #fff;
        }

        .vendor-form {
            padding: 1rem;
        }

            .vendor-form hr {
                border-color: #e2e2e2;
                margin-top: 0px;
                margin-bottom: 1.5rem;
            }

            .vendor-form h1 {
                color: #1378be;
                font-size: 1.75rem;
                margin-bottom: 1rem;
            }

            .vendor-form h3 {
                font-size: 1.15rem;
                margin-bottom: 0.5rem;
            }

                .vendor-form h3 span {
                    font-weight: 600;
                }

            .vendor-form .col-md-6 {
                margin-bottom: 1.5rem;
            }

                .vendor-form .col-md-6 .form-group {
                    margin-bottom: 0rem;
                }

            .vendor-form select:focus, .vendor-form select:active, .vendor-form select:hover {
                outline: 0;
            }

        form textarea.form-control {
            min-height: auto;
        }
    </style>
</head>
<body>


    <section class="vendor-form">
        <div class="container">
            <div class="row">
                <div class="text-center">
                    <img class="img-fluid mt-4 mb-5" style="margin-bottom: 20px;" src="https://www.adanitransmission.com/%2F-%2Fmedia%2F41F1F040918C46E3BCACE28BD97FA71C.ashx" alt="Adani Transmission logo" width="200" height="90" />
                </div>
                <div class="col-sm-2 col-xs-6">
                    <a class="btn initiatives-btn2" href="/sitecore/admin/transmission/CarbonCalculatorAdminDashboard.aspx"><< Back to Admin Dashboard</a>
                </div>
                <div class="col-sm-2 col-xs-6 pull-right">
                    <%--<a id="logout" runat="server" class="btn initiatives-btn2" href="VendorAdminDashboard.aspx">Log Out</a>--%>
                </div>
            </div>
            <div class="panel">

                <form id="userdetails" runat="server">
                    <div>
                        <h1>User Details</h1>
                        <hr>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <h3>Registration No -
                    <asp:Label ID="regNo" runat="server" Text=""></asp:Label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Registration Date -
                    <asp:Label ID="regDate" runat="server" Text=""></asp:Label>
                            </h3>
                        </div>
                       
                        <div class="col-md-6">
                            <h3>Name -
                    <asp:Label ID="fullname" runat="server" Text=""></asp:Label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Company Name -
                    <asp:Label ID="CompanyName1" runat="server" Text=""></asp:Label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Email -
                    <asp:Label ID="EmailId" runat="server" Text=""></asp:Label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Contact No -
                    <asp:Label ID="ContactNo" runat="server" Text=""></asp:Label>
                            </h3>
                        </div>
                        

                       
                        <div id="footprintDetail" runat="server" class="col-md-12 row">
                            <div class="col-md-12">
                                <h1>Carbon Footprint Details</h1> <hr>
                            </div>
                            <div class="col-md-6">
                                <h3>Submitted On -
                    <asp:Label ID="SubmittedOn" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>Month -
                    <asp:Label ID="month" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>Year -
                    <asp:Label ID="year" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>Total Family Member -
                    <asp:Label ID="TotalMember" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>Electricity Consumed at Residence -
                    <asp:Label ID="electricityConsumedResidence" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>

                            <div class="col-md-6">
                                <h3>CNG Used
                    <asp:Label ID="cngUsed" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>LPG Used -
                    <asp:Label ID="LPGUsed" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>Diesel Consumption -
                    <asp:Label ID="dieselConsumption" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>Petrol Consumption -
                    <asp:Label ID="PetrolConsumption" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>CNG Auto-Rikshaw -
                    <asp:Label ID="CNGAutoRikshaw" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>


                            <div class="col-md-6">
                                <h3>Bus Use -
                    <asp:Label ID="BUSUse" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>Train Use -
                    <asp:Label ID="TrainsUse" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                             <div class="col-md-6">
                                <h3>Emission from Domestic Use -
                    <asp:Label ID="EmissionfromDomesticUse" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                             <div class="col-md-6">
                                <h3>Total Carbon Emission -
                    <asp:Label ID="TotalCarbonEmission" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                             <div class="col-md-6">
                                <h3>Emission from Transportation -
                    <asp:Label ID="EmissionfromTransportation" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                              <div class="col-md-6">
                                <h3>Average Emission per Month Per Person-
                    <asp:Label ID="AverageEmissionperMonth" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>Emission from Air Trips -
                    <asp:Label ID="AirTrips" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>Annual Carbon Footprint -
                    <asp:Label ID="AnnualCarbonFootprint" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                             <div class="col-md-6">
                                <h3>Land Needed to Plant Trees -
                    <asp:Label ID="LandNeededtoPlantTrees" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                             <div class="col-md-6">
                                <h3>Number of Trees Needed -
                    <asp:Label ID="NumberofTreesNeeded" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <%-- <div class="col-md-6">
                                <h3>Average Annual Carbon Footprint -
                    <asp:Label ID="AverageAnnualCarbonFootprint" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>--%>
                             
                        </div>

                         <div id="OffsetfootprintDetail" runat="server" class="col-md-12 row">
                            <div class="col-md-12">
                                <h1>Offset Carbon Footprint Details</h1> 
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    Personal Transport -
                                    <asp:Label ID="PersonalTransport" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>

                            <div class="col-md-6">
                                <h3>
                                    Public Transport -
                                    <asp:Label ID="PublicTransport" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    Online Meeting instead of Air Trips -
                                    <asp:Label ID="OnlineMeeting" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    Instead of 1 star Opt for 5 star Appliances -
                                    <asp:Label ID="FiveStarAppliances" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    I Will plant no. of trees -
                                   <asp:Label ID="OffsetNumberofTreesNeeded" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    I Will fund to tree plantation (INR) -
                                   <asp:Label ID="OffsetFundNeededtoPlantTrees" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    Offset Emission from Domestic Use -
                                   <asp:Label ID="OffsetEmissionfromDomesticUse" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    Offset Emission from Transportation -
                                   <asp:Label ID="OffsetEmissionfromTransportation" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    Offset Emission from Air Trips -
                                  <asp:Label ID="OffsetEmissionfromAirTrips" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    Total Offset Carbon Emission -
                                  <asp:Label ID="TotalOffsetCarbonEmission" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                             <div class="col-md-6">
                                <h3>
                                    Annual Carbon Offset Footprint -
                                  <asp:Label ID="OffsetAnnualCarbonFootprint" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    Average Carbon Offset Emission per Month per person -
                                   <asp:Label ID="AverageOffsetEmissionperMonth" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>
                                    Offset Emission from Planting Trees -
                                    <asp:Label ID="EmissionOffsetForTreePlantation" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                              <div class="col-md-6">
                                <h3>
                                    Offset Emission from funding Tree Plantation -
                                    <asp:Label ID="OffsetEmissionforFundingTrees" runat="server" Text=""></asp:Label>
                                </h3>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </section>


    <!--Export table button CSS-->

    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.13/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.2.4/css/buttons.dataTables.min.css" />

   
    <script src="/scripts/Transmission/jquery-3.2.1.min.js"></script>

    <script src="/scripts/Transmission/bootstrap.min.js"></script>
    <script src="/scripts/Transmission/owl.carousel.min.js"></script>
    <script src="/scripts/Transmission/main.js"></script>
    <script src="/scripts/Transmission/jquery.easypiechart.min.js"></script>
    <script src="/scripts/Transmission/home.js"></script>
    <script src="/scripts/Transmission/modernizr-1.6.min.js"></script>
    <script src="/scripts/Transmission/register.js"></script>

    <!--Data Table-->
    <script type="text/javascript" src=" https://cdn.datatables.net/1.10.13/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src=" https://cdn.datatables.net/buttons/1.2.4/js/dataTables.buttons.min.js"></script>
</body>
</html>
