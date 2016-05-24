<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <webopt:bundlereference runat="server" path="~/Content/bootstrap.css" />
    <webopt:bundlereference runat="server" path="~/Content/Site.css" />
      <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css"/>
      <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.2/jquery.min.js"></script>
      <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    <style>
        /*SideBarNavigation*/
        .SideMenuBar{   
            margin-top:3.5%;       
            height:100vh;
            background-color:#252523;
            float:left;
            display:block;
            width:23%;
        }
        .SideMenuBar lh{
            color:white;
            padding:10px;
            display:block;     
        }
        .SideMenuBar ul{
            list-style-type:none;
            margin-left:4%;
            padding:0;
            width: 250px;
            
        }
        .SideMenu ul li{
            list-style-type:none;
            border:1px;
            border-style:solid;
            
        }
        .SideMenuBar ul li a{
            display:block;        
            color: ghostwhite;
            padding: 8px 0 8px 16px;
            font-size:0.9em;
            text-decoration: none;
            text-align:left;
            padding:10px 20px 10px 20px;
            cursor:pointer;
        }
        .SideMenuBar ul li a:hover {
            font-style:italic;
            color: white;
        }

        .SideMenuBar img{
            width:20px;
            height:20px;
            margin-right:2%;
        }
        /*user information*/
        .LoginUser{
            color:white;
            text-align:center;
            padding-top:6%;
            padding-bottom:2%;
            border-bottom:1px solid white;
           
        }
        
        .AdvertisementBar{
            color:white;
            text-align:center;
             padding-top:6%;
            padding-bottom:2%;
            border-top:1px solid white;
        }
        

        /*Main Content*/
        .MainContent{   
            float:left;
            width:77%;
            display: block; 
            margin-top:3.5%;     
            height:100vh;
        }

        .FileToolBar{
            width:100%;
            float:left;
            padding-top:2%;
            padding:1%;
            background-color: #4C7DB4;
            -webkit-box-shadow: 0px 1px 2px 0px rgba(0,0,0,0.75);
            -moz-box-shadow: 0px 1px 2px 0px rgba(0,0,0,0.75);
            box-shadow: 0px 1px 2px 0px rgba(0,0,0,0.75);

        }
        .FileToolBar img{
           float:right;
           margin-right:2%;
           cursor:pointer;
           width:20px;
           height:20px;
        }

        .FileContainer{
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
            float:left;
            width:35%;
            height:93%;
            overflow:auto;
            border-right:1px solid black;
        }
        .FileContainer h2{
            padding-bottom:3%;
            border-bottom:1px solid black;
        }
        .FileContainer ul{
            margin: 0.75em 0;
            padding: 0 1em;
            list-style:none;
        }
        .FileContainer ul li:before{
            content: "";
            border-color: transparent #111;
            border-style: solid;
            border-width: 0.35em 0 0.35em 0.45em;
            display: block;
            height: 0;
            width: 0;
            left: -1em;
            top: 1.1em;
            position: relative;
        }

        .FileTreeContainer{
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
            float:left;
            width:65%;
            height:93%;
            overflow:auto;
        }
        .FileTreeContainerTable{
            width:49%;
            float:left;
            padding-top:10px;
            padding-left:2.2%;
        }

        .FileTreeContainerObtainedFiles{

        }

    </style>
    <script>
        function gotoMain() {
            document.getElementById('MainDiv').style.display = "block";
            document.getElementById('OtherDiv').style.display = "none";
        }
        function gotoOther() {
            document.getElementById('MainDiv').style.display = "none";
            document.getElementById('OthersDiv').style.display = "block";
        }

        
</script>
    <title>Home - MasterBox</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="navbar navbar-inverse navbar-fixed-top navbar-custom">
            <div class="container">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
							<span class="icon-bar"></span>
							<span class="icon-bar"></span>
							<span class="icon-bar"></span>
						</button>
						<a class="navbar-brand" runat="server" style="cursor:default;" >MasterBox</a>
					</div>
					<div class="navbar-collapse collapse">
						<ul class="nav navbar-nav navbar-right">
							<li><a class="navbar-brand" runat="server" href="~/">Log out</a></li>
						</ul>
					</div>
            </div>
        </div>
        
        <div class="SideMenuBar">        
            <div class="LoginUser">
                <p>placeHolder{UserName}</p>  
                 
            </div>   
            <ul>
               <lh style="font-size:1em;">APPLICATIONS</lh>
               <li><a onclick="gotoMain()"title="Files"data-toggle="tooltip" data-placement="right"><img src=""align="left"/>Files</a></li> <!*Files*/>
               <li><a onclick="gotoOther()" ><img src=""align="left"/>Shared Files</a></li> <!/*Shared*/>
               <li><a onclick="gotoSettings()"><img src=""align="left"/>Settings</a></li> <!/*Settings*/>
               <li><a><img src=""align="left"/>Contact Us</a></li> <!/*Contact Us*/>
               <li><a><img src=""align="left"/>placeHolder{Navigation}</a></li> 
               <li><a><img src=""align="left"/>placeHolder{Navigation}</a></li>
            </ul>
                
            <div class="AdvertisementBar">
                 <p>placeHolder{Advertisement}</p>
                 <p>placeHolder{Advertisement-Image}</p>
            </div>
            
        </div>
        <div class="MainContent" id="MainDiv">
            <div class="FileToolBar">
                <div style="margin-right:2.5%;"> 
                <a onclick=""><img class="FileIcon"src="" title="Delete Folder" data-toggle="tooltip" data-placement="bottom"/></a>
                <a onclick=""><img class="FileIcon"src="" title="Share Folder" data-toggle="tooltip" data-placement="bottom"/></a>
                <a onclick=""><img class="FileIcon"src="" title="New Folder" data-toggle="tooltip" data-placement="bottom"/></a>
                <a onclick=""><img class="FileIcon"src="" title="Upload" data-toggle="tooltip" data-placement="bottom"/></a>
                </div>
           
            
            </div>
            <div class="FileContainer">
               <h2 >placeHolder{Files}</h2>   
                <ul>
                    <li>placeHolder{Files}</li>
                    <li>placeHolder{Files}</li>
                    <li>placeHolder{Files}</li>
                    <li>placeHolder{Files}</li>
                    

                </ul>



            </div>
            <div class="FileTreeContainer">
                <h3>placeHolder{Files-Tree}</h3>   
                <div class="row" style="margin:0 auto; border-bottom:2px solid black;" >
                    <div class="FileTreeContainerTable">
                        <p>Name: </p>
                    </div>
                    <div class="FileTreeContainerTable" >
                        <p>Last Modified: </p>
                    </div>
                    
                </div>
                <div class="FileTreeContainerObtainedFiles">
                    <div class="FileTreeContainerTable" >
                        <p>placeHolder{FileName} </p>
                    </div>
                    <div class="FileTreeContainerTable" >
                        <p>placeHolder{Last_Modified} </p>
                    </div>
                                 
                 </div>
                
            </div>
          
        
        </div>

        <div class="MainContent" id="OthersDiv" style="display:none;">
            <h1>Hello</h1>
            <h1>Hello</h1>
        </div>

    </div>
    </form>
    <script>
        $(document).ready(function(){
            $('[data-toggle="tooltip"]').tooltip();   
        });
     </script>
</body>
</html>
