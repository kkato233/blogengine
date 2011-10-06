<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Playground.aspx.cs" Inherits="Playground" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:FileUpload runat="server" ID="fileUpload" />
    <asp:Button runat="server" ID="btnUpload" Text="Upload" OnClick="btnUpload_Click" />
    <br />
    <asp:HyperLink runat="server" ID="hprLink" />
    <br />
    <asp:Image runat="server" ID="img" Visible="false" />
    </div>
    </form>
</body>
</html>
