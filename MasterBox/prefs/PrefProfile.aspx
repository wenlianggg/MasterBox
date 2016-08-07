<%@ Page Language="C#" Title="Profile Preferences" MasterPageFile="~/prefs/Preferences.Master" AutoEventWireup="true" CodeBehind="prefprofile.aspx.cs" Inherits="MasterBox.Prefs.FileSetting_Profile" %>

<asp:Content ID="SetProfile" ContentPlaceHolderID="Preferences" runat="server">
    <div class="page-header">
        <h1><%: Page.Title %>
        </h1>
    </div>
    <ol class="breadcrumb" style="margin-bottom: 5px;">
        <li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
        <li>User Preferences</li>
        <li class="active"><%: Page.Title %></li>
    </ol>
    <br />
    <div class="col-xs-12 col-sm-8 col-md-8">
        <h2>Account Preferences</h2>
        <h4>User Details</h4>
        <table>
            <tr>
                <td>
                    <label>Username: </label>
                </td>
                <td>
                    <asp:TextBox CssClass="form-control" ID="usernameTxt" runat="server" Font-Size="Medium"></asp:TextBox></td>
            </tr>
            <tr>
                <td>
                    <label>E-mail: </label>
                </td>
                <td>
                    <asp:TextBox CssClass="form-control" ID="emailTxt" runat="server" Font-Size="Medium"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label>First Name: </label>
                </td>
                <td>
                    <asp:TextBox CssClass="form-control" ID="FNameTxt" runat="server" Font-Size="Medium"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Last Name: </label>
                </td>
                <td>
                    <asp:TextBox CssClass="form-control" ID="LNameTxt" runat="server" Font-Size="Medium"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <label>Date of Birth: </label>
                </td>
                <td>
                    <asp:TextBox CssClass="form-control" ID="DobTxt" runat="server" Font-Size="Medium"></asp:TextBox>
                </td>
            </tr>
        </table>
        <h4>Contact Preferences</h4>
        <table>
            <tr>
                <td><input type="checkbox" class="form-control" /></td>
                <td>Notifications about my files and subscription</td>
            </tr>
            <tr><td><input type="checkbox" class="form-control" /></td>
                <td>Receive marketing information about MasterBox</td>
            </tr>
        </table>
        <asp:Label ID="Msg" runat="server" />
        <asp:Button ID="ProfileChangeBtn"
            Text="Update Details"
            CssClass="btn btn-info"
            OnClick="ProfileChange_Click"
            runat="server" />
    </div>

</asp:Content>


