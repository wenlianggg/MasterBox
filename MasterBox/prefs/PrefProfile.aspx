<%@ Page Language="C#" Title="Profile Preferences" MasterPageFile="~/prefs/Preferences.Master" AutoEventWireup="true" CodeBehind="prefprofile.aspx.cs" Inherits="MasterBox.Prefs.FileSetting_Profile" %>

<asp:Content ID="SetProfile" ContentPlaceHolderID="Preferences" runat="server">
		<div class="page-header">
			<h1><%: Page.Title %>
			</h1>
		</div>
		<ol class="breadcrumb" style="margin-bottom: 5px;">
			<li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
			<li>User Preferences</li>
			<li class="active"><%: Page.Title %></li>
		</ol>
		<br />
		<div class="row">
			<div class="col-xs-12 col-sm-6 col-md-8">
				<div class="row" style="padding: 2%;">
					<h4 class="SettingHr">Account Details</h4>
					<table>
						<tr>
							<td>
								<label>Username: </label>
							</td>
							<td>
								<asp:TextBox CssClass="form-control" ID="username" runat="server" Font-Size="Medium"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td>
								<label>E-mail: </label>
							</td>
							<td>
								<asp:TextBox CssClass="form-control" ID="email" runat="server" Font-Size="Medium"></asp:TextBox>
							</td>
						</tr>
					</table>
				</div>
                </div>
				<div class="row">
					<h4 class="SettingHr">Preference</h4>
					<h5>Email Notifications</h5>
					<table class="table_pref">
						<tr>
							<td>
								<input type="checkbox" /></td>
							<td>My MasterBox is almost out of space</td>
						</tr>
						<tr>
							<td>
								<input type="checkbox" /></td>
							<td>Many files deleted from my MasterBox</td>
						</tr>
						<tr>
							<td>
								<input type="checkbox" /></td>
							<td>MasterBox newsletters</td>
						</tr>
						<tr>
							<td>
								<input type="checkbox" /></td>
							<td>MasterBox activity digests</td>
						</tr>
						<tr>
							<td>
								<input type="checkbox" /></td>
							<td>Invitations to give MasterBox feedback</td>
						</tr>
					</table>
				</div>



			</div>

			<div class="col-xs-6 col-md-4">
				<div class="prof_icon">
					Profile Icon
                     
				</div>
				<input type="file" />

			</div>
		<div class="row">
			<button type="submit">Update</button>
		</div>

</asp:Content>


