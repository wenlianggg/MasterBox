<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Subscriptions.aspx.cs" Inherits="MasterBox.Subscriptions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1>Your Subscriptions</h1>
        <p>Manage your payments subscriptions and storage space.</p>
    </div>
    <h1 id="TotalSpace" runat="server"></h1>
    <div class="panel panel-default">
        <div class="panel-heading"><strong>Storage</strong></div>
        <div class="panel-body">
            <div class="progress">
                <div class="progress-bar progress-bar-striped active" role="progressbar" id="Bar"
                    aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width: 40%" runat="server">
                </div>
            </div>
            <h4 runat="server" id="Available"></h4>
            <asp:Button OnClick="GoToFiles" runat="server" CssClass="btn btn-info" Text="Go To Files" />
        </div>
    </div>
    <div class="panel panel-default">
        <div class="panel-heading"><strong>Subscription Plans</strong></div>
        <div class="panel-body">
            <p runat="server" id="FreeSpace"></p>
            <asp:Button runat="server" CssClass="btn btn-info" Text="Purchase Space" OnClick="GoToPrices" />
        </div>
    </div>
</asp:Content>
