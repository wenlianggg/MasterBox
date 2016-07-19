<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="MasterBox.filestore.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label runat="server" id="HelloWorldLabel"></asp:Label>
        <br />
        <asp:Label runat="server" id="VerifyLabel"></asp:Label>
        <br />
        <asp:Label runat="server" id="Check"></asp:Label>
        <br />
        <asp:Button runat="server" ID="DLButton" Text="Download"></asp:Button>
        <asp:Button runat="server" ID="UPLButton" Text="Upload"></asp:Button>
        <asp:Button runat="server" ID="DELButton" Text="Delete"></asp:Button>
        <br />
        <asp:Label runat="server" ID="CountDown"></asp:Label>
    </div>
    </form>
</body>
</html>
