<%@ page language="C#" autoeventwireup="true" codebehind="AdminDashboard.aspx.cs" inherits="Sitecore.Marathon.Website.sitecore.admin.Marathon.AdminDashboard" %>

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
            <div class="col-sm-6 col-md-3">
                Marathon Year

                        <asp:dropdownlist id="getyear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Page_Load" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
                            <asp:ListItem>Select Marathon Year</asp:ListItem>
                            <asp:ListItem Value="2019" selected>2019</asp:ListItem>
                            <asp:ListItem Value="2020">2020</asp:ListItem>
                            <asp:ListItem Value="2021">2021</asp:ListItem>
                            <asp:ListItem Value="2022">2022</asp:ListItem>
                        </asp:dropdownlist>

            </div>
            <div class="row">
                <div class="col-xs-12 userchart_">

                    <ul>
                        <li>
                            <div>


                                <i>
                                    <img src="https://www.ahmedabadmarathon.com/wp-content/themes/custom/images/icons8-running-50.png" width="35" height="35" alt="Run"></i>

                                <h3><span>
                                    <asp:label id="todaysTotalRegis" runat="server" text=""></asp:label>
                                </span>Today's Registraions</h3>
                                <p>
                                    Paid -
                                    <asp:label id="todaysPaidRegis" runat="server" text=""></asp:label>
                                </p>
                                <p>
                                    Outstanding -
                                    <asp:label id="todaysOutstandingRegis" runat="server" text=""></asp:label>
                                </p>
                                <p>
                                    Complimentary -
                                    <asp:label id="todaysComplimentaryRegis" runat="server" text=""></asp:label>
                                </p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <i class="">&#8377</i>
                                <h3><span>
                                    <asp:label id="todaysTotalCollection" runat="server" text=""></asp:label>
                                </span>Today's Collections</h3>
                                <p>
                                    Paid -
                                    <asp:label id="todaysPaidCollection" runat="server" text=""></asp:label>
                                </p>
                                <p>
                                    Outstanding -
                                    <asp:label id="todaysOutstandingCollection" runat="server" text=""></asp:label>
                                </p>
                                <p>
                                    Complimentary -
                                    <asp:label id="todaysComplimentaryCollection" runat="server" text=""></asp:label>
                                </p>
                            </div>
                        </li>
                        <li>
                            <div>

                                <i>
                                    <img src="https://www.ahmedabadmarathon.com/wp-content/themes/custom/images/icons8-running-50.png" width="35" height="35" alt="Run"></i>
                                <h3><span>
                                    <asp:label id="totalRegistration" runat="server" text=""></asp:label>
                                </span>Total Registraions</h3>
                                <p>
                                    Paid -
                                    <asp:label id="totalPaidRegistration" runat="server" text=""></asp:label>
                                </p>
                                <p>
                                    Outstanding -
                                    <asp:label id="totalOutstandingRegistration" runat="server" text=""></asp:label>
                                </p>
                                <p>
                                    Complimentary -
                                    <asp:label id="totalComplementaryRegistration" runat="server" text=""></asp:label>
                                </p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <i class="">&#8377</i>
                                <h3><span>
                                    <asp:label id="totalCollection" runat="server" text=""></asp:label>
                                </span>Total Collection</h3>
                                <p>
                                    Paid -
                                    <asp:label id="totalPaidCollection" runat="server" text=""></asp:label>
                                </p>
                                <p>
                                    Outstanding -
                                    <asp:label id="totalOutstandingCollection" runat="server" text=""></asp:label>
                                </p>
                                <p>
                                    Complimentary -
                                    <asp:label id="totalComplementaryCollection" runat="server" text=""></asp:label>
                                </p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <i class="">&#8377</i>
                                <h3><span>
                                    <asp:label id="TotalCoupon" runat="server" text=""></asp:label>
                                </span>Coupon Collection</h3>
                                <p><a href="/sitecore/admin/Marathon/active-coupon.aspx">Click here for details</a></p>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
                <div class="row " style="background-color: #f1f1f1;padding: 30px 0px 30px 0px;">
				<h2> Donation </h2>
                <div class="col-xs-12 userchart_">

                    <ul>
                        <li>
                            <div id="TotalEmailIdDIV">


                                <i>
                                    <img src="https://www.ahmedabadmarathon.com/wp-content/themes/custom/images/icons8-running-50.png" width="35" height="35" alt="Run"></i>

                                <h3><span>
                                    <asp:label id="TotalEmailId" runat="server" text=""></asp:label>
                                </span>Total Unique Email Id's</h3>
                               
                            </div>
                        </li>

                        <li>
                            <div id="TotalAmountRaisedDIV">
                                <i class="">&#8377</i>
                                <h3><span>
                                    <asp:label id="TotalAmountRaised" runat="server" text=""></asp:label>
                                </span>Total Amount Raised</h3>
                               
                            </div>
                        </li>
                        <li>
                            <div id="TotalNumberRunnersDIV">

                                <i>
                                    <img src="https://www.ahmedabadmarathon.com/wp-content/themes/custom/images/icons8-running-50.png" width="35" height="35" alt="Run"></i>
                                <h3><span>
                                    <asp:label id="TotalNumberRunners" runat="server" text=""></asp:label>
                                </span>Total Numbers Of Runners</h3>
                               
                            </div>
                        </li>
                         <li>
                            <div id="TotalAmountRaisedByRunnersDIV">


                                 <i class="">&#8377</i>
                                <h3><span>
                                    <asp:label id="TotalAmountRaisedByRunners" runat="server" text=""></asp:label>
                                </span>Total Amount Raised By Runners</h3>
                               
                            </div>
                        </li>
                       
                    </ul>
                </div>
           
         
             
            			
          
             
           
            
             </div>
            <div class="row mt-100">
                <div class="col-sm-12 col-md-5 race_table">
                    <h3>Race Category</h3>
                    <div class="size_table_main">
                        <asp:placeholder id="raceCategory" runat="server"></asp:placeholder>

                    </div>
                </div>
                <div class="col-sm-12 col-md-7 size_table">
                    <h3>Gender Wise Classification</h3>
                    <div class="size_table_main">
                        <asp:placeholder id="genderWiseTable1" runat="server"></asp:placeholder>
                    </div>

                </div>
            </div>

            <div class="row mt-100">
                <div class="col-sm-12 col-md-5 race_table">
                    <h3>Tshirt Size For (5 Km) Race</h3>
                    <div class="size_table_main">
                        <asp:placeholder id="TshirtSize5Km1" runat="server"></asp:placeholder>

                    </div>
                </div>
                <div class="col-sm-12 col-md-7 size_table">
                    <h3>Tshirt Size For (Remaining) Race</h3>
                    <div class="size_table_main">
                        <asp:placeholder id="TshirtSizeRemainingRace" runat="server"></asp:placeholder>
                    </div>

                </div>
            </div>
            <div style="clear: both; padding: 50px 0 0 0;">
                <div class="row">
                    <div class="col-sm-6 col-md-3">
                        Payment Status

                        <asp:dropdownlist id="selectStatus" runat="server" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
