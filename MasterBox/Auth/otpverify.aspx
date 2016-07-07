<%@ Page Title="OTP Authentication" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="otpverify.aspx.cs" Inherits="MasterBox.Auth.otpverify" %>
<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
	<link href="<%= ResolveUrl("~/CSS/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="OneTime" ContentPlaceHolderID="MainContent" runat="server">
    	<div class="jumbotron">
		<h1><%:Title %></h1>
		<p>Please enter your One Time Password to continue.</p>
	</div>
    	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
        <li><a href="<%= ResolveUrl("~/Auth/signin") %>">Login</a></li>
		<li class="active">One Time Password</li>
	</ol>
    <br />

	<div class="panel panel-default">
        <div class="panel-heading">
			<h3 class="panel-title">One Time Password</h3>
		</div>
        <div class="panel-body">
			<p>Enter your One-Time Password generated from your previously set-up authenticator app.
			<br />(Google Authenticator, Authy, etc..)</p>
			<asp:Label ID="UID" ForeColor="blue" runat="server" /><br />
           <table runat="server" class="otpTable">
               <tr>
                   <td><strong>OTP:</strong></td>
                   <td>
                       <asp:TextBox ID="OTPValue" runat="server" CssClass="form-control" ToolTip="Enter your OTP Auth Code"></asp:TextBox>
                   </td>
				   <td>
						<asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
							ControlToValidate="OTPValue"
							ValidatorGroup="valGroup1"
							ErrorMessage="Please ensure your OTP is entered."
							ForeColor="Red">
						</asp:RequiredFieldValidator>
				   </td>
               </tr>
           </table>
			<asp:Label ID="Msg" ForeColor="red" runat="server" />
			<br />
			<br />
		<asp:Button class="otpLogin" ID="OTPLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="ConfirmOTP"/>
		<asp:Button class="otpCancel" ID="OTPCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="CancelOTP" CausesValidation="false"/>
    </div>
    </div>
</asp:Content>
