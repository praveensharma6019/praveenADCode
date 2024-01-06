<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportData.aspx.cs" Inherits="Sitecore.Marathon.Website.sitecore.admin.Marathon.ExportData" %>

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

                    <ul>
                        <li>
                            <div>


                                <i>
                                    <img src="https://www.ahmedabadmarathon.com/wp-content/themes/custom/images/icons8-running-50.png" width="35" height="35" alt="Run"></i>

                                <h3><span>
                                    <asp:Label ID="todaysTotalRegis" runat="server" Text=""></asp:Label></span> Today's Registraions</h3>
                                <p>
                                    Paid -
                                    <asp:Label ID="todaysPaidRegis" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Outstanding -
                                    <asp:Label ID="todaysOutstandingRegis" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Complimentary -
                                    <asp:Label ID="todaysComplimentaryRegis" runat="server" Text=""></asp:Label>
                                </p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <i class="">&#8377</i>
                                <h3><span>
                                    <asp:Label ID="todaysTotalCollection" runat="server" Text=""></asp:Label></span> Today's Collections</h3>
                                <p>
                                    Paid -
                                    <asp:Label ID="todaysPaidCollection" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Outstanding -
                                    <asp:Label ID="todaysOutstandingCollection" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Complimentary -
                                    <asp:Label ID="todaysComplimentaryCollection" runat="server" Text=""></asp:Label>
                                </p>
                            </div>
                        </li>
                        <li>
                            <div>

                                <i>
                                    <img src="https://www.ahmedabadmarathon.com/wp-content/themes/custom/images/icons8-running-50.png" width="35" height="35" alt="Run"></i>
                                <h3><span>
                                    <asp:Label ID="totalRegistration" runat="server" Text=""></asp:Label></span> Total Registraions</h3>
                                <p>
                                    Paid -
                                    <asp:Label ID="totalPaidRegistration" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Outstanding -
                                    <asp:Label ID="totalOutstandingRegistration" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Complimentary -
                                    <asp:Label ID="totalComplementaryRegistration" runat="server" Text=""></asp:Label>
                                </p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <i class="">&#8377</i>
                                <h3><span>
                                    <asp:Label ID="totalCollection" runat="server" Text=""></asp:Label></span> Total Collection</h3>
                                <p>
                                    Paid -
                                    <asp:Label ID="totalPaidCollection" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Outstanding -
                                    <asp:Label ID="totalOutstandingCollection" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Complimentary -
                                    <asp:Label ID="totalComplementaryCollection" runat="server" Text=""></asp:Label>
                                </p>
                            </div>
                        </li>
                        <li>
                            <div>
                                <i class="">&#8377</i>
                                <h3><span>
                                    <asp:Label ID="TotalCoupon" runat="server" Text=""></asp:Label></span> Coupon Collection</h3>
                                <p><a href="/sitecore/admin/Marathon/active-coupon.aspx">Click here for details</a></p>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="row mt-100">
                <div class="col-sm-12 col-md-5 race_table">
                    <h3>Race Category</h3>
                    <div class="size_table_main">
                        <asp:PlaceHolder ID="raceCategory" runat="server"></asp:PlaceHolder>

                    </div>
                </div>
                <div class="col-sm-12 col-md-7 size_table">
                    <h3>Gender Wise Classification</h3>
                    <div class="size_table_main">
                        <asp:PlaceHolder ID="genderWiseTable1" runat="server"></asp:PlaceHolder>
                    </div>

                </div>
            </div>

            <div class="row mt-100">
                <div class="col-sm-12 col-md-5 race_table">
                    <h3>Tshirt Size For (5 Km) Race</h3>
                    <div class="size_table_main">
                        <asp:PlaceHolder ID="TshirtSize5Km1" runat="server"></asp:PlaceHolder>

                    </div>
                </div>
                <div class="col-sm-12 col-md-7 size_table">
                    <h3>Tshirt Size For (Remaining) Race</h3>
                    <div class="size_table_main">
                        <asp:PlaceHolder ID="TshirtSizeRemainingRace" runat="server"></asp:PlaceHolder>
                    </div>

                </div>
            </div>
            <div style="clear: both; padding: 50px 0 0 0;">
                <div class="row">
                    <div class="col-sm-6 col-md-3">
                        Payment Status

                        <asp:DropDownList ID="selectStatus" runat="server" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
