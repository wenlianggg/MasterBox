<%@ Page Title="Login Authentication" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signin.aspx.cs" Inherits="MasterBox.SignIn" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
	<link href="~/Auth/LoginStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="LoginIn" ContentPlaceHolderID="MainContent" runat="server">
	<h2><%: Title %></h2>
	<h3>Welcome to MasterBox</h3>
	<p>Please login or register to access our features.</p>
	<table>
		<tr>
			<td><strong>E-mail address:</strong></td>
			<td>
				<asp:TextBox ID="UserEmail" runat="server" /></td>
			<td>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="UserEmail"
					Display="Dynamic" ErrorMessage="Cannot be empty." runat="server" />
			</td>
		</tr>
		<tr>
			<td><strong>Password:</strong></td>
			<td>
				<asp:TextBox ID="UserPass" CssClass="pwdfield" TextMode="Password" runat="server" />
			</td>
			<td>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="UserPass"
					ErrorMessage="Cannot be empty." runat="server" />
			</td>
		</tr>
		<tr>
			<td><strong>Remember me?</strong></td>
			<td>
				<asp:CheckBox ID="Persist" runat="server" /></td>
		</tr>
		<tr>
			<td><strong>I am a...</strong></td>
			<td>
				<asp:RadioButtonList runat="server">
					<asp:ListItem  />
					<asp:ListItem  />
				</asp:RadioButtonList>
			</td>
		</tr>
	</table>
	<asp:Button ID="LoginButton" Text="Sign In" CssClass="btn btn-default-blue loginBtn" OnClick="logonClick" runat="server" />
	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" /></p>
	<script>
		function registerMode(bool) {
			$(".pwdfield").attr('readonly', bool);
			$(".pwdfield").attr('disabled', bool);
			if (bool === true) {
				$(".loginBtn").prop('value', "Register");
			} else {
				$(".loginBtn").prop('value', "Sign In");
			}
		}
		pwdFieldActive(false);
	</script>
</asp:Content>
