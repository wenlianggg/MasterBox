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
                    <asp:Label ID="FolderCurrectPassword" runat="server" Text="Current Password: "></asp:Label>
                    <asp:TextBox ID="CurrentPassword" TextMode="Password" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CurrentPassValid" runat="server" 
                        ControlToValidate="CurrentPassword"
                        ErrorMessage="Please Fill in this"
                        ></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="FolderNewPassword" runat="server" Text="New Password: "></asp:Label>
                    <asp:TextBox ID="NewPassword" TextMode="Password" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="NewPassValid" runat="server" ControlToValidate="NewPassword"></asp:RequiredFieldValidator>
                    <br />
                    <asp:Label ID="FolderCfmPassword" runat="server" Text="Confirm Password: "></asp:Label>                    
                    <asp:TextBox ID="CfmPassword" TextMode="Password" runat="server" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="CfmPassValid" runat="server" ControlToValidate="CfmPassword"></asp:RequiredFieldValidator>
                </div>
                <div class="modal-footer">
                 
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
            </div>
        </div>


    </div>
</asp:Content>
