<%@ Page Title="One Time Password Setup" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="prefotp.aspx.cs" Inherits="MasterBox.Auth.otpsetup" %>
<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
	<link href="<%= ResolveUrl("~/Auth/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="OneTimeSetup" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
		<h1><%:Title %></h1>
		<p>Setup your two-factor authentication app</p>
	</div>
    	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
		<li class="active">One Time Password Setup.</li>
	</ol>
    <br />  

    <div class="panel panel-default">
        <div class="panel-heading">
			<h3 class="panel-title">Two-Factor Authentication Setup</h3>
		</div>
        <div class="panel-body">
            <div class="setupRow">
                <div class="setupRowLeft">
                    <p><strong>Use the QR code to configure your Two-Factor Authentication on your TOTP app on multiple devices</strong></p>
                    <ol>
                        <li>Download the any TOTP app, E.g. Authy, Google Authenticator, Etc.</li>
                        <li>Open the app, then scan the QR code to the right or manually enter this code: <code>XXXXXXXXXXXXXXXX</code></li>
                        <li><strong>Important: </strong>In the case that you have lost your device authenticator, save the backup code <code>YYYYYYYYYYYYYYYY</code> somewhere safe. This code can be used to access your account.</li>
                        <li>Enter the current six-digit numerical passcode from the application to verify that your device is properly configured</li>
                    </ol>
                    <br />
                        <asp:TextBox ID="setupValue" class="otpSetupValue" runat="server"></asp:TextBox>
                    <br />
                            <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
		                    ControlToValidate="setupValue"
		                    ValidatorGroup="valGroup1"
		                    ErrorMessage="Please Enter The Generated 6-digit code to continue."
		                    ForeColor="Red"/>
                    <br />
                    <asp:Button class="otpEnable" ID="OTPLogin" runat="server" Text="Enable" CssClass="btn btn-success"/>
		            <asp:Button class="otpCancelSetup" ID="OTPCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" CausesValidation="false"/>
                </div>
                <div class="setupRowRight" runat="server" id="QRCodeHolder">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
