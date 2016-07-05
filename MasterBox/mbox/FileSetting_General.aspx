<%@ Page Title="" Language="C#" MasterPageFile="~/mbox/FileSetting.master" AutoEventWireup="true" CodeBehind="FileSetting_General.aspx.cs" Inherits="MasterBox.mbox.FileSetting_General" %>

<asp:Content ID="SetGeneral" ContentPlaceHolderID="Child_Profile" runat="server">
    <div class="Setting_Profile">
        <h1>General Information</h1>
        <hr class="aboutRowHR"/>
        <div class="row">
            <div class="SettingsRow">
            <h4 class="SettingHr">Storage Space</h4>
            <div class="progress">
                <div class="progress-bar" role="progressbar" aria-valuenow="70"
                    aria-valuemin="0" aria-valuemax="100" style="width: 20%">
                    2.7MB
                </div>
                </div>
                    <a CssClass="btn btn-primary" runat="server" href="~/Auth/changepw.aspx">Change Password</a>
                </div>
            <hr class="aboutRowHR" />
            <div class="SettingsRow">
            <h4 class="SettingHr">Miscellaneous</h4>
            <table>
                <tr>
                    <td>Date Format</td>
                    <td>
                        <select>
                            <option>DD/MM/YYYY</option>
                            <option>MM/DD/YYYY</option>
                            <option>YYYY/MM/DD</option>

                        </select>
                    </td>
                </tr>
            </table>
                </div>
            <hr class="aboutRowHR" />
            <div class="SettingsRow">
            <h4 class="SettingHr">Connections</h4>
            <table>
                <tr>
                    <td>G-mail: </td>
                    <td><input type="email" /></td>
                </tr>
                <tr>
                    <td>Yahoo mail: </td>
                    <td><input type="email" /></td>
                </tr>
                <tr>
                    <td>Facebook: </td>
                    <td><button>Sync</button></td>
                </tr>
                <tr>
                    <td>Twitter: </td>
                    <td><button>Sync</button></td>
                </tr>
            </table>
        </div>
        </div>
        <button type="submit">Save</button>
    </div>
</asp:Content>

