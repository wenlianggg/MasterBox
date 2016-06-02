<%@ Page Title="User Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="MasterBox.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<link href="<%= ResolveUrl("~/Auth/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
	<script src='https://www.google.com/recaptcha/api.js'></script>
</asp:Content>
<asp:Content ID="Register" ContentPlaceHolderID="MainContent" runat="server">
	<div class="jumbotron">
		<h1><%: Title %></h1>
		<p>Please login or register to access our features.</p>
	</div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
		<li><a href="<%= ResolveUrl("~/Auth/signup") %>">Registration</a></li>
		<li class="active">Details</li>
	</ol>
	<br />
	<div class="panel panel-default">
		<div class="panel-heading">
			<h3 class="panel-title">Registration Form</h3>
		</div>
		<div class="panel-body">
			<table runat="server" class="regTable">
				<tr>
					<td><strong>Username: </strong></td>
					<td>
						<asp:TextBox ID="UserName" runat="server" CssClass="form-control" /></td>
					<td>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator1"
							ControlToValidate="UserName"
							ErrorMessage="Cannot be empty."
							runat="server" />
					</td>
				</tr>
				<tr>
					<td><strong>Password:</strong></td>
					<td>
						<asp:TextBox ID="UserPass" CssClass="pwdfield form-control" TextMode="Password" runat="server" />
					</td>
					<td>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator2"
							ControlToValidate="UserPass"
							ErrorMessage="Cannot be empty."
							runat="server" />
					</td>
				</tr>
				<tr>
					<td><strong>Confirm Password:</strong></td>
					<td>
						<asp:TextBox ID="UserPassCfm" CssClass="pwdfield form-control" TextMode="Password" runat="server" />
					</td>
					<td>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator3"
							ControlToValidate="UserPassCfm"
							ErrorMessage="Cannot be empty."
							runat="server" />
					</td>
				</tr>
				<tr>
					<td><strong>First Name:</strong></td>
					<td>
						<asp:TextBox ID="FirstName" CssClass="form-control" runat="server" />
					</td>
					<td>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator5"
							ControlToValidate="FirstName"
							ErrorMessage="Cannot be empty."
							runat="server" />
					</td>
				</tr>
				<tr>
					<td><strong>Last Name:</strong></td>
					<td>
						<asp:TextBox ID="LastName" CssClass="form-control" runat="server" />
					</td>
					<td>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator6"
							ControlToValidate="LastName"
							ErrorMessage="Cannot be empty."
							runat="server" />
					</td>
				</tr>
				<tr>
					<td><strong>E-mail Address:</strong></td>
					<td>
						<asp:TextBox ID="UserEmail" CssClass="form-control" runat="server" />
					</td>
					<td>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator4"
							ControlToValidate="UserEmail"
							ErrorMessage="Cannot be empty."
							runat="server" />
					</td>
				</tr>
				<tr>
					<td><strong>Human Verification:</strong></td>
					<td>
						<div class="g-recaptcha" data-sitekey="6Ld6kiETAAAAAMplec1OuKhJ3VKCBOhZmOcAkZsg"></div>
					</td>
				</tr>
			</table>
			<br />
			<asp:Button ID="RegisterButton"
				Text="Sign Up"
				CssClass="btn btn-success"
				OnClick="processRegistration"
				runat="server" />
		</div>
	</div>



	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" />
	</p>
</asp:Content>
