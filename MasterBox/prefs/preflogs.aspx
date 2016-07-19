<%@ Page Title="User Logs" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="preflogs.aspx.cs" Inherits="MasterBox.Prefs.AccessLog" %>

<asp:Content ID="Logs" ContentPlaceHolderID="Preferences" runat="server">
        <div class="page-header">
          <h1><%: Page.Title %>
              <small>Periodically review your user logs for security</small>
          </h1>
        </div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
		<li>User Preferences</li>
		<li>Security</li>
		<li class="active"><%: Page.Title %></li>
	</ol>
	<br />
    <div class="panel panel-default">
        <div class="panel-heading">
		    <h3 class="panel-title">Select Log to Show</h3>
	    </div>
        <div class="panel-body">
            <asp:Button ID="RefreshAuth"
            Text="Access Logs"
            CssClass="btn btn-default"
            OnClick="RefreshAuthTable"
            runat="server" />
            <asp:Button ID="RefreshFiles"
            Text="File Logs"
            CssClass="btn btn-default"
            OnClick="RefreshFilesTable"
            runat="server" />
            <asp:Button ID="RefreshTransact"
            Text="Transaction Logs"
            CssClass="btn btn-default"
            OnClick="RefreshFilesTable"
            runat="server" />
        </div>
    </div>

	<div class="panel panel-primary">
		<div class="panel-heading">
			<h3 class="panel-title">
            <asp:Label 
                runat="server" 
                ID="LogTypeName" 
                Text="Logs View"/>
			</h3>
		</div>
		<div class="panel-body">
            <asp:GridView 
                runat="server" 
                ID="LogTable" 
                CssClass="table table-condensed" 
                OnRowDataBound="LogsRowDataBound"
                EmptyDataText="There are no entries of this log type."/>
		</div>
	</div>
    	<div class="panel panel-info">
		<div class="panel-heading">
			<h3 class="panel-title">
            Log Legend
            </h3>
		</div>
		<div class="panel-body">
            <ul class="list-group">
              <li class="list-group-item" style="background-color:azure">Standard log entry, no red flags!</li>
              <li class="list-group-item" style="background-color:lightyellow">Something security related was accessed.</li>
              <li class="list-group-item" style="background-color:tomato">Something was changed! Please take note.</li>
              <li class="list-group-item" style="background-color:dodgerblue">Client error log entry, please take note!</li>
              <li class="list-group-item" style="background-color:steelblue">Server error log entry, please take note!</li>
            </ul>
		</div>
        </div>
	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" />
	</p>
</asp:Content>
