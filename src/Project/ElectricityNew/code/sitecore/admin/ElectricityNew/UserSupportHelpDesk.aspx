<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSupportHelpDesk.aspx.cs" Inherits="Sitecore.ElectricityNew.Website.sitecore.admin.Electricity.UserSupportHelpDesk" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
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
                            <h4 class="float-left"><span class="txt-orange">User Support Form</span></h4>
                            <div class="clearfix"></div>
                        </div>
                    </div>
                    <!-- /Welcome Message -->
                    <div class="py-2"></div>
                    <!-- Section -->

                    <div class="row">
                        <!-- Main Content Panel -->
                        <div class="col-md-12 col-lg-12" id="mainContent">
                            <div class="clearfix"></div>
                            <!-- Update Page Content-->
                            <div class="panel">
                                <form id="form1" class="pt-3" runat="server">
                                    <div class="form-group row">
                                        <div class="col-sm-12">
                                            <strong>
                                                <asp:Label ID="lblSuccessMsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" Visible="false" Text="Records Updated Successfully"></asp:Label></strong>
                                            <strong>
                                                <asp:Label ID="lblErroMsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" Visible="false"></asp:Label></strong>
                                           
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-12 col-md-12 col-form-label"><strong>Find user by account number</strong></label></div>
                                    </div>
                                  <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Account Number</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">
                                                    <asp:TextBox ID="txtaccountforuser" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-12">
                                            <div>
                                                <div class="col-sm-12 col-lg-12">
                                                    <asp:Button Text="Fetch User" ID="btnSearchUser" runat="server" class="btn btn-primary float-left" OnClick="btnSearchUser_Click" />
                                                </div>
                                                
                                                <strong>
                                                <asp:Label ID="lblmsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" Visible="true"></asp:Label></strong>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-8">
                                            <label for="" class="col-sm-5 col-md-5 col-form-label">Registred User List</label>
                                            <div class="col-sm-12 col-md-12">
                                                <div class="input-group date mb-3">


                                                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                                                    </asp:ScriptManager>

                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                        <ContentTemplate>

                                                            <asp:GridView ID="gvSecondaryAcList" runat="server"
                                                                AutoGenerateColumns="false"
                                                                DataKeyNames="Id"
                                                                PageSize="5"
                                                                AllowPaging="true"
                                                                OnPageIndexChanging="gvSecondaryAcList_PageIndexChanging"
                                                               EmptyDataText="No records available.">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Accoun Number" ItemStyle-Width="150">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("AccountNumber") %>'></asp:Label>
                                                                            <asp:HiddenField ID ="hdnID" runat="server" Value='<%# Eval("Id") %>' />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Meter Number" ItemStyle-Width="150">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMeterNumber" runat="server" Text='<%# Eval("MeterNumber") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Account Type" ItemStyle-Width="150">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAccountType" runat="server" Text='<%# Eval("AccountType") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="User Name" ItemStyle-Width="150">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAccountType" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Email Id" ItemStyle-Width="150">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAccountType" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                     <asp:TemplateField HeaderText="Mobile Number" ItemStyle-Width="150">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAccountType" runat="server" Text='<%# Eval("Mobile") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <div>
                                                <div class="col-sm-12 col-lg-12">
                                                    <asp:Button ID="btnsubmit" runat="server" OnClick="btnsubmit_Click" Text="Update" class="btn btn-primary" ValidationGroup="SubmitData" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </form>
                            </div>
                            <!-- / Update Page Content-->
                        </div>
                        <!-- / Main Content Panel -->
                    </div>
                    <!-- /Section -->
                </div>
            </div>
            <!-- / Page Content -->
        </div>
    </main>
    <!--  /Main content Section-->
    <!-- Footer -->
    <footer class="">
    </footer>
    <!-- /Footer -->

</body>
</html>
