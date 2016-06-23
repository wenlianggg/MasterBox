<%@ Page Title="" Language="C#" MasterPageFile="~/mbox/FileSetting.master" AutoEventWireup="true" CodeBehind="FileSetting_Security.aspx.cs" Inherits="MasterBox.mbox.FileSetting_Security" %>

<asp:Content ID="SetSecurity" ContentPlaceHolderID="Child_Profile" runat="server">

    <div id="FolderChangePsss" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Folder Password Option</h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="FolderPasswordOptionLabel" runat="server" Text="Choose Folder: "></asp:Label>
                    <asp:DropDownList ID="FolderPasswordOption" runat="server">
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
                    <asp:TextBox ID="NewPassword" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="NewPassValid" runat="server" 
                        ValidationGroup="FolderPasswordChangeValidation"
                        ValidateEmptyText="true"
                        ControlToValidate="NewPassword"
                        ErrorMessage="Cannot be empty"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="FolderCfmPassword" runat="server" Text="Confirm Password: "></asp:Label>                    
                    <asp:TextBox ID="CfmPassword" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CfmPassValid" runat="server"
                        ValidationGroup="FolderPasswordChangeValidation"
                        ValidateEmptyText="true"
                        ControlToValidate="CfmPassword"
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
        <h1>Security</h1>
        <hr class="aboutRowHR" />
        <div class="row">
            <div class="SettingsRow">
                <h4 class="SettingHr">User Password Settings</h4>
                <a href="../Auth/changepw.aspx">Change Password</a>
            </div>
            <div class="SettingsRow">
                <h4 class="SettingHr">Folder Password Settings</h4>
                <a data-toggle="modal" data-target="#FolderChangePsss" data-backdrop="static">Change Password</a>
            <asp:Label ID="testing" runat="server"></asp:Label>
            </div>
        </div>


    </div>
</asp:Content>
