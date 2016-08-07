<%@ Page Title="Contact Us!" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="emailclass.aspx.cs" Inherits="MasterBox.emailclass" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <table class="table table-hover table-condensed">     
            
            <tr>
                <td> <span title="Your Email Address"> From </span> </td>
                <td> <asp:TextBox ID="txtFrom" runat="server" type="email" placeholder="YourEmail@example.com" style="Width:100%; border-width:3px;"/> </td>
            </tr>

            <tr>
                <td> To </td>
                <td> <asp:TextBox ID="txtToMail" runat="server" type="email" value="masterboxsp@gmail.com" style="Width:100%; border-width:3px;" /> </td>
            </tr>
            <tr>
                <td> Subject </td>
                <td> <asp:TextBox ID="txtSubject" runat="server" style="Width:100%; border-width:3px;" /> </td>
            </tr>
            <tr>
                <td> Message </td>
                <td> <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" style="Height:200px; Width:100%; border-width:3px;" /></td>
            </tr>
            
            <tr>
                <td colspan="2" > <asp:Button class="btn btn-default" ID="btnSubmit" runat="server"
                                       OnClick="btnSubmit_Click" Text="Send" style="Width:20%;"/> </td>
            </tr>
            <tr>
            <td colspan="2"><asp:Label ID="lblMsg" runat="server" />  </td>
            </tr>                 
    </table>

</asp:Content>
