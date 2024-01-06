<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarbonCalculatorAuditTrailHistory.aspx.cs" Inherits="Sitecore.Transmission.Website.sitecore.admin.Transmission.CarbonCalculatorAuditTrailHistory" %>

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
        <form id="form1" runat="server">            
            <div class="row">
                <div class="col-xs-12 userchart_">
				<div class="text-center">
                    <img class="img-fluid mt-4" style="margin-bottom: 20px;" src="https://www.adanitransmission.com/%2F-%2Fmedia%2F41F1F040918C46E3BCACE28BD97FA71C.ashx" alt="Adani Transmission logo" width="200" height="90"/>
					</div>
                </div>
            </div>
             <hr>
            <div style="clear: both; padding: 50px 0 0 0;">
                <div class="mt-100 style="display: none">
                    <asp:placeholder id="gridrecord1" runat="server"></asp:placeholder>
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
