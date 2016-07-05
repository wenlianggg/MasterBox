<%@ Page Language="C#" Title="" MasterPageFile="~/mbox/FileSetting.Master" AutoEventWireup="true" CodeBehind="FileSetting_Profile.aspx.cs" Inherits="MasterBox.FileSetting_Profile" %>

<asp:Content ID="SetProfile" ContentPlaceHolderID="Child_Profile" runat="server">
    <div class="Setting_Profile">
        <h1>Profile</h1>
        <hr class="aboutRowHR" />
        <br />
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-8">
                <div class="row" style="padding: 2%;">
                    <h4 class="SettingHr">Account Details</h4>
                    <table>
                        <tr>
                            <td>
                                <label>Username: </label>
                            </td>
                            <td>
                                <asp:TextBox CssClass="form-control" ID="username" runat="server" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>E-mail: </label>
                            </td>
                            <td>
                                <asp:TextBox CssClass="form-control" ID="email" runat="server" Font-Size="Medium"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="row">
                    <h4 class="SettingHr">Preference</h4>
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
                     
                </div>
                <input type="file" />

            </div>
        </div>
        <div class="row">
            <button type="submit">Update</button>
        </div>
    </div>

</asp:Content>


