<%@ Page Title="Password Reset" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="lostpw.aspx.cs" Inherits="MasterBox.Auth.PasswordReset" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<link href="<%= ResolveUrl("~/CSS/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
	<script src='https://www.google.com/recaptcha/api.js'></script>
</asp:Content>
<asp:Content ID="Register" ContentPlaceHolderID="MainContent" runat="server">
	<div class="jumbotron">
		<h1><%: Title %></h1>
		<p>Reset your password if you have lost it</p>
	</div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
		<li><a href="<%= ResolveUrl("~/Auth/signup") %>">Login</a></li>
		<li class="active">Forgotten Password</li>
	</ol>
	<br />
	<div class="panel panel-default">
		<div class="panel-heading">
			<h3 class="panel-title">Reset your password</h3>
		</div>
		<div class="panel-body">
			<div class="alert alert-info" role="alert" runat="server" visible="false" id="Message">
				<asp:Label ID="Msg" runat="server" />
			</div>
			<table runat="server" class="loginTable">
				<tr>
					<td><strong>Username:</strong></td>
					<td>
						<asp:TextBox ID="UserName" runat="server" CssClass="form-control" /></td>
					<td>
						<asp:RequiredFieldValidator
							ID="UserNameValidator"
							ControlToValidate="UserName"
							Display="Dynamic"
							ErrorMessage="*"
							ValidationGroup="valGroup1"
							runat="server" />
					</td>
				</tr>
				<tr>
					<td><strong>Password Reset Key:</strong></td>
					<td>
						<asp:FileUpload ID="UploadControl" runat="server" />
					</td>
					<td>
						<asp:RequiredFieldValidator
							ID="StegUploadVal"
							ControlToValidate="UploadControl"
							ErrorMessage="*"
							runat="server" />
					</td>
				</tr>
			</table>
			<asp:Button ID="PwResetBtn"
				Text="Reset Password"
				CssClass="btn btn-primary"
				OnClick="ValidateSubmission_Click"
				runat="server" />
		</div>
	</div>
	<p>
	</p>
</asp:Content>
