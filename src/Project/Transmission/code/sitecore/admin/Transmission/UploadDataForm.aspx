<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadDataForm.aspx.cs" Inherits="Sitecore.Transmission.Website.sitecore.admin.Transmission.UploadDataForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <div>

            <table class="auto-style1">
                <tr>
                    <td colspan="4" align="center">UPLOAD A DOCUMENT</td>
                </tr>
                <tr>
                    <td colspan="4" align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td>From</td>
                    <td>
              <asp:DropDownList ID="SourceDDL" runat="server"  AppendDataBoundItems="True"></asp:DropDownList>
      </td>
                    <td>To</td>
                    <td>
        <asp:TextBox ID="DocumentTitle" runat="server"></asp:TextBox>
        </td>
                </tr>
                <tr>
                    <td colspan="4">  
        <asp:FileUpload ID="Img" runat="server" AssociatedControlID="Img" />
             </tr>
                <tr>
        <td colspan="4"><asp:Button ID="Button1" runat="server" Text="Upload" OnClick="UploadButton_Click" />
              </tr>
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
            </table>

        </div>
    </form>
     <script src="/scripts/Transmission/jquery.min.js"></script>
    <script src="/scripts/Transmission/bootstrap.min.js"></script>
</body>
</html>
