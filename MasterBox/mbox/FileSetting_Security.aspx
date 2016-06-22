<%@ Page Title="" Language="C#" MasterPageFile="~/mbox/FileSetting.master" AutoEventWireup="true" CodeBehind="FileSetting_Security.aspx.cs" Inherits="MasterBox.mbox.FileSetting_Security" %>

<asp:Content ID="SetSecurity" ContentPlaceHolderID="Child_Profile" runat="server">

        <!--Upload Modal -->
        <div id="changePassword" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Change Password</h4>
                    </div>
                    <div class="modal-body">
                        
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
                <h4 class="SettingHr">Password Settings</h4>
                <a data-toggle="modal" data-target="#changePassword" data-backdrop="static">Change Password</a>
            </div>
        </div>


    </div>
</asp:Content>
