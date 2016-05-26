<%@ Page Title="Login Authentication" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="MasterBox.WebForm1" %>

<asp:Content ID="HeadContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="~/Auth/LoginStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="LoginIn" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>Welcome to MasterBox</h3>
    <p>
        <asp:Login ID="LoginForm" runat="server" OnAuthenticate="authenticate" CssClass="loginForm">
            <LayoutTemplate>
                <p>Please login or register to access our features.</p>
                <table>
                    <tr>
                        <td>
                            <strong>Username</strong>

                        </td>
                        <td>
                            <asp:TextBox ID="UserName" class="usrfield" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Password</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="Password" class="pwdfield" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><strong>I am a...</strong></td>
                        <td>
                            <input type="radio" name="userExists" onclick="registerMode(false)" id="existingUser" checked>
                            <label for="existingUser">Returning user</label><br />
                            <input type="radio" name="userExists" onclick="registerMode(true)" id="newUser">
                            <label for="newUser">New user</label>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="LoginButton" class="btn btn-default-blue loginBtn" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" />
            </LayoutTemplate>
        </asp:Login>
    </p>
    <script>
        function registerMode(bool) {
            $(".pwdfield").attr('readonly', bool);
            $(".pwdfield").attr('disabled', bool);
            if (bool === true) {
                $(".loginBtn").prop('value', "Register");
            } else {
                $(".loginBtn").prop('value', "Sign In");
            }
        }
        pwdFieldActive(false);
    </script>
</asp:Content>
