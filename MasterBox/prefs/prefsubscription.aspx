<%@ Page Title="General Settings" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="prefsubscription.aspx.cs" Inherits="MasterBox.mbox.PrefSubscription" %>

<asp:Content ID="SetGeneral" ContentPlaceHolderID="Preferences" runat="server">
    <div class="Setting_Profile">
        <div class="page-header">
          <h1>Subscriptions
              <small>Expand your available storage space</small>
          </h1>
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
    </div>
</asp:Content>

