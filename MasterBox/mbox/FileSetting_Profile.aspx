﻿<%@ Page Language="C#" Title="" MasterPageFile="~/mbox/FileSetting.Master" AutoEventWireup="true" CodeBehind="FileSetting_Profile.aspx.cs" Inherits="MasterBox.FileSettingInterface" %>

<asp:Content ID="Testing" ContentPlaceHolderID="Child_Profile" runat="server">
    <div class="Setting_Profile">
        <h1>Profile</h1>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-8">
                <div class="row" style="padding: 2%;">
                    <h4>Account Details</h4>
                    <table>
                        <tr>
                            <td>
                                <label>Username: </label>
                            </td>
                            <td>
                                <label>
                                    <input type="text" />
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>E-mail: </label>
                            </td>
                            <td>
                                <input type="email" /></td>
                        </tr>
                    </table>
                </div>
                <div class="row">
                    <h4>Preference</h4>
                    <h5>Email Notifications</h5>
                    <table class="table_pref">
                        <tr>
                            <td>
                                <input type="checkbox" /></td>
                            <td>My MasterBox is almost out of space</td>
                        </tr>
                        <tr>
                            <td>
                                <input type="checkbox" /></td>
                            <td>Many files deleted from my MasterBox</td>
                        </tr>
                        <tr>
                            <td>
                                <input type="checkbox" /></td>
                            <td>MasterBox newsletters</td>
                        </tr>
                        <tr>
                            <td>
                                <input type="checkbox" /></td>
                            <td>MasterBox activity digests</td>
                        </tr>
                        <tr>
                            <td>
                                <input type="checkbox" /></td>
                            <td>Invitations to give MasterBox feedback</td>
                        </tr>
                    </table>
                </div>



            </div>

            <div class="col-xs-6 col-md-4">
                <div class="prof_icon">
                    Profile Icon
                     <input type="file" />
                </div>

            </div>
        </div>
        <div class="row">
            <button type="submit">Update</button>
        </div>
    </div>

</asp:Content>


