<%@ Page Title="All About Masterbox" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="MasterBox.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
     <div class="aboutHeadline">
        <h1>Cloud Security is of great importance now</h1>
         <hr class="aboutHR" />
        <p class="lead">All about how we provide a quality service for the protection of your files.</p>
    </div>

    <div class="aboutRow">
        <h1>What Is MasterBox?</h1>
        <p class="aboutInfo">MasterBox is a cloud file storage system that aims to provide a secure service for file storaging,
             to protect the files that you would nver want leaked or lost. Use MasterBox to store a myriad of important files that
            you may need to keep confidential. 
        </p>
    </div>

    <div class="aboutRow">
        <div class="aboutLeft">
         <h1>What do we provide?</h1>
                <ul class="aboutList">
                    <li>End-to-End Encryption</li>
                    <p class="aboutDesc">No MasterBox admin, hacker or government can access your data. Hacking even just one file takes lifetimes.</p>
                    <li>Secured Logins</li>
                    <p class="aboutDesc">We provide a very secure login system that has a multi-factored authentication to really make sure that the <br/>only
                        person that will be logging into the account is you.
                    </p>
                    <li>User Friendly Interface</li>
                    <p class="aboutDesc">MasterBox uses an interface that is easy for anyone to understand so that you can keep your files safely at <br/>the same time being able to find these files easily</p>
                </ul>
            </div>
    </div>
</asp:Content>
