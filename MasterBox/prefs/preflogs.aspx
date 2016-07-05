<%@ Page Title="Access Logs" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="preflogs.aspx.cs" Inherits="MasterBox.Auth.AccessLog" %>

<asp:Content ID="AccessLogs" ContentPlaceHolderID="Preferences" runat="server">
        <div class="page-header">
          <h1><%: Page.Title %>
              <small>Periodically review your access logs for security</small>
          </h1>
        </div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
        <li>Security</li>
		<li class="active">Access Logs</li>
	</ol>
	<br />
	<div class="panel panel-default">
		<div class="panel-heading">
			<h3 class="panel-title">Access Logs</h3>
		</div>
		<div class="panel-body">
            <asp:GridView runat="server" ID="LogsTable" CssClass="table table-striped table-condensed"/>
            <asp:Button ID="RefreshButton"
            Text="Refresh Logs"
            CssClass="btn btn-primary loginBtn"
            OnClick="RefreshTable"
            runat="server" />
		</div>
	</div>


	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" />
	</p>
</asp:Content>
