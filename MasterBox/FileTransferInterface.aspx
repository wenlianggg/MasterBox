<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <webopt:bundlereference runat="server" path="~/Content/css" />
    <style>
        /*SideBarNavigation*/
        .SideMenuBar{
            margin:0;          
            height:100vh;
            background-color:#363533;
            float:left;
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

        /*user information*/
        .LoginUser{
            color:white;
            text-align:center;
            padding-top:5%;
            border-bottom:1px solid white;
           
        }
        .AdvertisementBar{
            color:white;
            text-align:center;
            border-top:1px solid white;
        }
        

        /*Main Content*/
        .MainContent{
            border:1px solid green;
            float:left;
            width:77%;
            height:100vh;
        }
        .FileContainer{
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
            float:left;
            width:35%;
            height:100vh;
        }
        .FileTreeContainer{
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
            float:left;
            width:65%;
            height:100vh;
        }
          
        table,th,td{    
            border:1px solid black;
            border-spacing:10px;
            border-collapse: separate;
        }
        
        .FileTable th{
            padding:10px;
        }
        .FileIcon{
            width:20px;
            height:20px;
        }
    </style>
    <title>Home - MasterBox</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="navbar navbar-inverse navbar-fixed-top">
            <div class="container">
                <div class="navbar-header">
              
                    <a class="navbar-brand" runat="server">placeHolder{Title}</a>
                    
                   
                      
                </div>  
            </div>
        </div>
        
        <div class="SideMenuBar">
           
            <div class="LoginUser">
                <p>placeHolder{UserName}</p>
               
            </div>
           
           
            
            <ul>
                <lh style="font-size:1em;">placeHolder{Title}</lh>
               <li><a><img class="FileIcon"src=""/align="left">placeHolder{Navigation}</a></li> <!*Files*/>
               <li><a><img class="FileIcon"src=""/align="left">placeHolder{Navigation}</a></li> <!/*Shared*/>
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
        <div class="MainContent">
            <div class="FileContainer" style="border:1px solid blue;">
               <p><h3>placeHolder{Files}</h3></p>
            </div>
            <div class="FileTreeContainer" style="border:1px solid yellow;">
                <p><h2>placeHolder{Files-Tree}</h2></p>
                
                
                    <asp:Table ID="Table1" class="FileTable" runat="server" BorderStyle="Solid">
                        <asp:TableRow>
                            <asp:TableHeaderCell>Name:</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Last Modified</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Shared With</asp:TableHeaderCell>
                        </asp:TableRow>



                    </asp:Table>
               
            </div>
          
         

    </div>
    </form>
</body>
</html>
