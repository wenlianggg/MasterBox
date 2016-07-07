<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="signin.aspx.cs" Inherits="MasterBox.Auth.SignIn" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
	<link href="<%= ResolveUrl("~/CSS/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="LoginIn" ContentPlaceHolderID="MainContent" runat="server">
	<div class="jumbotron">
		<h1><%: Title %></h1>
		<p>Welcome to MasterBox, please login or register to access our features!</p>
	</div>
	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
		<li class="active">Login</li>
	</ol>
	<br />
	<div class="panel panel-default">
		<div class="panel-heading">
			<h3 class="panel-title">Login</h3>
		</div>
		<div class="panel-body">
			<table runat="server" class="loginTable">
				<tr>
					<td><strong>Username:</strong></td>
					<td>
						<asp:TextBox ID="UserName" runat="server" CssClass="form-control" /></td>
					<td>
						<asp:RequiredFieldValidator
							ID="UserNameValidator"
							ControlToValidate="UserName"
							Display="Dynamic"
							ErrorMessage="*"
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
							ID="UserPassValidator"
							ControlToValidate="UserPass"
							ErrorMessage="*"
							runat="server" />
					</td>
				</tr>
				<tr>
					<td></td>
					<td>
						<asp:CheckBox ID="Persist" runat="server" />
						<strong>Remember Login</strong>
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
				CssClass="btn btn-primary loginBtn"
				OnClick="logonClick"
				runat="server" />
			<asp:Button ID="RegisterButton"
				Text="Sign Up"
				ValidationGroup="valGroup1"
				CssClass="btn btn-success regBtn"
				OnClick="Registration_Start"
				runat="server" />
		</div>
	</div>


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
