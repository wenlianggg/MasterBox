<%@ Page Title="Accounts Management" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="usermgmt.aspx.cs" Inherits="MasterBox.Admin.UserMgmt" %>
<asp:Content ID="UserMgmtContent" ContentPlaceHolderID="AdminPanelContentPH" runat="server">
    <br /><br /><br />
	<h3>Users List</h3>
	<asp:GridView runat="server"
		ID="userstable"
		CssClass="table table-striped table-bordered"
		emptydatatext="No data available." />

</asp:Content>
