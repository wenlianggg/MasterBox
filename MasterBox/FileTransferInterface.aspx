<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <webopt:bundlereference runat="server" path="~/Content/bootstrap.css" />
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
            color: #000;
            padding: 8px 0 8px 16px;
            font-size:0.9em;
            text-decoration: none;
            text-align:center;
            padding:10px 20px 10px 20px;
        }
        .SideMenuBar ul li a:hover {
            font-style:italic;
            color: white;
        }

        .FileIcon{
            width:20px;
            height:20px;
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
            border:1px solid green;
            float:left;
            width:77%;
            display: block; 
            margin-top:3.5%;     
            height:100vh;
        }
        .FileContainer{
            border:1px solid blue;
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
            float:left;
            width:35%;
            height:100vh;
        }
        .FileContainer h3{
            padding-bottom:2.2%;
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
            border:1px solid yellow;
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
            float:left;
            width:65%;
            height:100vh;
        }
        .FileTreeContainerTable{
            width:33%;
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
        <div class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
						<a class="navbar-brand" runat="server" href="~/">placeHolder{Title}</a>
					</div>
					<div class="navbar-collapse collapse">
						<ul class="nav navbar-nav navbar-right">
							<li><a runat="server" href="~/">Log out</a></li>
						
						</ul>
					</div>
            </div>
        </div>
        
        <div class="SideMenuBar">        
            <div class="LoginUser">
                <p>placeHolder{UserName}</p>  
              
                   
            </div>   
            <ul>
               <lh style="font-size:1em;">placeHolder{Title}</lh>
               <li><a onclick="gotoMain()"><img class="FileIcon"src=""/align="left">placeHolder{Navigation}</a></li> <!*Files*/>
               <li><a onclick="gotoOther()"><img class="FileIcon"src=""/align="left">placeHolder{Navigation}</a></li> <!/*Shared*/>
               <li><a><img class="FileIcon"src=""/align="left">placeHolder{Navigation}</a></li> <!/*Settings*/>
               <li><a><img class="FileIcon"src=""/align="left">placeHolder{Navigation}</a></li> <!/*Contact Us*/>
               <li><a><img class="FileIcon"src=""/align="left">placeHolder{Navigation}</a></li> 
               <li><a><img class="FileIcon"src=""/align="left">placeHolder{Navigation}</a></li>
            </ul>
                
            <div class="AdvertisementBar">
                 <p>placeHolder{Advertisement}</p>
                 <p>placeHolder{Advertisement-Image}</p>
            </div>
            
        </div>
        <div class="MainContent" id="MainDiv">
            <div class="FileContainer">
               <h3 >placeHolder{Files}</h3>
                
                <ul>
                    <li>placeHolder{Files}</li>
                        <ul>
                            <li>placeHolder{Files}</li>
                            <li>placeHolder{Files}</li>
                            <li>placeHolder{Files}</li>
                        </ul>
                    <li>placeHolder{Files}</li>
                    <li>placeHolder{Files}</li>
                    <li>placeHolder{Files}</li>
                    <li>placeHolder{Files}</li>
                </ul>



            </div>
            <div class="FileTreeContainer">
                <h2>placeHolder{Files-Tree}</h2> 
                 <!--
                    <asp:Table ID="Table1" class="FileTable" runat="server" BorderStyle="Solid">
                        <asp:TableRow>
                            <asp:TableHeaderCell>Name:</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Last Modified:</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Shared With:</asp:TableHeaderCell>
                        </asp:TableRow>
                    </asp:Table>
                  -->
                <div class="row" style="border-bottom:1px solid black;">
                    <div class="FileTreeContainerTable" style="border:1px solid black">
                        <p>Name: </p>
                    </div>
                    <div class="FileTreeContainerTable" style="border:1px solid black">
                        <p>Last Modified: </p>
                    </div>
                    <div class="FileTreeContainerTable" style="border:1px solid black">
                        <p>Shared With:</p>
                    </div>
                </div>
                <div class="FileTreeContainerObtainedFiles">

                    <div class="FileTreeContainerTable" >
                        <p>placeHolder{FileName} </p>
                    </div>
                    <div class="FileTreeContainerTable" >
                        <p>placeHolder{Last_Modified} </p>
                    </div>
                    <div class="FileTreeContainerTable" >
                        <p>placeHolder{Shared_With}</p>
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
</body>
</html>
