<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClubAhmedabad-FormsRecord.aspx.cs" Inherits="Sitecore.BelvedereClubAhmedabad.Website.sitecore.admin.BelvedereClubAhmedabad.ClubAhmedabad_FormsRecord" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
                                .vertical-middle{border: 1px solid #f5f5f5;
                                box-shadow:0px 0px 11px -6px #000;
    padding: 25px;
    left: 50%;
    top: 50%;
    -webkit-transform: translate(-50%, -50%);
    transform: translate(-50%, -50%);
    position: absolute !important;}
.txt-center{text-align:center;}
.btn-primary{    background: #2768b3;
    color: #fff;
    border: 0px;
    padding: 5px 10px;
    transition: ease all 0.3s;}
.input{border: 1px solid #e2e2e2;
    background: #efefef;
    padding: 3px 5px;}

.btn-primary:hover{    background: #0653a7;
    transition: ease all 0.3s;}
.txt-red{color:red;}
    </style>
    <!-- Bootstrap core CSS -->
    <link href="/styles/BelvedereClubAhmedabad/bootstrap.min.css" rel="stylesheet" />
    
    
</head>
<body>
      <div class="container">
                <div class="row">
                                <div class="col-4 mx-auto vertical-middle">
                                    
                                                <p class="txt-center"><img src="../../../images/BelvedereClubAhmedabad/belvedere-logo.png"  alt="Belvedere Club Ahmedabad" disablewebedit="False" /></p>
                                                <p class="txt-center"><b>Select the Form Type and date range to export the data </b></p>
                                                <form id="form2" runat="server">
       
                                <div class="row">
                                     <div class="col-sm-6 mx-auto">
                                                                <div class="form-group">
                                            <asp:DropDownList ID="ddl1" runat="server" class="col-sm-12 mx-auto p-1">
                                                <asp:ListItem>--Select Form Type--</asp:ListItem>
                                                <asp:ListItem>Room & Suits Booking</asp:ListItem>
                                                <asp:ListItem>Enquiry Form</asp:ListItem>
                                            </asp:DropDownList>
                                                                </div>
                                     </div>
                                    </div>
                                    <div class="row">
                                                 <div class="col-sm-6 w-100">
                                                                <div class="form-group">
                                                                                <label class="d-block">From</label>
                        <asp:TextBox ID="TextBoxFrom" runat="server" TextMode="Date" class="input"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="validateFrom" runat="server" ControlToValidate="TextBoxFrom" ErrorMessage="(From) Date is Required"></asp:RequiredFieldValidator>
                                                                </div>
                                                </div>
                                                <div class="col-sm-6 w-100">
                                                                <div class="form-group">
                                                                <label class="d-block">To</label>
                        <asp:TextBox ID="TextBoxTo" runat="server" TextMode="Date" class="input"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="validateTo" runat="server" ControlToValidate="TextBoxTo" ErrorMessage="(To) Date is Required"></asp:RequiredFieldValidator>
                                                                </div>
                                                </div>
                                </div>
                                <div class="row txt-center">
                                                <p class="txt-center w-100 d-block"><strong class="txt-red"><asp:Label ID="lblErroMsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" Visible="false"></asp:Label></strong></p>
                                </div>
                                <div class="row">
                                                <p class="txt-center w-100 d-block"><asp:Button ID="Button1" runat="server" Text="Download" OnClick="Button1_Click" class="btn btn-primary"/></p>
                                </div>
                                
                                
                                                </form>
                                </div>
                </div>
</div>


    <script src="/scripts/BelvedereClubAhmedabad/jquery.min.js"></script>
    <script src="/scripts/BelvedereClubAhmedabad/bootstrap.min.js"></script>
</body>
</html>







