﻿<%@ Page Title="Login Authentication" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signin.aspx.cs" Inherits="MasterBox.SignIn" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
	<link href="<%= ResolveUrl("~/Auth/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="LoginIn" ContentPlaceHolderID="MainContent" runat="server">
	<h2><%: Title %></h2>
	<p>Please login or register to access our features.</p>
	<table runat="server" class="loginTable">
		<tr>
			<td><strong>E-mail address:</strong></td>
			<td>
				<asp:TextBox ID="UserEmail" runat="server" CssClass="form-control" /></td>
			<td>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="UserEmail"
					Display="Dynamic" ErrorMessage="Cannot be empty." runat="server" />
			</td>
		</tr>
		<tr>
			<td><strong>Password:</strong></td>
			<td>
				<asp:TextBox ID="UserPass" CssClass="pwdfield form-control" TextMode="Password" runat="server" />
			</td>
			<td>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="UserPass"
					ErrorMessage="Cannot be empty." runat="server" />
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
	<asp:Button ID="LoginButton" Text="Sign In" CssClass="btn btn-default-blue loginBtn" OnClick="logonClick" runat="server" />
	<p>
		<asp:Label ID="Msg" ForeColor="red" runat="server" />
	</p>
	<script>
		function registerMode(val) {
			if (val == 'true') {
				$(".loginBtn").prop('value', "Register");
				$(".pwdfield").attr('readonly', "readonly");
				$(".pwdfield").attr('disabled', "disabled");
			} else {
				$(".loginBtn").prop('value', "Sign In");
				$(".pwdfield").removeAttr('readonly');
				$(".pwdfield").removeAttr('disabled');
			}
			console.log("CHANGED: " + bool);
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
