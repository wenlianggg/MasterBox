<%@ Page Title="Contact Us!" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="MasterBox.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="aboutHeadline">
        <h1><%: Title %></h1>
        <hr class="aboutHR" />
        <p class="lead">Talk to us about your needs!</p>
    </div>
    
    <div class="col-md-4">
        <h2>Help Corner</h2><br />
        <address>
            <strong>Support: </strong><br />
            <a href="mailto:support@masterbaux.com">masterboxsp@gmail.com</a><br />
        </address>
        <address>
            <strong>Marketing: </strong><br />
            <a href="mailto:marketing@masterbaux.com">marketing@masterbaux.com</a>
        </address>
        <address>
            <strong>Security: </strong><br />
            <a href="mailto:verysecure@masterbaux.com">masterboxsec@gmail.com</a>
        </address>
    </div>

     <div class="col-md-3">
        <h2>Find us!</h2><br />
        <address>
            <strong>Masterbox Inc.</strong><br />
            One Wanker Way<br />
            Singapore, S999999<br />
            <abbr title="Phone">P:</abbr>
            6123 4567
        </address>
    </div>

    <div class="col-md-5">
        <h2>For any other enquiries</h2><br />
        Please submit the following form and a MasterBox representative will get in touch with you<br /><br />
        <!--
        <div class="form-horizontal">
            <div class="form-group">
                <label for="inputEmail3" class="col-sm-2 control-label">Email</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="inputEmail3" placeholder="Email">
                </div>
            </div>
            <div class="form-group">
                <label for="inputPassword3" class="col-sm-2 control-label">Subject</label>
                <div class="col-sm-10">
                    <input type="text" class="form-control" id="inputSubject3" placeholder="Subject">
                </div>
            </div>
            <div class="form-group">
                <label for="inputPassword3" class="col-sm-2 control-label">Message</label>
                <div class="col-sm-10">
                    <textarea class="form-control" rows="5" placeholder="Give us a description of your concerns"></textarea>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <button type="submit" class="btn btn-default">Send Message</button>
                </div>
            </div>
        </div>
        -->
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
    </div>
</asp:Content>
