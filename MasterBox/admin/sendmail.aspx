<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="sendmail.aspx.cs" Inherits="MasterBox.Admin.SendMail" %>

<asp:Content ID="SendMailContent" ContentPlaceHolderID="AdminPanelContentPH" runat="server">
    <div class="panel panel-info">
        <div class="panel-heading"><strong>Send Users Email</strong></div>

        <div class="panel panel-info" style="border-width:0 0 0 5px; border-color:#abd4e9; background-color:#f5f5f5; margin:1%; padding:1px; padding-left:1%;">
            <p>Need to send to multiple people?<p>
            <p style="margin-bottom:0px;">Add a comma inbetween <br />e.g.<br /> email1@exmaple.com, email2@example.com, email3@example.com</p>
        </div>

        <div class="panel-body">
            <table class="table table-hover table-condensed">

                <tr>
                    <td> To </td>
                    <td> <asp:TextBox ID="txtToMail" runat="server" type="email" placeholder="UsersEmail@example.com" style="Width:100%; border-width:3px;" /> </td>
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
        </div>
    </div>
</asp:Content>
