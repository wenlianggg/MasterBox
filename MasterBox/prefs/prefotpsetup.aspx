<%@ Page Title="One Time Password Setup" Language="C#" MasterPageFile="~/prefs/Preferences.Master" AutoEventWireup="true" CodeBehind="prefotpsetup.aspx.cs" Inherits="MasterBox.Auth.prefotpsetup" %>

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

	<div class="panel panel-default">
		<div class="panel-heading">
			<h3 class="panel-title">Two-Factor Authentication Setup</h3>
		</div>
		<div class="panel-body">
			<div class="seowtupRow">
				<div class="setupRLeft">
					<p><strong>Use the QR code to configure your Two-Factor Authentication on your TOTP app on multiple devices</strong></p>
					<ol>
						<li>Download the any TOTP app, E.g. Authy, Google Authenticator, Etc.</li>
						<li>Open the app, then scan the QR code to the right or manually enter this code: <code>XXXXXXXXXXXXXXXX</code></li>
						<li><strong>Important: </strong>In the case that you have lost your device authenticator, save the backup code <code>YYYYYYYYYYYYYYYY</code> somewhere safe. This code can be used to access your account.</li>
						<li>Enter the current six-digit numerical passcode from the application to verify that your device is properly configured</li>
					</ol>
					<br />
					<asp:TextBox ID="setupValue" CssClass="otpSetupValue" runat="server"></asp:TextBox>
					<br />
					<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
						ControlToValidate="setupValue"
						ValidationGroup="valGroup1"
						ErrorMessage="Please Enter The Generated 6-digit code to continue."
						ForeColor="Red" />
					<br />
					<asp:Button ID="OTPLogin" runat="server" Text="Enable" CssClass="btn btn-success otpEnable" />
					<asp:Button ID="OTPCancel" runat="server" Text="Cancel" CssClass="btn btn-danger otpCancelSetup" CausesValidation="false" />
				</div>
				<div class="setupRowRight" runat="server" id="QRCodeHolder">
				</div>
			</div>
		</div>
	</div>
</asp:Content>
