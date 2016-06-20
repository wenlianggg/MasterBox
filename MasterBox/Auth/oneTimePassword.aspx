﻿<%@ Page Title="OTP Authentication" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="oneTimePassword.aspx.cs" Inherits="MasterBox.Auth.oneTimePassword" %>
<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
	<link href="<%= ResolveUrl("~/Auth/LoginStyle.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="OneTime" ContentPlaceHolderID="MainContent" runat="server">
    	<div class="jumbotron">
		<h1><%:Title %></h1>
		<p>Please enter your One Time Password to continue.</p>
	</div>
    	<ol class="breadcrumb" style="margin-bottom: 5px;">
		<li><a href="<%= ResolveUrl("~/Auth/signin") %>">Authentication</a></li>
        <li><a href="<%= ResolveUrl("~/Auth/signin") %>">Login</a></li>
		<li class="active">One Time Password</li>
	</ol>
    <br />

	<div class="panel panel-default">
        <div class="panel-heading">
			<h3 class="panel-title">One Time Password</h3>
		</div>
        <div class="panel-body">
           <table class="otpTable">
               <tr class="otpRow">
                   <td>OTP:</td>
                   <td>
                       <asp:TextBox ID="OTPValue" runat="server" CssClass="form control"></asp:TextBox>
                       <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
                          ControlToValidate="OTPValue"
                          ValidatorGroup="valGroup1"
                          ErrorMessage="Please Enter Your OTP to Continue."
                          ForeColor="Red">
                        </asp:RequiredFieldValidator>
                   </td>
               </tr>
               <tr>
                   <td>
                       <asp:Button class="otpLogin" ID="OTPLogin" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="ConfirmOTP"/>
                          <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
                          ControlToValidate="OTPValue"
                          ValidatorGroup="valGroup1">
                        </asp:RequiredFieldValidator>
                   </td>
                   <td>
                       <asp:Button class="otpCancel" ID="OTPCancel" runat="server" Text="Cancel" CssClass="btn btn-danger" OnClick="CancelOTP" CausesValidation="false"/>
                   </td>
               </tr>
           </table>
    </div>
    </div>
</asp:Content>
