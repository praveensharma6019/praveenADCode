<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BookingPaymentHistory.aspx.cs" Inherits="Sitecore.Realty.Website.sitecore.admin.Realty.BookingPaymentHistory" %>

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

        . .btn-primary:hover {
            background: #0653a7;
            transition: ease all 0.3s;
        }

        .txt-red {
            color: red;
        }
    </style>
    <!-- Bootstrap core CSS -->
    <link href="/styles/Realty/bootstrap.min.css" rel="stylesheet" />


</head>
<body>
    <div class="container">
        <div class="row">
            <div class="col-4 mx-auto vertical-middle">
                <p class="txt-center">
                    <img src="/-/media/Project/Realty/Icons/logo-adani-realty.png?la=en&amp;hash=074CA0CB379D87607FC97C445181693B" alt="Adani Realty" disablewebedit="False">
                    Download Booking Payment Transaction History
                </p>
                <p class="txt-center"><b>Select a date range to export the data </b></p>
                <form id="form2" runat="server">

                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="d-block">From</label>
                                <asp:TextBox ID="TextBoxFrom" runat="server" TextMode="Date" class="input w-100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="validateFrom" runat="server" ControlToValidate="TextBoxFrom" ErrorMessage="(From) Date is Required"></asp:RequiredFieldValidator>
                            
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="d-block">To</label>
                                <asp:TextBox ID="TextBoxTo" runat="server" TextMode="Date" class="input w-100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="validateTo" runat="server" ControlToValidate="TextBoxTo" ErrorMessage="(To) Date is Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="d-block">Payment Status</label>
                                <asp:DropDownList ID="drptype" runat="server" class="form-control">
                                    <asp:ListItem Value="--Please select--">--Please select--</asp:ListItem>
                                    <asp:ListItem Value="All">All</asp:ListItem>
                                    <asp:ListItem Value="Success">Success</asp:ListItem>
                                    <asp:ListItem value="Failure">Failed</asp:ListItem>
                                    <asp:ListItem value="Pending">Pending</asp:ListItem>
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
                    <div class="row">
                        <p class="txt-center w-100 d-block">
                            <asp:Button ID="Button1" runat="server" Text="Download" OnClick="Button1_Click" class="btn btn-primary" />
                        </p>
                    </div>


                </form>
            </div>
        </div>
    </div>


    <script src="/scripts/Realty/jquery.min.js"></script>
    <script src="/scripts/Realty/bootstrap.min.js"></script>
</body>
</html>
