<%@ page language="C#" autoeventwireup="true" codebehind="VendorDetails.aspx.cs" inherits="Sitecore.Transmission.Website.sitecore.admin.Transmission.VendorDetails" %>

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
                    <a class="btn initiatives-btn2" href="/sitecore/admin/transmission/VendorAdminDashboard.aspx"><< Back to Admin Dashboard</a>
                </div>
                <div class="col-sm-2 col-xs-6 pull-right">
                    <%--<a id="logout" runat="server" class="btn initiatives-btn2" href="VendorAdminDashboard.aspx">Log Out</a>--%>
                </div>
            </div>
            <div class="panel">

                <form id="vedordetails" runat="server">
                    <div>
                        <h1>Vendor Details</h1>
                        <hr>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <h3>Registration No -
                    <asp:label id="registrationNo" runat="server" text=""></asp:label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Current Status -
                    <asp:label id="CurrentStatus" runat="server" text=""></asp:label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Name -
                    <asp:label id="name" runat="server" text=""></asp:label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Company Name -
                    <asp:label id="ComName" runat="server" text=""></asp:label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Email -
                    <asp:label id="EmailId" runat="server" text=""></asp:label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>MessageType -
                    <asp:label id="MessageType" runat="server" text=""></asp:label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Message -
                    <asp:label id="Message" runat="server" text=""></asp:label>
                            </h3>
                        </div>
                        <div class="col-md-6">
                            <h3>Contact No -
                    <asp:label id="ContactNo" runat="server" text=""></asp:label>
                            </h3>
                        </div>

                        <div class="col-md-6">
                            <h3>Inquiry Created On -
                    <asp:label id="InqCreatedOn" runat="server" text=""></asp:label>
                            </h3>
                        </div>
                        <div id="Level1" runat="server" class="col-md-12 row">
                            <div class="col-md-12">
                                <h1>Level 1 Details</h1>
                            </div>
                            <div class="col-md-6">
                                <h3>L1 Status -
                    <asp:label id="L1Status" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>L1 Remark -
                    <asp:label id="L1Remark" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>L1 Updated By
                    <asp:label id="L1UpdatedBy" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>L1 Updated On
                    <asp:label id="L1UpdatedOn" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                        </div>
                        <div id="Level2" runat="server" class="col-md-12 row">
                            <div class="col-md-12">
                                <h1>Level 2 Details</h1>
                            </div>
                            <div class="col-md-6">
                                <h3>L2 Status -
                    <asp:label id="L2Status" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>L2 Remark -
                    <asp:label id="L2Remark" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>L2 Updated By
                    <asp:label id="L2UpdatedBy" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>L2 Updated On
                    <asp:label id="L2UpdatedOn" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                        </div>
                        <div id="Level3" runat="server" class="col-md-12 row">
                            <div class="col-md-12">
                                <h1>Level 3 (Reassessment) Details</h1>
                            </div>
                            <div class="col-md-6">
                                <h3>L3 Status -
                    <asp:label id="L3Status" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>L3 Remark -
                    <asp:label id="L3Remark" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>L3 Updated By
                    <asp:label id="L3UpdatedBy" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                            <div class="col-md-6">
                                <h3>L3 Updated On
                    <asp:label id="L3UpdatedOn" runat="server" text=""></asp:label>
                                </h3>
                            </div>
                        </div>

                        <div id="SelectInqDiv" runat="server" class="col-md-6">
                            <div class="row">
                                <div class="col-md-5">
                                    <h3><span>Inquiry Status</span></h3>
                                </div>
                                <div class="col-md-7">
                                    <div class="form-group">
                                        <asp:dropdownlist id="selectStatus" runat="server" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%; height: 30px; font-size: 0.8rem;">
                                            <asp:ListItem>Select Inquiry Status</asp:ListItem>
                                        </asp:dropdownlist>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div id="RemarkInqDiv" runat="server" class="col-md-6">
                            <div class="row">
                                <div class="col-md-5">
                                    <h3>Remarks</h3>
                                </div>
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <asp:textbox id="Remarks" class="form-control" runat="server" textmode="MultiLine" height="80"></asp:textbox>
                                        <%--  <asp:RequiredFieldValidator ID="RemarkReq" runat="server" ControlToValidate="Remarks" ErrorMessage="Remark is Required"></asp:RequiredFieldValidator> --%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:label id="LabelError" runat="server" text=""></asp:label>
                        <div id="Updatebtn" runat="server" class="col-lg-12 text-right">
                            <asp:button id="UpdateInqBtn" runat="server" cssclass="btn initiatives-btn2" text="Update" onclick="UpdateInq_Click" />
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </section>


    <!--Export table button CSS-->
    <link rel="stylesheet" href="https://cdn.datatables.net/1.13.4/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/2.3.6/css/buttons.dataTables.min.css" />
    <script src="/scripts/Transmission/jquery-3.2.1.min.js"></script>

    <script src="/scripts/Transmission/bootstrap.min.js"></script>
    <script src="/scripts/Transmission/owl.carousel.min.js"></script>
    <script src="/scripts/Transmission/main.js"></script>
    <script src="/scripts/Transmission/jquery.easypiechart.min.js"></script>
    <script src="/scripts/Transmission/home.js"></script>
    <script src="/scripts/Transmission/modernizr-1.6.min.js"></script>
    <script src="/scripts/Transmission/register.js"></script>
    <!--Data Table-->

    <script type="text/javascript" src="https://cdn.datatables.net/1.13.4/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/2.3.6/js/dataTables.buttons.min.js"></script>
</body>
</html>