<asp:ListItem>Select Payment Status</asp:ListItem>
                            <asp:ListItem Value="complementary">Complimentary</asp:ListItem>
                            <asp:ListItem Value="no">Outstanding</asp:ListItem>
                            <asp:ListItem Value="successful">Paid</asp:ListItem>
                            <asp:ListItem Value="pending">Pending</asp:ListItem>

                        </asp:DropDownList>
                       
                    </div>


                    <div class="col-sm-6 col-md-3">
                        Reference Code
                        <asp:DropDownList ID="selectCouponCode" runat="server" AppendDataBoundItems="true" Style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">

                            <asp:ListItem>--Select Reference Code Status--</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="col-sm-6 col-md-3">
                        From
                        <asp:TextBox ID="DdlDateFrom" runat="server" TextMode="Date" Style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
                        </asp:TextBox>
                    </div>
                    <div class="col-sm-6 col-md-3">
                        To
                        <asp:TextBox ID="DdlDateTo" runat="server" TextMode="Date" placeholder="--Select Date (To)--" Style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
                        </asp:TextBox>
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
                     <%--  <asp:Button ID="downloadCSV" runat="server" CssClass="btn initiatives-btn2" Text="Download" OnClick="downloadCSV_Click" />--%>
                    </div>
                    <div class="col-sm-2 col-xs-6 pull-right">
                       <asp:Button ID="searchBtn" runat="server" CssClass="btn initiatives-btn2" Text="Search" style="height: 40px; width: 100%;" OnClick="searchBtn_Click" />
                       
                    </div>
                    <p>
                        <strong class="txt-red">
                            <asp:Label ID="lblErroMsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" Visible="false"></asp:Label></strong>
                    </p>

                </div>
               <div class="mt-100">
               <asp:PlaceHolder ID="gridrecord1" runat="server"></asp:PlaceHolder>
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
                                <asp:TextBox ID="TextBoxFrom" runat="server" TextMode="Date" class="input"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="validateFrom" runat="server" ControlToValidate="TextBoxFrom" ErrorMessage="(From) Date is Required"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="col-sm-6 w-100">
                            <div class="form-group">
                                <label class="d-block">To</label>
                                <asp:TextBox ID="TextBoxTo" runat="server" TextMode="Date" class="input"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="validateTo" runat="server" ControlToValidate="TextBoxTo" ErrorMessage="(To) Date is Required"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                    </div>
                    <div class="row txt-center">
                        <p class="txt-center w-100 d-block">
                            <strong class="txt-red">
                                <asp:Label ID="lasd" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" Visible="false"></asp:Label></strong>
                        </p>
                    </div>
                    <div class="row">
                        <p class="txt-center w-100 d-block">
                            <asp:Button ID="Button1" runat="server" Text="Download" OnClick="Button1_Click" class="btn btn-primary" />
                        </p>
                    </div>



                </div>
            </div>
                <div class="row" style="display:none">
                        <p class="txt-center w-100 d-block">
                           <asp:GridView ID="hiddenGrid1" runat="server"></asp:GridView>
                            </p>
                    </div>
                  <asp:HiddenField ID="getyear" runat="server" Value="2019" />
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
    <script src="/scripts/Marathon/modernizr-1.6.min.js"></script>
    <script src="/scripts/Marathon/register.js"></script>
     <!--Data Table-->
    <script type="text/javascript"  src=" https://cdn.datatables.net/1.10.13/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript"  src=" https://cdn.datatables.net/buttons/1.2.4/js/dataTables.buttons.min.js"></script>

   

</body>
</html>

