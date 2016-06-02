<%@ Page Title="Login Authentication" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signin.aspx.cs" Inherits="MasterBox.SignIn" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
	<link href="<%= ResolveUrl("~/Auth/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="LoginIn" ContentPlaceHolderID="MainContent" runat="server">
	<h2><%: Title %></h2>
	<p>Please login or register to access our features.</p>
	<table runat="server" class="loginTable">
		<tr>
			<td><strong>Username:</strong></td>
			<td>
				<asp:TextBox ID="UserName" runat="server" CssClass="form-control" /></td>
			<td>
				<asp:RequiredFieldValidator 
					ID="RequiredFieldValidator1" 
					ControlToValidate="UserName"
					Display="Dynamic" 
					ErrorMessage="Cannot be empty." 					
					ValidationGroup="valGroup1"
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
			<td><strong>Remember me?</strong></td>
			<td>
				<asp:CheckBox ID="Persist" runat="server" />
			</td>
		</tr>
		<tr>
			<td><strong>I am a...</strong></td>
			<td>
				<div class="radio">
					<asp:RadioButtonList runat="server" RepeatLayout="flow">
						<asp:ListItem Text=" Returning User" Value="false" Selected="True" />
						<asp:ListItem Text=" New User" Value="true" />
					</asp:RadioButtonList>
				</div>
			</td>
		</tr>
	</table>
	<asp:Button ID="LoginButton" 
				Text="Sign In" 
				CssClass="btn btn-default-blue loginBtn" 
				OnClick="logonClick" 
				runat="server" />
	<asp:Button ID="RegisterButton" 
				Text="Sign Up"
				validationGroup="valGroup1"
				CssClass="btn btn-default regBtn" 
				OnClick="registrationStart"
				runat="server" />

	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" />
	</p>
	<script>
		function registerMode(val) {
			if (val == 'true') {
				$(".loginBtn").hide();
				$(".regBtn").show();
				$(".loginBtn").prop('value', "Register");
				$(".pwdfield").attr('readonly', "readonly");
				$(".pwdfield").attr('disabled', "disabled");
			} else {
				$(".regBtn").hide();
				$(".loginBtn").show();
				$(".loginBtn").prop('value', "Sign In");
				$(".pwdfield").removeAttr('readonly');
				$(".pwdfield").removeAttr('disabled');
			}
			console.log("Register Mode: " + bool);
		}
		$(function () {
			$("input[type='radio']").on('click', function (e) {
				getCheckedRadio($(this).attr("name"), $(this).val(), this.checked);
			});
		});

		function getCheckedRadio(group, item, value) {
			registerMode(item);
		}
	</script>
</asp:Content>
