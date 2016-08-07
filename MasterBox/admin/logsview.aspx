<%@ Page Title="Audit Logs" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="logsview.aspx.cs" Inherits="MasterBox.Admin.LogsView" %>
<asp:Content ID="UserMgmtContent" ContentPlaceHolderID="AdminPanelContentPH" runat="server">
    <br /><br /><br />
	<h3>All Logs</h3>
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
              <li class="list-group-item" style="background-color:plum">Something was changed! Please take note.</li>
              <li class="list-group-item" style="background-color:dodgerblue">Client error log entry, please take note!</li>
              <li class="list-group-item" style="background-color:steelblue">Server error log entry, please take note!</li>
            </ul>
		</div>
       </div>
	<asp:GridView runat="server"
		ID="LogsGridView"
		CssClass="table table-striped table-bordered"
		emptydatatext="No data available."
		OnRowDataBound="LogsGridView_DataBound" />

</asp:Content>
