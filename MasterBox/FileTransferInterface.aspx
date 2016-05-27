<%@ Page Language="C#" Title="" MasterPageFile="~/Internal.Master" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>

<asp:Content ID="LoginIn" ContentPlaceHolderID="InternalContent" runat="server">
    
     <ul class='custom-menu'>
          <li data-action="upload">Upload</li>
          <li data-action="file">New Folder</li>
          <li data-action="sharefile">New Shared Folder</li>
         <li data-action="delete">Delete</li>
     </ul>


        <div class="MainContent">
            <div id="light" class="white_content">This is the lightbox content. <a href = "javascript:void(0)" onclick = "document.getElementById('light').style.display='none';document.getElementById('fade').style.display='none'">Close</a></div>
    <div id="fade" class="black_overlay"></div>
            <div class="FileToolBar">
                <div style="margin-right: 2.5%;">
                    <a onclick="">
                        <img class="FileIcon" src="images/Logged/FileDelete.png" title="Delete Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                    <a onclick="">
                        <img class="FileIcon" src="images/Logged/NewSharedFolder.png" title="New Shared Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                    <a onclick="">
                        <img class="FileIcon" src="images/Logged/NewFolder.png" title="New Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                    <a onclick="document.getElementById('light').style.display='block';document.getElementById('fade').style.display='block'">
                        <img class="FileIcon" src="images/Logged/Upload.png" title="Upload" data-toggle="tooltip" data-placement="bottom" /></a>
                </div>
            </div>
            <div class="FileContainer">
                <h2>Files</h2>
                <ul>
                    <li>placeHolder{Files}</li>
                    <li>placeHolder{Files}</li>
                    <li>placeHolder{Files}</li>
                    <li>placeHolder{Files}</li>
                </ul>
            </div>
            <div class="FileTreeContainer">
                <h3>placeHolder{Files-Tree}</h3>
                <div class="row" style="margin: 0 auto; border-bottom: 2px solid black;">
                    <div class="FileTreeContainerTable">
                        <p>Name: </p>
                    </div>
                    <div class="FileTreeContainerTable">
                        <p>Last Modified: </p>
                    </div>

                </div>
                <div class="FileTreeContainerObtainedFiles">
                    <div class="FileTreeContainerTable">
                        <p>placeHolder{FileName} </p>
                    </div>
                    <div class="FileTreeContainerTable">
                        <p>placeHolder{Last_Modified} </p>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
