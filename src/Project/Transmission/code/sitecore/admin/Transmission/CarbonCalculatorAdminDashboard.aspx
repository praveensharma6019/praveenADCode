<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarbonCalculatorAdminDashboard.aspx.cs" Inherits="Sitecore.Transmission.Website.sitecore.admin.Transmission.TransmissionCarbonCalculator" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }

        .vertical-middle {
            border: 1px solid #f5f5f5;
            box-shadow: 0px 0px 11px -6px #000;
            padding: 25px;
            left: 50%;
            top: 50%;
            -webkit-transform: translate(-50%, -50%);
            transform: translate(-50%, -50%);
            position: absolute !important;
        }

        .txt-center {
            text-align: center;
        }

        .btn-primary {
            background: #2768b3;
            color: #fff;
            border: 0px;
            padding: 5px 10px;
            transition: ease all 0.3s;
        }

        .input {
            border: 1px solid #e2e2e2;
            background: #efefef;
            padding: 3px 5px;
        }

        .btn-primary:hover {
            background: #0653a7;
            transition: ease all 0.3s;
        }

        .txt-red {
            color: red;
        }
    </style>


</head>
<body>

    <div class="container">
        <form id="form3" runat="server">            
            <div class="row">
                <div class="col-xs-12 userchart_">
				<div class="text-center">
                    <img class="img-fluid mt-4" style="margin-bottom: 20px;" src="https://www.adanitransmission.com/%2F-%2Fmedia%2F41F1F040918C46E3BCACE28BD97FA71C.ashx" alt="Adani Transmission logo" width="200" height="90"/>
					</div>
                    <ul>
                        <li>
                             <div>
                                <h3>
                                    <asp:label id="totalRegistration" runat="server" text=""></asp:label>
                                </h3>
                                <p>No. of Persons Registered for Calculated Carbon footprint</p>
                            </div>                          
                           
                        </li>
                         <li>
                            
                            <div>
                                <h3>
                                    <asp:label id="FootprintCount" runat="server" text=""></asp:label>
                                </h3>
                                <p>No. of Carbon Footprint Calculation</p>
                            </div>
                           
                        </li>
                        <li>
                            <div>
                                <h3>
                                    <asp:label id="totalRegisteredEmission" runat="server" text=""></asp:label>
                               </h3>
                                <p>Total Carbon Emission of registered persons per month (in Kg)</p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <h3>
                                    <asp:label id="avgRegisteredEmission" runat="server" text=""></asp:label>
                                </h3>
                                <p>Average carbon emission of registered persons per month (in Kg)</p>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
             <hr>
            <div style="clear: both; padding: 50px 0 0 0;">
                <div class="row">
                    <div class="col-sm-6 col-md-4">
                        Filter By

                        <asp:dropdownlist id="selectCompany" runat="server" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
                            <asp:ListItem>Select your Company</asp:ListItem>
                            <asp:ListItem Value="Adani Transmission Limited">Adani Transmission Limited</asp:ListItem>
                            <asp:ListItem Value="Adani Power Limited">Adani Power Limited</asp:ListItem>
                            <asp:ListItem Value="Adani Electricity Mumbai Limited">Adani Electricity Mumbai Limited</asp:ListItem>
                            <asp:ListItem Value="Others">Others</asp:ListItem>
                           
                        </asp:dropdownlist>

                    </div>                    

                    <div class="col-sm-6 col-md-4">
                        From
                        <asp:textbox id="dateFilterFrom" runat="server" textmode="Date" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
                        </asp:textbox>
                    </div>
                    <div class="col-sm-6 col-md-4">
                        To
                        <asp:textbox id="dateFilterTo" runat="server" textmode="Date" placeholder="--Select Date (To)--" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
                        </asp:textbox>
                    </div>
                    
                </div>
                <div class="row multi-step" style="padding-top: 10px">


                    <div class="col-sm-2 col-xs-4">
                        <asp:button id="downloadCSVBtn" runat="server" cssclass="btn initiatives-btn2" text="Download" onclick="downloadCSV_Click"/>
                    </div>
                    <div class="col-sm-2 col-xs-4 pull-right">
                        <asp:button id="resetBtn" runat="server" cssclass="btn initiatives-btn2" text="Reset" onclick="resetForm" />
                    </div>
                    <div class="col-sm-2 col-xs-4 pull-right">
                        <asp:button id="searchBtn" runat="server" cssclass="btn initiatives-btn2" text="Search" style="height: 40px; width: 100%;" onclick="searchBtn_Click"/>

                    </div>
                    <p>
                        <strong class="txt-red">
                            <asp:label id="lblErroMsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" visible="false"></asp:label>
                        </strong>
                    </p>

                </div>
                <div class="mt-100 style="display: none">
                    <asp:placeholder id="gridrecord1" runat="server"></asp:placeholder>
                </div>



            </div>
            <div class="row" style="display: none">
                <div class="col-4 mx-auto vertical-middle">

                    <p class="txt-center">
                        <img src="../../../images/Marathon/Marathonlogo.png" alt="Belvedere Club Ahmedabad" disablewebedit="False" />
                    </p>
                    <p class="txt-center"><b>Select the Form Type and date range to export the data </b></p>



                    <div class="row">
                        <div class="col-sm-6 w-100">
                            <div class="form-group">
                                <label class="d-block">From</label>
                                <asp:textbox id="TextBoxFrom" runat="server" textmode="Date" class="input"></asp:textbox>
                                <%--<asp:RequiredFieldValidator ID="validateFrom" runat="server" ControlToValidate="TextBoxFrom" ErrorMessage="(From) Date is Required"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="col-sm-6 w-100">
                            <div class="form-group">
                                <label class="d-block">To</label>
                                <asp:textbox id="TextBoxTo" runat="server" textmode="Date" class="input"></asp:textbox>
                                <%--<asp:RequiredFieldValidator ID="validateTo" runat="server" ControlToValidate="TextBoxTo" ErrorMessage="(To) Date is Required"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                    </div>
                    <div class="row txt-center">
                        <p class="txt-center w-100 d-block">
                            <strong class="txt-red">
                                <asp:label id="lasd" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" visible="false"></asp:label>
                            </strong>
                        </p>
                    </div>
                    <div class="row">
                        <p class="txt-center w-100 d-block">
                            <asp:button id="Button1" runat="server" text="Download" onclick="Button1_Click" class="btn btn-primary" />
                        </p>
                    </div>



                </div>
            </div>
            <div class="row" style="display: none">
                <p class="txt-center w-100 d-block">
                    <asp:gridview id="hiddenGrid1" runat="server"></asp:gridview>
                </p>
            </div>
            <%--<asp:hiddenfield id="getyear" runat="server" value="2019" />--%>
        </form>
    </div>




    <!-- Bootstrap core CSS -->
    <link href="/styles/Marathon/bootstrap.min.css" rel="stylesheet" />
    <link href="/styles/Marathon/font-awesome.min.css" rel="stylesheet" />
    <link href="/styles/Marathon/owl.carousel.min.css" rel="stylesheet" />
    <link href="/styles/Marathon/style.css" rel="stylesheet" />
   
    <link href="/styles/Marathon/stylesaead.css" rel="stylesheet" />
    <link href="/styles/Marathon/register.css" rel="stylesheet" />

    <!--Export table button CSS-->

    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.4/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.3.6/css/buttons.dataTables.min.css" />

    <script src="/scripts/marathon/jquery-3.2.1.min.js"></script>

    <script src="/scripts/Marathon/bootstrap.min.js"></script>
    <script src="/scripts/Marathon/owl.carousel.min.js"></script>
    <script src="/scripts/Marathon/main.js"></script>
    <script src="/scripts/Transmission/main.js"></script>
    <script src="/scripts/Marathon/jquery.easypiechart.min.js"></script>
    <script src="/scripts/Marathon/home.js"></script>
    <script src="/scripts/Marathon/modernizr-1.6.min.js"></script>
    <script src="/scripts/Marathon/register.js"></script>
    <!--Data Table-->
    <script type="text/javascript" src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/2.3.6/js/dataTables.buttons.min.js"></script>
    
</body>
</html>
