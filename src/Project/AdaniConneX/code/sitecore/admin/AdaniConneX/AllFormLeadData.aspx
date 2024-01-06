<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllFormLeadData.aspx.cs" Inherits="Sitecore.AdaniConneX.Website.sitecore.admin.AdaniConneX.AllFormLeadData" %>

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

    <!-- Bootstrap core CSS -->
    <link href="/styles/AdaniConneX/bootstrap.min.css" rel="stylesheet" />
</head>

<body>
    <div class="container">
        <div class="row">
            <div class="col-4 mx-auto vertical-middle">
                <%--Logo--%>
                <p class="txt-center">
                    <img src="/-/media/Project/AdaniConneX/AdaniConneX-Homepage-Assets/AdaniConneX-RGB-Primary-FullColor.svg?la=en&hash=0E58297A4A12F5DD823EDD3611BAF076" alt="Adani ConneX" disablewebedit="False"/>
                </p>
                <%--Date Range Option--%>
                <p class="txt-center"><b>Select a date range to export the data </b></p>
                <form id="form2" runat="server">
                    <div class="row">
                        <%--From--%>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="d-block">From</label>
                                <asp:TextBox ID="TextBoxFrom" runat="server" TextMode="Date" class="input w-100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="validateFrom" runat="server" ControlToValidate="TextBoxFrom" ErrorMessage="(From) Date is Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <%--To--%>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="d-block">To</label>
                                <asp:TextBox ID="TextBoxTo" runat="server" TextMode="Date" class="input w-100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="validateTo" runat="server" ControlToValidate="TextBoxTo" ErrorMessage="(To) Date is Required"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>

                    <%--Form Type Option--%>
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <asp:DropDownList ID="drptype" runat="server" class="form-control">
                                    <asp:ListItem Value="Select form type">Select form type</asp:ListItem>
                                    <asp:ListItem Value="ContactUs">Contact Us</asp:ListItem>
                                    <asp:ListItem Value="GetInTouch">Get In Touch</asp:ListItem>
                                    <asp:ListItem Value="WhitePaper">White Paper</asp:ListItem>
                                    <asp:ListItem Value="Ebook_form">Ebook</asp:ListItem>
                                    <asp:ListItem Value="JoinUs">Join Us</asp:ListItem>
                                    <asp:ListItem Value="ConnectWithHR">Connect With HR</asp:ListItem>
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
                            <asp:Button ID="Button1" runat="server" Text="Download" OnClick="Button1_Click" class="btn btn-primary" style="width: 366px;"/>
                        </p>
                    </div>
                    <div class="row">
                        <p class="txt-center w-100 d-block">
                            <asp:Button ID="Button2" runat="server" Text="Download All" OnClick="Button2_Click" class="btn btn-primary" style="width: 366px;"/>
                        </p>
                    </div>

                </form>
            </div>
        </div>
    </div>
    <script src="/scripts/AdaniConneX/jquery.min.js"></script>
    <script src="/scripts/AdaniConneX/bootstrap.min.js"></script>
</body>
</html>
