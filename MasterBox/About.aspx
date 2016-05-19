<%@ Page Title="All About Masterbox" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="MasterBox.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <div class="aboutHeadline">
        <h1>Cloud Security is of great importance now</h1>
         <hr class="aboutHR" />
        <p class="lead">All about how we provide a quality service for the protection of your files.</p>
    </div>

    <div class="aboutRow" id="row1">
        <h1>What Is MasterBox?</h1>
        <p class="aboutInfo">In this day and age, security is increasingly becoming an issue. Many users have encountered situation in where their data is compromised and have been leaked. MasterBox aims to provide that security, so that your personal and confidential data is protected as best as possible. MasterBox is a cloud file storage system that aims to provide a secure service for file storaging,
             to protect the files that you would never want leaked or lost. Use MasterBox to store a myriad of important files that
            you may need to keep confidential.</p>
    </div>

    <hr class="aboutRowHR"/>
        <div class="aboutRow" id="row3">
            <h1>Our Mission</h1>
            <p class="aboutInfo">
                Our mission is to provide a high quality service for users to store their personal files in a safe and secured environment. Also ensuring that none of the personal data leaks out and to ensure that none of the accounts are compromised.
                We also hope to be the new standard of security for the current cloud based storage systems, to encourage a safer and brighter tomorrow.
            </p>
        </div>

    <hr class="aboutRowHR"/>

    <div class="aboutRow" id="row2">
        <div class="aboutLeft" id ="left1">
         <h1>What do we provide?</h1>
                <ul class="aboutList">
                    <li>End-to-End Encryption</li>
                    <p class="aboutDesc">No MasterBox admin, hacker or government can access your data. Hacking even just one file takes lifetimes.</p>
                    <li>Secured Logins</li>
                    <p class="aboutDesc">We provide a very secure login system that has a multi-factored authentication to really make sure that the only
                        person that will be logging into the account is you.
                    </p>
                    <li>User Friendly Interface</li>
                    <p class="aboutDesc">MasterBox uses an interface that is easy for anyone to understand so that you can keep your files safely at the same time being able to find these files easily</p>
                </ul>
            </div>
        <div class="aboutRight" id="right1">
            <img class="secureBox" src="images/Logistic_shipping_delivery_service_transportation_transport-07-512.png" />
        </div>
    </div>

    <hr class="aboutRowHR"/>

    <div class="aboutRow" >
        <div class="aboutLeft" id="left2">
        <h1>Why should you use Masterbox?</h1>
            <ul class="aboutList">
                <li>Safe</li>
                <p class="aboutDesc">We are pretty safe to use</p>
                <li>Secured</li>
                <p class="aboutDesc">We are pretty secure to use</p>
                <li>Systems</li>
                <p class="aboutDesc">We have pretty systems to use</p>
            </ul>
        </div>
        <div class="aboutRight" id="right2">
            <img class="shield" src="images/shield_and_swords-512.png" />
        </div>
    </div>
    <hr class="aboutRowHR"/>
</asp:Content>
