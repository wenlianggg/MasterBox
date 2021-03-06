﻿<%@ Page Title="Expand Storage" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="prefsubscription.aspx.cs" Inherits="MasterBox.Prefs.PrefSubscription" %>

<asp:Content ID="SetGeneral" ContentPlaceHolderID="Preferences" runat="server">
    <div class="Setting_Profile">
        <div class="page-header">
          <h1>Subscriptions
              <small>Expand your available storage space</small>
          </h1>
        </div>
		<ol class="breadcrumb" style="margin-bottom: 5px;">
			<li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
			<li>User Preferences</li>
			<li class="active"><%: Page.Title %></li>
		</ol>
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
                <p runat="server" id="Additional"></p>
                <p runat="server" id="SubscriptionStart"></p>
                <p runat="server" id="SubscriptionEnd"></p>
                <asp:Button runat="server" CssClass="btn btn-info" Text="Purchase Space" OnClick="GoToPrices" />
            </div>
        </div>
        <div class="panel panel-default">
            <div class="panel-heading"><strong>Coupons!</strong></div>
            <div class="panel-body">
                <asp:TextBox runat="server" ID="CouponValue" CssClass="form-control"></asp:TextBox>
                <br />
                <asp:Button runat="server" CssClass="btn btn-success" OnClick="RedeemCoupon" Text="Redeem A Coupon!"/>
            </div>
        </div>
    </div>
</asp:Content>