<asp:ListItem>Select Payment Status</asp:ListItem>
                            <asp:ListItem Value="complementary">Complimentary</asp:ListItem>
                            <asp:ListItem Value="no">Outstanding</asp:ListItem>
                            <asp:ListItem Value="successful">Paid</asp:ListItem>
                            <asp:ListItem Value="pending">Pending</asp:ListItem>

                        </asp:dropdownlist>

                    </div>


                    <div class="col-sm-6 col-md-3">
                        Reference Code
                        <asp:dropdownlist id="selectCouponCode" runat="server" appenddatabounditems="true" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">

                            <asp:ListItem>--Select Reference Code Status--</asp:ListItem>
                        </asp:dropdownlist>
                    </div>

                    <div class="col-sm-6 col-md-3">
                        From
                        <asp:textbox id="DdlDateFrom" runat="server" textmode="Date" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
                        </asp:textbox>
                    </div>
                    <div class="col-sm-6 col-md-3">
                        To
                        <asp:textbox id="DdlDateTo" runat="server" textmode="Date" placeholder="--Select Date (To)--" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
                        </asp:textbox>
                    </div>

                    <%--   <div class="col-sm-6 col-md-6">
                        <div id="reportrange" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%">
                            <i class="fa fa-calendar"></i>&nbsp;
                <span>Choose Date</span> <i class="fa fa-caret-down"></i>
                        </div>

                    </div>--%>
                </div>
                <div class="row multi-step" style="padding-top: 10px">


                    <div class="col-sm-2 col-xs-6">
                        <asp:button id="downloadCSV" runat="server" cssclass="btn initiatives-btn2" text="Download" onclick="downloadCSV_Click" />
                    </div>
                    <div class="col-sm-2 col-xs-6 pull-right">
                        <asp:button id="searchBtn" runat="server" cssclass="btn initiatives-btn2" text="Search" style="height: 40px; width: 100%;" onclick="searchBtn_Click" />

                    </div>
                    <p>
                        <strong class="txt-red">
                            <asp:label id="lblErroMsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" visible="false"></asp:label>
                        </strong>
                    </p>

                </div>
                <div class="mt-100">
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

    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.13/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.2.4/css/buttons.dataTables.min.css" />
   
    <script src="/scripts/marathon/jquery-3.2.1.min.js"></script>
    
	
    <script src="/scripts/Marathon/bootstrap.min.js"></script>
    <script src="/scripts/Marathon/owl.carousel.min.js"></script>
    <script src="/scripts/Marathon/main.js"></script>
    <script src="/scripts/Marathon/jquery.easypiechart.min.js"></script>
    <script src="/scripts/Marathon/home.js"></script>
    <script src="/scripts/Marathon/DashBoard.js"></script>
    <script src="/scripts/Marathon/modernizr-1.6.min.js"></script>
    <script src="/scripts/Marathon/register.js"></script>
    <!--Data Table-->
    <script type="text/javascript" src=" https://cdn.datatables.net/1.10.13/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src=" https://cdn.datatables.net/buttons/1.2.4/js/dataTables.buttons.min.js"></script>
   
</body>
</html>

