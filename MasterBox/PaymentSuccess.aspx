<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PaymentSuccess.aspx.cs" Inherits="MasterBox.PaymentSuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Payment Succesful.</h1>
        <p>You will be automatically redirected! Hold on!</p>
        <p id="ItemName" runat="server"></p>
        <h1 id="TestSpace" runat="server"></h1>
    </div>
</asp:Content>
