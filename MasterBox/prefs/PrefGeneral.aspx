<%@ Page Title="User Dashboard" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="prefgeneral.aspx.cs" Inherits="MasterBox.Prefs.FileSetting_General" %>

<asp:Content ID="SetGeneral" ContentPlaceHolderID="Preferences" runat="server">
		<div class="page-header">
			<h1><%: Page.Title %>
			</h1>
		</div>
		<ol class="breadcrumb" style="margin-bottom: 5px;">
			<li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
			<li>User Preferences</li>
			<li class="active"><%: Page.Title %></li>
		</ol>
        <div class="row">
            <h4 class="SettingHr">Storage Space</h4>
            <div class="progress">
                <div class="progress-bar" role="progressbar" aria-valuenow="70"
                    aria-valuemin="0" aria-valuemax="100" style="width: 20%">
                    2.7MB
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

