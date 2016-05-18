<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <webopt:bundlereference runat="server" path="~/Content/css" />
    <style>
        /*SideBarNavigation*/
        .SideMenuBar{
            margin:0;
            display:block;
            width:100%;
            height:100vh;
            background-color:#555;
        }
        .SideMenuBar lh{
            color:white;
            padding:10px;
            display:block;     
        }
        .SideMenuBar ul{
            list-style-type:none;
            margin:0 auto;
            padding:0;
            width: 200px;
            background-color: #f1f1f1;
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
            text-decoration: none;
            text-align:center;
            padding:10px 20px 10px 20px;
        }
        .SideMenuBar ul li a:hover {
            background-color: #555;
            color: white;
        }

        /*user information*/
        .LoginUser{
            color:white;
            text-align:center;
            padding-top:5%;
        }
        .AdvertisementBar{
            color:white;
            text-align:center;
        }
        

        /*Main Content*/
        .MainContent{
            border:1px solid black;
            float:left;
            width:75%;
            height:100vh;
        }
        .FileContainer{
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
            float:left;
            width:35%;
        }
        .FileTreeContainer{
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
            float:left;
            width:65%;
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
        <div class="row">
        <div class="col-md-3" style="border:1px solid red;background:#555;">
            <div class="SideMenuBar">
            <div class="LoginUser">
                <p>placeHolder{UserName}</p>
            </div>
            <hr />
           
            <lh>placeHolder{Title}</lh>
            <ul>
               <li><a>placeHolder{Navigation}</a></li> <!*Files*/>
               <li><a>placeHolder{Navigation}</a></li> <!/*Shared*/>
               <li><a>placeHolder{Navigation}</a></li> <!/*Settings*/>
               <li><a>placeHolder{Navigation}</a></li> <!/*Contact Us*/>
               <li><a>placeHolder{Navigation}</a></li> 
               <li><a>placeHolder{Navigation}</a></li>
            </ul>
                <hr />
                <div class="AdvertisementBar">
             <p>placeHolder{Advertisement}</p>
                     <p>placeHolder{Advertisement-Image}</p>
            </div>
            </div>
        </div>
        <div class="MainContent">
            <div class="FileContainer" style="border:1px solid red;">
               <p>placeHolder{Files}</p>
            </div>
            <div class="FileTreeContainer" style="border:1px solid red;">
                <p>placeHolder{Files-Tree}</p>
                 <p>placeHolder{Files-Tree}</p>
                 <p>placeHolder{Files-Tree}</p>
                 <p>placeHolder{Files-Tree}</p>
                 <p>placeHolder{Files-Tree}</p>
            </div>
          </div>
         

    </div>
    </form>
</body>
</html>
