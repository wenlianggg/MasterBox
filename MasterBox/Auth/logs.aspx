<%@ Page Title="Access Logs" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="logs.aspx.cs" Inherits="MasterBox.Auth.AccessLog" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
	<link href="<%= ResolveUrl("~/Auth/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="LoginIn" ContentPlaceHolderID="MainContent" runat="server">
	<div class="jumbotron">
		<h1><%: Title %></h1>
		<p>Welcome to MasterBox, please login or register to access our features!</p>
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
