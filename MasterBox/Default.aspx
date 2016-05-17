<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MasterBox._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>MasterBox</h1>
        <p class="lead">Secure storage solutions for the discerning security professional.</p>
        <p><a href="http://google.com.sg" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>Get started</h2>
            <p>
                Register on our web service to gain access to our web storage service.
                New account registrations get 1GB of free space.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Secure</h2>
            <p>
               Secure AES-256 and Blowfish-448 encryption standard used for file storage management, along
                with secure two factor authentication and facial recognition features. 
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Scalable</h2>
            <p>
                Easily upgrade your storage space with our storage plans going up to 5GB in size, with our streamlined
                checkout process with streamlined integration with PayPal, our payment processor.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
    </div>

</asp:Content>
