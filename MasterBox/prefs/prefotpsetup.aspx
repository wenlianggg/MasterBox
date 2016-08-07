<%@ Page Title="One Time Password Setup" Language="C#" MasterPageFile="~/prefs/Preferences.Master" AutoEventWireup="true" CodeBehind="prefotpsetup.aspx.cs" Inherits="MasterBox.Prefs.prefotpsetup" %>

<asp:Content ID="OneTimeSetup" ContentPlaceHolderID="Preferences" runat="server">
        <div class="page-header">
          <h1><%: Page.Title %>
              <small>Use a second layer of authentication for added security</small>
          </h1>
        </div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
		<li>User Preferences</li>
		<li>Security</li>
		<li>Authentication</li>
		<li class="active"><%: Page.Title %></li>
	</ol>
	<br />
    <div runat="server" id="ExistingOTP" class="alert alert-success" role="alert" visible="false">Hey, it seems like you already have 2FA set up!</div>
	<div class="panel panel-primary">
		<div class="panel-heading">
			<h3 class="panel-title">Two-Factor Authentication Setup</h3>
		</div>
		<div class="panel-body">
			<div class="setupRow">
				<div class="setupRowLeft">
					<p><strong>Use the QR code to configure your Two-Factor Authentication on your TOTP app on multiple devices</strong></p>
					<ol>
						<li>Download any OTP Generator app, E.g. Authy, Google Authenticator, etc.</li>
						<li>Open the app, then scan the QR code to the right or manually enter this code: <code><asp:Label runat="server" ID="GeneratedSecret" Text="TOTPSECRETHERE"></asp:Label></code></li>
						<li>Enter the current six-digit numerical passcode from the application to verify that your device is properly configured</li>
					</ol>
					<br />
					<table>
                        <tr>
                            <td><strong>Current Password:  </strong></td>
                            <td><asp:TextBox ID="CurrPw" CssClass="otpSetupValue form-control" runat="server" TextMode="Password"/></td>
                        </tr>
                        <tr>
                            <td><strong>Generated OTP:  </strong></td>
                            <td><asp:TextBox ID="OTPVal" CssClass="otpSetupValue form-control" runat="server" /></td>
                        </tr>
                    </table>
					<br />
                    <asp:Label runat="server" ID="Msg" ForeColor="Red" /><br>
					<asp:Button ID="OTPSubmitBtn" runat="server" Text="Enable 2FA" CssClass="btn btn-success otpEnable" OnClick="VerifyOTP_Button" />
					<asp:Button ID="OTPCancelBtn" runat="server" Text="Disable 2FA" CssClass="btn btn-danger otpCancelSetup" OnClick="DisableTOTP_Button" />
				</div>
				<div class="setupRowRight" runat="server" id="QRCodeHolder">
                    <asp:Image ID="OTPQrCode" runat="server" AlternateText="QR Code Not Available" height="300px" Width="300px"/>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
