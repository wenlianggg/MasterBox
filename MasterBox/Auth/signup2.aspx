<%@ Page Title="User Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signup2.aspx.cs" Inherits="MasterBox.SignUp2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<link href="<%= ResolveUrl("~/CSS/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
	<script src='https://www.google.com/recaptcha/api.js'></script>
</asp:Content>
<asp:Content ID="Register" ContentPlaceHolderID="MainContent" runat="server">
	<div class="jumbotron">
		<h1><%: Title %></h1>
		<p>Please login or register to access our features.</p>
	</div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
		<li><a href="<%= ResolveUrl("~/Auth/signup") %>">Registration</a></li>
		<li class="active">Confirmation</li>
	</ol>
	<br />
	<div class="panel panel-default">
		<div class="panel-heading">
			<h3 class="panel-title">Registration Successful</h3>
		</div>
	</div>

	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" />
	</p>
</asp:Content>
