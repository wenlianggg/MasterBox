<%@ Page Title="Register" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="MasterBox.Auth.SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<link href="<%= ResolveUrl("~/CSS/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
	<script src='https://www.google.com/recaptcha/api.js'></script>
</asp:Content>
<asp:Content ID="Register" ContentPlaceHolderID="MainContent" runat="server">
	<div class="jumbotron">
		<h1><%: Title %></h1>
		<p>Welcome to MasterBox, please login or register to access our features.</p>
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
			<asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="DarkRed"
			DisplayMode="BulletList" ShowSummary ="true" HeaderText="Errors:" />
			<asp:Label ID="Msg" ForeColor="red" runat="server" />
			<table runat="server" class="regTable">
				<tr>
					<td><strong>Username: </strong></td>
					<td>
						<asp:TextBox ID="UserName" runat="server" CssClass="form-control" placeholder="cookiemonster123" /></td>
					<td>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="UserName" ForeColor="Red"
							ErrorMessage="Username is required." Text="*" runat="server" />
						<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
							ErrorMessage="Username can only contain alphanumerics and underscores, and at least 4 characters"
							ControlToValidate="UserName" Display="None" ValidationExpression="^[a-zA-Z_]{4,40}$" />
					</td>
				</tr>
				<tr>
					<td><strong>Password:</strong></td>
					<td>
						<asp:TextBox ID="UserPass" CssClass="pwdfield form-control" TextMode="Password" runat="server" placeholder=""/>
					</td>
					<td>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="UserPass" ForeColor="Red"
							ErrorMessage="Password is required." Text="*" runat="server" />
						<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
							ErrorMessage="Password requires an uppercase, lowercase and a number." 
							ControlToValidate="UserPass" Display="None" ValidationExpression="^[a-zA-Z0-9_]{8,50}$" />
					</td>
				</tr>
				<tr>
					<td><strong>Confirm Password:</strong></td>
					<td>
						<asp:TextBox ID="UserPassCfm" CssClass="pwdfield form-control" TextMode="Password" runat="server" placeholder=""/>
					</td>
					<td>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="UserPassCfm" ForeColor="Red"
							ErrorMessage="Password confirmation required" Text="*" runat="server" />
						<asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="UserPassCfm"
							ControlToCompare="UserPass" Display="None" ErrorMessage="Passwords entered do not match!"/>
					</td>
				</tr>
				<tr>
					<td><strong>First Name:</strong></td>
					<td>
						<asp:TextBox ID="FirstName" CssClass="form-control" runat="server" placeholder="Cookie" />
					</td>
					<td>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="FirstName" ForeColor="Red"
							ErrorMessage="First Name is required" Text="*" runat="server" />
						<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
							ErrorMessage="First name can only contain alphabets and spaces up to 50 characters" 
							ControlToValidate="FirstName" Display="None" ValidationExpression="^[a-zA-Z ]{1,50}$" />
					</td>
				</tr>
				<tr>
					<td><strong>Last Name:</strong></td>
					<td>
						<asp:TextBox ID="LastName" CssClass="form-control" runat="server" placeholder="Monster" />
					</td>
					<td>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="LastName" ForeColor="Red"
							ErrorMessage="Last Name is required." Text="*" runat="server" />
						<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
							ErrorMessage="Last name can only contain alphabets and spaces up to 50 characters" 
							ControlToValidate="FirstName" Display="None" ValidationExpression="^[a-zA-Z ]{1,50}$" />
					</td>
				</tr>
				<tr>
					<td><strong>E-mail Address:</strong></td>
					<td>
						<asp:TextBox ID="UserEmail" CssClass="form-control" runat="server" placeholder="cookie@monsters.com" />
					</td>
					<td>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="UserEmail" ForeColor="Red"
							ErrorMessage="Email Address is required." Text="*" runat="server" />
						<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
							ErrorMessage="Last name can only contain alphabets and spaces up to 50 characters" 
							ControlToValidate="UserEmail" Display="None" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
					</td>
				</tr>
				<tr>
					<td><strong>Confirm E-Mail:</strong></td>
					<td>
						<asp:TextBox ID="UserEmailCfm" CssClass="form-control" runat="server" placeholder="cookie@monsters.com" />
					</td>
					<td>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="UserEmailCfm" ForeColor="Red"
							ErrorMessage="Email Address confirmation required." Text="*" runat="server" />
						<asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="UserEmailCfm"
							ControlToCompare="UserEmail" Display="None" ErrorMessage="Emails entered do not match!" />
					</td>
				</tr>
				<tr>
					<td><strong>Captcha:</strong></td>
					<td>
						<div class="g-recaptcha" data-sitekey="6Ld6kiETAAAAAMplec1OuKhJ3VKCBOhZmOcAkZsg"></div>
					</td>
				</tr>
			</table>
			<p>
			</p>

			<br />
			<asp:Button ID="RegisterButton" Text="Sign Up" CssClass="btn btn-success" OnClick="processRegistration" runat="server" />
		</div>
	</div>
</asp:Content>
