<%@ Page Title="" Language="C#" MasterPageFile="~/mbox/FileSetting.master" AutoEventWireup="true" CodeBehind="FileSetting_Security.aspx.cs" Inherits="MasterBox.mbox.FileSetting_Security" %>

<asp:Content ID="SetSecurity" ContentPlaceHolderID="Child_Profile" runat="server">

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
                    <asp:DropDownList ID="NewFolderPasswordOption" runat="server">
                    </asp:DropDownList>
                    <br />
                    <br />
                    <asp:Label ID="NewFolderPasswordLabel" runat="server" Text="New Password: "></asp:Label>
                    <asp:TextBox ID="NewPassword" TextMode="Password" runat="server" CssClass="pwdfield form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="NewPasswordValidation" runat="server" 
                        ValidationGroup="NewFolderPasswordChangeValidation"
                        ValidateEmptyText="true"
                        ControlToValidate="NewPassword"
                        ErrorMessage="Cannot be empty"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
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
                    <asp:DropDownList ID="ChangeFolderPasswordOption" runat="server">
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
                    <asp:RequiredFieldValidator ID="NewPassValid" runat="server" 
                        ValidationGroup="FolderPasswordChangeValidation"
                        ValidateEmptyText="true"
                        ControlToValidate="ChangeNewPassword"
                        ErrorMessage="Cannot be empty"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
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
        <h1>Security</h1>
        <hr class="aboutRowHR" />
        <div class="row">
            <div class="SettingsRow">
                <h4 class="SettingHr">User Password Settings</h4>
                <a class="btn btn-default" href="../Auth/changepw.aspx">Change Password</a>
            </div>
            <div class="SettingsRow">
                <h4 class="SettingHr">Folder Password Settings</h4>
                <a class="btn btn-default" data-toggle="modal" data-target="#FolderNewPass" data-backdrop="static">New Password</a>
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
