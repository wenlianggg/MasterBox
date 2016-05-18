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
            height:100vh;
        }
        .FileTreeContainer{
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
            float:left;
            width:64%;
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
        <div class="SideMenuBar" style="border:1px solid red;background:#555;">
           
            <div class="LoginUser">
                <p>placeHolder{UserName}</p>
            </div>
            <hr />
           
            
            <ul>
                <lh style="font-size:1em;">placeHolder{Title}</lh>
               <li><a><img src=""/align="left">placeHolder{Navigation}</a></li> <!*Files*/>
               <li><a><img src=""/align="left">placeHolder{Navigation}</a></li> <!/*Shared*/>
               <li><a><img src=""/align="left">placeHolder{Navigation}</a></li> <!/*Settings*/>
               <li><a><img src=""/align="left">placeHolder{Navigation}</a></li> <!/*Contact Us*/>
               <li><a><img src=""/align="left">placeHolder{Navigation}</a></li> 
               <li><a><img src=""/align="left">placeHolder{Navigation}</a></li>
            </ul>
                <hr />
                <div class="AdvertisementBar">
             <p>placeHolder{Advertisement}</p>
                     <p>placeHolder{Advertisement-Image}</p>
            </div>
            
        </div>
        <div class="MainContent">
            <div class="FileContainer" style="border:1px solid red;">
               <p><h3>placeHolder{Files}</h3></p>
            </div>
            <div class="FileTreeContainer" style="border:1px solid red;">
                <p><h2>placeHolder{Files-Tree}</h2></p>
                <hr style="border:1px solid black"/>
                <hr style="border:1px solid black"/>
                
                    <asp:Table ID="Table1" class="FileTable" runat="server" BorderStyle="Solid">
                        <asp:TableRow>
                            <asp:TableHeaderCell>Name:</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Last Modified</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Shared With</asp:TableHeaderCell>
                        </asp:TableRow>



                    </asp:Table>
               
            </div>
          </div>
         

    </div>
    </form>
</body>
</html>
