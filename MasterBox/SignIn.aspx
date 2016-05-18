<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SignIn.aspx.cs" Inherits="MasterBox.WebForm1" %>
<asp:Content ID="LoginIn" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <h3>Login / Register</h3>
    <p>
        <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" Width="228px">
            <LayoutTemplate>
                <table cellpadding="1" cellspacing="0" style="border-collapse:collapse;">
                    <tr>
                        <td>
                            <table cellpadding="0" style="width:560px;">
                                <tr>
                                    <td align="center" colspan="2">Log In</td>
                                </tr>
                                <tr>
                                    <td align="right"><strong>Username:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </strong></td>
                                    <td>
                                        <asp:TextBox ID="UserName" runat="server" Width="342px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right"><strong>Password:&nbsp;&nbsp;&nbsp;
                                        <br />
                                        &nbsp; </strong></td>
                                    <td>
                                        <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="343px"></asp:TextBox>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="RememberMe" runat="server" OnCheckedChanged="RememberMe_CheckedChanged" Text="Remember me next time." />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" style="color:Red;">
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2">
                                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" ValidationGroup="Login1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
        </asp:Login>
    </p>
</asp:Content>
