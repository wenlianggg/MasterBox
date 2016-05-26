<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="MasterBox.WebForm1" %>
<asp:Content ID="LoginIn" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .loginForm table tr > td {
            padding: 5px;
        }

        .loginForm ol ,loginForm li {
            margin: 0;
            padding: 0.2em;
        }

        .loginForm .loginBtn {
            float: right;
        }
    </style>
    <h2><%: Title %></h2>
    <h3>Welcome to MasterBox</h3>
    <p>
        <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" CssClass="loginForm">
            <LayoutTemplate>
                <p>Please login or register to access our features.</p>
                    <table>
                    <tr>
                        <td>
                            <strong>Username</strong>

                        </td>
                        <td>
                            <asp:TextBox ID="Username" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Password</strong>
                        </td>
                        <td>
                            <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td><strong>I am a...</strong></td>
                        <td>
                            <asp:RadioButtonList ID="ExistingUsrBool" runat="server" RepeatLayout="Flow">
                                <asp:ListItem Selected="True" onclick="">Returning user</asp:ListItem>
                                <asp:ListItem>New user</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" CssClass="loginBtn"/>
            </LayoutTemplate>
        </asp:Login>
    </p>
</asp:Content>
