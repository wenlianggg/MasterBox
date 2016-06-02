<%@ Page Title="Registration" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="MasterBox.SignUp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Auth/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
    <script src='https://www.google.com/recaptcha/api.js'></script>
</asp:Content>
<asp:Content ID="Register" ContentPlaceHolderID="MainContent" runat="server">
    <h2>User Registration</h2>
	<p></p>
    <ol class="breadcrumb" style="margin-bottom: 5px;">
        <li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
        <li><a href="#">Registration</a></li>
    <li class="active">Step 1</li>
    </ol>
	<table runat="server" class="regTable">
		<tr>
			<td><strong>Username: </strong></td>
			<td>
				<asp:TextBox ID="UserName" runat="server" CssClass="form-control" /></td>
			<td>
				<asp:RequiredFieldValidator 
					ID="RequiredFieldValidator1" 
					ControlToValidate="UserName"
					ErrorMessage="Cannot be empty." 					
              		runat="server" />
			</td>
		</tr>
		<tr>
			<td><strong>Password:</strong></td>
			<td>
				<asp:TextBox ID="UserPass" CssClass="pwdfield form-control" TextMode="Password" runat="server" />
			</td>
			<td>
				<asp:RequiredFieldValidator 
					ID="RequiredFieldValidator2"
					ControlToValidate="UserPass"
					ErrorMessage="Cannot be empty."
					runat="server" />
			</td>
		</tr>
		<tr>
			<td><strong>Confirm Password:</strong></td>
			<td>
                <asp:TextBox ID="UserPassCfm" CssClass="pwdfield form-control" TextMode="Password" runat="server" />
			</td>
            <td>
                	<asp:RequiredFieldValidator 
					ID="RequiredFieldValidator3"
					ControlToValidate="UserPassCfm"
					ErrorMessage="Cannot be empty."
					runat="server" />
            </td>
		</tr>
		<tr>
			<td><strong>E-mail Address:</strong></td>
			<td>
                <asp:TextBox ID="UserEmail" CssClass="form-control" runat="server" />
			</td>
            <td>
                   <asp:RequiredFieldValidator 
					ID="RequiredFieldValidator4"
					ControlToValidate="UserEmail"
					ErrorMessage="Cannot be empty."
					runat="server" />
            </td>
		</tr>
	</table>
    <div class="g-recaptcha" data-sitekey="6Ld6kiETAAAAAMplec1OuKhJ3VKCBOhZmOcAkZsg"></div>
	<br />
    <asp:Button ID="RegisterButton" 
				Text="Sign Up"
				CssClass="btn btn-default" 
				OnClick="processRegistration"
				runat="server" />
        
	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" />
	</p>  
</asp:Content>