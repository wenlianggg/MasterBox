<%@ Page Title="User Control Panel" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="changepw.aspx.cs" Inherits="MasterBox.ChangePw" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
	<link href="<%= ResolveUrl("~/Auth/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="LoginIn" ContentPlaceHolderID="MainContent" runat="server">
	<div class="jumbotron">
		<h1><%: Title %></h1>
		<p>Edit your user information here</p>
	</div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
		<li class="active">Edit User Security Settings</li>
	</ol>
	<br />
	<div class="panel panel-default">
		<div class="panel-heading">
			<h3 class="panel-title">Change Password</h3>
		</div>
		<div class="panel-body">
			<table runat="server" class="loginTable">
				<tr>
					<td><strong>Username:</strong></td>
					<td>
						<strong><asp:Label ID="LoggedInUsername" runat="server" /></strong></td>
					<td>
					</td>
				</tr>
				<tr>
					<td><strong>Old Password:</strong></td>
					<td>
						<asp:TextBox ID="OldUserPass" CssClass="pwdfield form-control" TextMode="Password" runat="server" />
					</td>
					<td>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator2"
							ControlToValidate="OldUserPass"
							ErrorMessage="Cannot be empty."
							runat="server" />
					</td>
				</tr>
				<tr>
					<td><strong>New Password:</strong></td>
					<td>
						<asp:TextBox ID="NewUserPass" CssClass="pwdfield form-control" TextMode="Password" runat="server" />
					</td>
					<td>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator3"
							ControlToValidate="NewUserPass"
							ErrorMessage="Cannot be empty."
							runat="server" />
					</td>
				</tr>
				<tr>
					<td><strong>Confirm New:</strong></td>
					<td>
						<asp:TextBox ID="NewUserPassCfm" CssClass="pwdfield form-control" TextMode="Password" runat="server" />
					</td>
					<td>
						<asp:RequiredFieldValidator
							ID="RequiredFieldValidator4"
							ControlToValidate="NewUserPassCfm"
							ErrorMessage="Cannot be empty."
							runat="server" />
					</td>
				</tr>
			</table>
			<asp:Button ID="ChangePWButton"
				Text="Change Password"
				CssClass="btn btn-primary loginBtn"
				OnClick="ChangePassClick"
				runat="server" />
		</div>
	</div>


	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" />
	</p>
</asp:Content>