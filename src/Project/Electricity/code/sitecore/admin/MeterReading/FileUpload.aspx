<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="Sitecore.Electricity.Website.sitecore.admin.MeterReading.FileUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblSuccessMsg" runat="server" Visible="false" Text="Records Uploaded Successfully"></asp:Label>
            <asp:Label ID="lblErroMsg" runat="server" Visible="false"></asp:Label>
        </div>
        <div>
            <asp:FileUpload ID="fuExcelselection" runat="server" />
            <asp:Button ID="btnUploadFile" runat="server" Text="Upload" OnClick="btnUploadFile_Click" />
        </div>
    </form>
</body>
</html>
