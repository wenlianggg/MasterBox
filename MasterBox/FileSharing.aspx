<%@ Page Title="File Sharing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="MasterBox.filestore.WebForm1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

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

</asp:Content>