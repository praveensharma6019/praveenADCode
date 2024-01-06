<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadDoc.aspx.cs" Inherits="Sitecore.Transmission.Website.sitecore.admin.Transmission.UploadDoc" %>

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
        header{position:relative;}
		header .toggle-btn{text-align: right;}
		header .toggle-btn i{background: #f0f0f0; padding: 5px 10px; font-size: 1.2rem; border: 1px solid #ececec; border-top: 0px; cursor: pointer;transition: ease all 0.3s;}
		header .toggle-btn i:hover{background: #868686; color: #fff;transition: ease all 0.3s;}
	
          div.toggle{position: relative; width: 100%; left: 0px; top: 0px; background: #f7f7f7; z-index: 9; border-bottom: 1px solid #e8e8e8; padding: 1rem 0rem;transition: ease all 0.3s;}
	
        
	
                div.toggle ul{text-align:center;margin-bottom: 0px;}

                div.toggle ul li{display: inline-block;}
	
                div.toggle ul li a{border: 1px solid #e8e8e8; padding: 5px 5px; border-radius: 2px; color: #868686;transition: ease all 0.3s;font-size: 14px;}
	
                div.toggle ul li a:hover{transition: ease all 0.3s;background: #616161; color: #fff;text-decoration: none;}

                div.toggle.active{height: 0px; padding: 0px; transition: ease all 0.3s; overflow: hidden;}
    </style>
    <!-- Bootstrap core CSS -->
    <link href="/styles/Transmission/bootstrap.min.css" rel="stylesheet" />
    
    
</head>
     <link href="https://adanistaging-cm.azurewebsites.net/styles/Transmission/bootstrap.min.css" rel="stylesheet"></head>
    <link href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet"></head>
<body>

    <header>
<div class="toggle">
<div class="container">
	<ul>
		<li><a href="/sitecore/admin/Transmission/UploadData.aspx?Sorce=CorporateAnnouncement"><i class="fa fa-upload mr-2"></i>Corporate Announcement</a></li>
		<li><a href="/sitecore/admin/Transmission/UploadData.aspx?Sorce=NewsAdvertisement"><i class="fa fa-upload mr-2"></i>News Advertisement</a></li>
		<li><a href="/sitecore/admin/Transmission/UploadData.aspx?Sorce=OtherDownloads"><i class="fa fa-upload mr-2"></i>Other Downloads</a></li>
		<li><a href="/sitecore/admin/Transmission/UploadDoc.aspx?Sorce=Corporate Governance"><i class="fa fa-upload mr-2"></i>Corporate Governance</a></li>
		<li><a href="/sitecore/admin/Transmission/UploadDOC.aspx?Sorce=InvestorDownloads"><i class="fa fa-upload mr-2"></i>Investor Downloads</a></li>
	</ul>
</div>
</div>
<div class="container d-none"><div class="toggle-btn"><i class="fa fa-angle-double-down mr-2"></i></div></div>
</header>

    <div class="container">
         
        <div class="row">
            <div class="col-md-5 mx-auto vertical-middle">
                <p class="txt-center">
                    <img src="/images/ports/Ports-and-terminals-logo-footer.png" alt="Adani" disablewebedit="False">
                </p>
                <p class="txt-center"><b>Upload a new file here, to manage files click on CMS View button  </b></p>
                <form id="form2" runat="server">

                    <div class="row">
                        <div class="col-sm-12">
                            <div class="form-group">
                                <label class="d-block">Source Folder</label>
                              <div class="row"><div class="col-9">
							   <asp:DropDownList ID="SourceDDL" runat="server"  onchange="TypeDiv()"  AppendDataBoundItems="True" class="input w-100"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SourceDDL" ErrorMessage="(From) Date is Required"></asp:RequiredFieldValidator>     
  </div>
							<div class="col-3">
								
                                <p class="text-right"><a target="_blank"  class="btn btn-secondary" id="CmsLink" href="https://adanistaging-cm.azurewebsites.net/sitecore/shell/Applications/Content%20Editor.aspx?sc_bw=1&amp;sc_lang=en&amp;ro={D31147B9-4EFE-4530-976F-1F42D034FC0D}">CMS View</a></p></div>  </div>
                                </div>

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label class="d-block">File Title</label>
                                <asp:TextBox ID="DocumentTitle" runat="server" class="input w-100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DocumentTitle" ErrorMessage="(To) Date is Required"></asp:RequiredFieldValidator>
                                 <%-- <asp:TextBox ID="TextBoxTo" runat="server" TextMode="Date" class="input w-100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="validateTo" runat="server" ControlToValidate="TextBoxTo" ErrorMessage="(To) Date is Required"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                  <label class="d-block">Upload File</label>
                                <%--<asp:DropDownList ID="drpCity" runat="server" class="form-control" AutoPostBack="false">
                                </asp:DropDownList>--%>
                                <asp:FileUpload ID="Img" runat="server" AssociatedControlID="Img" class="form-control"  />
                            </div>
                        </div>
                    </div>

                    <div class="row txt-center">
                        <p class="txt-center w-100 d-block">
                            <strong class="txt-red">
                                 <asp:Label ID="lblErroMsg" runat="server" class="txt-orange col-sm-5 col-md-5 col-form-label" ></asp:Label></strong>
                        </p>
                    </div>
                    <div class="row">
                        <p class="txt-center w-100 d-block">                            
                            <asp:Button ID="Button1" runat="server" Text="Upload" OnClick="UploadButton_Click" class="btn btn-primary" />
                        </p>
                    </div>


                </form>
            </div>
        </div>
    </div>







     <script src="/scripts/Transmission/jquery.min.js"></script>
    <script src="/scripts/Transmission/bootstrap.min.js"></script>
     <script>

        $(document).ready(function(){
               var e = document.getElementById("SourceDDL");
            var strUserText = e.options[e.selectedIndex].text;
             var strUservalue = e.options[e.selectedIndex].value;
               
               var href ="https://adanistaging-cm.azurewebsites.net/sitecore/shell/Applications/Content%20Editor.aspx?sc_bw=1&sc_lang=en&ro="+strUservalue;
               document.getElementById("CmsLink").href = href;
           
                // $("CalendarExtender1").hide();
         });




           function TypeDiv() {
            var e = document.getElementById("SourceDDL");
            var strUserText = e.options[e.selectedIndex].text;
               var strUservalue = e.options[e.selectedIndex].value;
               
               var href ="https://adanistaging-cm.azurewebsites.net/sitecore/shell/Applications/Content%20Editor.aspx?sc_bw=1&sc_lang=en&ro="+strUservalue;
               document.getElementById("CmsLink").href = href;

            }



         </script>
</body>
</html>
