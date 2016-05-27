<%@ Page Language="C#" Title="" MasterPageFile="~/Internal.Master" AutoEventWireup="true" CodeBehind="FileSettingInterface.aspx.cs" Inherits="MasterBox.FileSettingInterface" %>

<asp:Content ID="Testing" ContentPlaceHolderID="InternalContent" runat="server">
    <div class="MainContent">
        <div class="SettingsPanel"> 
            <h3>Application Settings</h3>
            <ul>
                <li><a runat="server" href="" class="active">Profile</a></li>
                <li><a>General</a></li>
                <li><a>Security</a></li>
                <li><a>Upgrade</a></li>
            </ul>


        </div>
        <div class="SettingSelectionPanel"> 
             <div id="SettingProfile">
                 <h1>Profile</h1>
             </div>
        </div>


    </div>
</asp:Content>


