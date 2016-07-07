<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/prefs/Preferences.Master" AutoEventWireup="true" CodeBehind="prefpassword.aspx.cs" Inherits="MasterBox.ChangePw" %>

<asp:Content ID="LoginIn" ContentPlaceHolderID="Preferences" runat="server">
        <div class="page-header">
          <h1><%: Page.Title %>
              <small>Change your password periodically for security</small>
          </h1>
        </div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
		<li>User Preferences</li>
		<li>Security</li>
		<li>Authentication</li>
		<li class="active">Change Password</li>
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