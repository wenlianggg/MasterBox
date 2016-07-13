<%@ Page Title="Access Logs" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="preflogs.aspx.cs" Inherits="MasterBox.Auth.AccessLog" %>

<asp:Content ID="AccessLogs" ContentPlaceHolderID="Preferences" runat="server">
        <div class="page-header">
          <h1><%: Page.Title %>
              <small>Periodically review your access logs for security</small>
          </h1>
        </div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
		<li>User Preferences</li>
		<li>Security</li>
		<li class="active"><%: Page.Title %></li>
	</ol>
	<br />
    <div class="panel panel-primary">
        <div class="panel-heading">
		    <h3 class="panel-title">Log Controls</h3>
	    </div>
        <div class="panel-body">
            <asp:Button ID="RefreshAuth"
            Text="Load Access Logs"
            CssClass="btn btn-default loginBtn"
            OnClick="RefreshAuthTable"
            runat="server" />
            <asp:Button ID="RefreshFiles"
            Text="Load File Logs"
            CssClass="btn btn-default loginBtn"
            OnClick="RefreshFilesTable"
            runat="server" />
        </div>
    </div>

	<div class="panel panel-default" runat="server" ID="AuthLogs" visible="false">
		<div class="panel-heading">
			<h3 class="panel-title">Access Logs</h3>
		</div>
		<div class="panel-body">
            <asp:GridView runat="server" ID="AuthLogsTable" CssClass="table table-striped table-condensed"/>
		</div>
	</div>
    <div class="panel panel-default" runat="server" ID="FileLogs" visible="false">
		<div class="panel-heading">
			<h3 class="panel-title">File Logs</h3>
		</div>
		<div class="panel-body">
            <asp:GridView runat="server" ID="FileLogsTable" CssClass="table table-striped table-condensed"/>
		</div>
	</div>
	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" />
	</p>
</asp:Content>
