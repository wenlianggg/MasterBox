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
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                    <br />
                    <asp:CompareValidator runat="server" ControlToCompare="NewCfmPassword" ControlToValidate="NewPassword"
                        CssClass="field-validation-error" ForeColor="Red" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match."
                        ValidationGroup="NewFolderPasswordChangeValidation" />
                </div>
                <div class="modal-footer">
                    <asp:Button ID="NewFolderPassword" runat="server" ValidationGroup="NewFolderPasswordChangeValidation" Text="Create" OnClick="NewFolderPassword_Click" />
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
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                    <br />
                    <asp:CompareValidator runat="server" ControlToCompare="ChangeCfmPassword" ControlToValidate="ChangeNewPassword"
                        CssClass="field-validation-error" ForeColor="Red" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match."
                        ValidationGroup="FolderPasswordChangeValidation" />
                </div>
                <div class="modal-footer">
                    <asp:Button ID="ChangeFolderPassword" runat="server" ValidationGroup="FolderPasswordChangeValidation" Text="Change Password" OnClick="ChangeFolderPassword_Click" />
                </div>
            </div>
        </div>
    </div>

    <div id="FolderDeletePass" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Delete Folder Password Option</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="DeleteFolderPasswordOptionLbl" runat="server" Text="Choose Folder: "></asp:Label>
                    <asp:DropDownList ID="DeleteFolderPasswordOption" CssClass="form-control" runat="server">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <asp:Label ID="FolderCurrentDeletePasswordLbl" runat="server" Text="Current Password: "></asp:Label>
                    <asp:TextBox ID="FolderCurrentDeleteTxtBox" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CurrentDeleteValidator" runat="server"
                        ValidationGroup="FolderPasswordDeleteValidation"
                        ValidateEmptyText="true"
                        ControlToValidate="FolderCurrentDeleteTxtBox"
                        ErrorMessage="Cannot be empty"
                        ForeColor="Red"
                        CssClass="field-validation-error">
                    </asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="CfmCurrentDeleteLbl" runat="server" Text="Confirm Password: "></asp:Label>
                    <asp:TextBox ID="CfmCurrentDeleteTxtBox" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CurrentCfmDeleteValidator" runat="server"
                        ValidationGroup="FolderPasswordDeleteValidation"
                        ValidateEmptyText="true"
                        ControlToValidate="CfmCurrentDeleteTxtBox"
                        ErrorMessage="Cannot be empty"
                        ForeColor="Red"
                        CssClass="field-validation-error">
                    </asp:RequiredFieldValidator>
                    <br />
                    <asp:CompareValidator runat="server" ControlToCompare="CfmCurrentDeleteTxtBox" ControlToValidate="FolderCurrentDeleteTxtBox"
                        CssClass="field-validation-error" ForeColor="Red" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match."
                        ValidationGroup="FolderPasswordDeleteValidation" />

                </div>
                <div class="modal-footer">
                    <asp:Button ID="DeleteFolderPassword" runat="server" ValidationGroup="FolderPasswordDeleteValidation" Text="Change Password" OnClick="DeleteFolderPassword_Click" />
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
                <h3 class="panel-title">Folder and Files Security
                </h3>
            </div>
            <div class="panel-body">
                <strong>Change folder password options</strong><br />
                <p>You can choose to either: </p>
                <ol>
                    <li>Create a new password for folder</li>
                    <li>Change an old password for folder</li>
                    <li>Delete a existing password for folder</li>
                </ol>
                <a class="btn btn-default" data-toggle="modal" data-target="#FolderNewPass" data-backdrop="static">Set New Password</a>
                <a class="btn btn-default" data-toggle="modal" data-target="#FolderChangePass" data-backdrop="static">Change Password</a>
                <a class="btn btn-default" data-toggle="modal" data-target="#FolderDeletePass" data-backdrop="static">Delete Password</a>
            </div>
        </div>
    </div>

</asp:Content>
