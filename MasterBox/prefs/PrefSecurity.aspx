﻿<%@ Page Title="Folder Encryption" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="prefsecurity.aspx.cs" Inherits="MasterBox.Prefs.FileSetting_Security" %>

<asp:Content ID="SetSecurity" ContentPlaceHolderID="Preferences" runat="server">

    <div id="FolderNewPass" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">New Folder Password Option</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="NewFolderPasswordOptionLabel" runat="server" Text="Choose Folder: "></asp:Label>
                    <asp:DropDownList ID="NewFolderPasswordOption" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <asp:Label ID="NewFolderPasswordLabel" runat="server" Text="New Password: "></asp:Label>
                    <asp:TextBox ID="NewPassword" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="NewPasswordValid" runat="server" 
                        ControlToValidate="NewPassword" 
                        ValidationGroup="NewFolderPasswordChangeValidation"
                         ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$" 
                        ErrorMessage="Password must contain: Minimum 8 characters at least 1 Alphabet and 1 Number" 
                        ForeColor="Red" />
                    <br />
                    <asp:Label ID="NewFolderCfmPasswordLabel" runat="server" Text="Confirm Password: "></asp:Label>                    
                    <asp:TextBox ID="NewCfmPassword" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="NewCfmPasswordValidation" runat="server"
                        ValidationGroup="NewFolderPasswordChangeValidation"
                        ValidateEmptyText="true"
                        ControlToValidate="NewCfmPassword"
                        ErrorMessage="Cannot be empty"
                        ForeColor="Red" >
                    </asp:RequiredFieldValidator>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="NewFolderPassword" runat="server" ValidationGroup="NewFolderPasswordChangeValidation" Text="Create" OnClick="NewFolderPassword_Click"/>
                </div>
            </div>
        </div>
    </div>

    <div id="FolderChangePass" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Change Folder Password Option</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="ChangeFolderPasswordOptionLabel" runat="server" Text="Choose Folder: "></asp:Label>
                    <asp:DropDownList ID="ChangeFolderPasswordOption" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <asp:Label ID="FolderCurrectPassword" runat="server" Text="Current Password: "></asp:Label>
                    <asp:TextBox ID="CurrentPassword" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CurrentPasswordValid" runat="server"
                            ValidationGroup="FolderPasswordChangeValidation"
                            ValidateEmptyText="true"
                            ControlToValidate="CurrentPassword"
                            ErrorMessage="Cannot be empty"
                            ForeColor="Red">
                        </asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="FolderNewPassword" runat="server" Text="New Password: "></asp:Label>
                    <asp:TextBox ID="ChangeNewPassword" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="ChangeNewPassValid" runat="server" 
                        ControlToValidate="ChangeNewPassword" 
                        ValidationGroup="FolderPasswordChangeValidation"
                         ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$" 
                        ErrorMessage="Password must contain: Minimum 8 characters at least 1 Alphabet and 1 Number" 
                        ForeColor="Red" />
                    <br />
                    <asp:Label ID="FolderCfmPassword" runat="server" Text="Confirm Password: "></asp:Label>                    
                    <asp:TextBox ID="ChangeCfmPassword" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CfmPassValid" runat="server"
                        ValidationGroup="FolderPasswordChangeValidation"
                        ValidateEmptyText="true"
                        ControlToValidate="ChangeCfmPassword"
                        ErrorMessage="Cannot be empty"
                        ForeColor="Red" >
                    </asp:RequiredFieldValidator>
                </div>
                <div class="modal-footer">
                 <asp:Button ID="ChangeFolderPassword" runat="server" ValidationGroup="FolderPasswordChangeValidation" Text="Change Password" OnClick="ChangeFolderPassword_Click"/>
                </div>
            </div>
        </div>
    </div>

    <div class="Setting_Profile">
		<div class="page-header">
			<h1><%: Page.Title %>
                <small>Secure files using our industry-grade encryption</small>
			</h1>
		</div>
		<ol class="breadcrumb" style="margin-bottom: 5px;">
			<li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
			<li>User Preferences</li>
			<li>Security</li>
			<li>Encryption</li>
			<li class="active"><%: Page.Title %></li>
		</ol>
        <div class="row">
            </div>
        <div class="panel panel-primary">
		    <div class="panel-heading">
			    <h3 class="panel-title">
                    Folder and Files Security
			    </h3>
		    </div>
		    <div class="panel-body">
                <a class="btn btn-default" data-toggle="modal" data-target="#FolderNewPass" data-backdrop="static">Set New Password</a>
                <a class="btn btn-default" data-toggle="modal" data-target="#FolderChangePass" data-backdrop="static">Change Password</a>
                <a class="btn btn-default" data-toggle="modal" data-target="#FolderDeletePass" data-backdrop="static">Delete Password</a>
		    </div>
        </div>
    </div>
<script>
    // To validate New Confirm Password
        $(document).ready(function () {
            $('#<%=NewFolderPassword.ClientID %>').click(function (event) {
                var pass = document.getElementById('<%=NewPassword.ClientID%>').value;
                var passcfm = document.getElementById('<%=NewCfmPassword.ClientID%>').value
                if (pass != passcfm) {
                    alert('Confirm Password Again')
                    return false;
                } else {
                    return true;
                }

            });
        });

    // To validate Change Confirm Password
        $(document).ready(function () {
            $('#<%=ChangeFolderPassword.ClientID %>').click(function (event) {
                var pass = document.getElementById('<%=ChangeNewPassword.ClientID%>').value;
                var passcfm = document.getElementById('<%=ChangeCfmPassword.ClientID%>').value
                if (pass != passcfm) {
                    alert('Confirm Password Again')
                    return false;
                } else {
                    return true;
                }

            });
        });

</script>
</asp:Content>
