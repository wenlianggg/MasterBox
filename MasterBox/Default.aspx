<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MasterBox._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <!--
    <div class="jumbotron">
        <h1>MasterBox</h1>
        <p class="lead">Secure storage solutions for the discerning security professional.</p>
        <p><a href="http://google.com.sg" class="btn btn-primary btn-lg">Learn more &raquo;</a></p>
    </div>
    -->

    <!-- ********** Testing image slider ********** -->
        <div id="myCarousel" class="carousel slide" data-ride="carousel">

            <!-- Indicators -->
            <ol class="carousel-indicators">
                <li data-target="#myCarousel" data-slide-to="0" class="active"></li>
                <li data-target="#myCarousel" data-slide-to="1"></li>
                <li data-target="#myCarousel" data-slide-to="2"></li>
                <li data-target="#myCarousel" data-slide-to="3"></li>
                <li data-target="#myCarousel" data-slide-to="4"></li>
            </ol>

            <!-- Wrapper for slides -->
            <div class="carousel-inner" role="listbox">

                <div class="item active">
                    <img src="images/MasterBoxSlider.png" alt="the best logo in the world"/>
                </div>

                <div class="item">
                    <img src="images/MasterBoxSlidergrey.png" alt="the best logo in the world 2"/>
                </div>

                <div class="item">
                    <img src="images/MasterBoxSliderblue.png" alt="the best logo in the world 3"/>
                </div>

                <div class="item">
                    <img src="images/MasterBoxSlidergreen.png" alt="the best logo in the world 3"/>
                </div>

            </div>
            
            <!-- Left and right controls -->
            <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
                <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                <span class="sr-only">Previous</span>
            </a>
            <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
                <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                <span class="sr-only">Next</span>
            </a>
        </div>
    <!-- ********** /Testing image slider ********** -->

    <div class="punchlinetitle">
        <h2>Rest assured, it's <span class="h2secure">secured</span>.</h2>
        <p>At MasterBox, we make security a reality, one user at a time, one file at a time.<br /> Using the latest encryption algorithms and industry grade protocol,
             your secrets are safe with us.
        </p>
        <p>It's no delusion. <b> - </b> We are your solution.</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2><span class="h2getstarted">Get started</span></h2>
            <p>
                Register on our web service to gain access to our web storage service.
                New account registrations get 1GB of free space.
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301948">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2><span class="h2secure">Secure</span></h2>
            <p>
               Secure AES-256 and Blowfish-448 encryption standard used for file storage management, along
                with secure two factor authentication and facial recognition features. 
            </p>
            <p>
                <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2><span class="h2scalable">Scalable</span></h2>
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
