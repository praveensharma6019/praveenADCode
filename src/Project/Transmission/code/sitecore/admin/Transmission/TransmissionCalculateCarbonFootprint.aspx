<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TransmissionCalculateCarbonFootprint.aspx.cs" Inherits="Sitecore.Transmission.Website.sitecore.admin.Transmission.TransmissionCalculateCarbonFootprint" %>


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
                    <img src="/images/AdaniGas/logo.png" alt="AdaniGas" disablewebedit="False">
                </p>
                <p class="txt-center"><b>Select a date range and connection type to export the data </b></p>
                <form id="form2" runat="server">
                    <ul>
                     <li>
                            <div>

                             
                                <h3><span>
                                    <asp:Label ID="totalRegistration" runat="server" Text=""></asp:Label></span> Total Registraions</h3>
                                <p>
                                    Paid -
                                    <asp:Label ID="totalPplRegistration" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Outstanding -
                                    <asp:Label ID="totalCarbonEmission" runat="server" Text=""></asp:Label>
                                </p>
                                <p>
                                    Complimentary -
                                    <asp:Label ID="AveragetotalCarbonEmission" runat="server" Text=""></asp:Label>
                                </p>
                            </div>
                        </li>
                        </ul>
                      <div class="col-sm-6 col-md-12">
                        Connection TypedataTables_length

                        <asp:DropDownList ID="selectConnectionType" runat="server" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 40px;">
<asp:ListItem Value="">Select Company Name</asp:ListItem>
                               <asp:ListItem Value="Adani Transmission Limited">Adani Transmission Limited</asp:ListItem>
                            <asp:ListItem Value="Adani Power Limited">Adani Power Limited</asp:ListItem>
                            <asp:ListItem Value="Adani Electricity Mumbai Limited">Adani Electricity Mumbai Limited</asp:ListItem>
                            <asp:ListItem Value="Others">Others</asp:ListItem>
                           <asp:ListItem Value="All">All</asp:ListItem>
                            

                        </asp:DropDownList>
                          <asp:RequiredFieldValidator ID="ValidateDropdown" runat="server" ControlToValidate="selectConnectionType"
   ErrorMessage="Connection Type is Required"></asp:RequiredFieldValidator>
                       
                    </div>
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