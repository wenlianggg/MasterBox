<%@ Page Title="Contact Us!" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="MasterBox.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
		<div class="aboutHeadline">
		<h1><%: Title %></h1>
		<hr class="aboutHR" />
		<p class="lead">Talk to us about your needs!</p>
	</div>
    <address>
        One Wanker Way<br />
        Singapore, S999999<br />
        <abbr title="Phone">P:</abbr> 6123 4567
    </address>

    <address>
        <strong>Support: </strong><a href="mailto:support@masterbaux.com">support@masterbaux.com</a><br />
        <strong>Marketing: </strong><a href="mailto:marketing@masterbaux.com">marketing@masterbaux.com</a>
    </address>
</asp:Content>
