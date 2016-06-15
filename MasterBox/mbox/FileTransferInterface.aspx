<%@ Page Language="C#" Title="" MasterPageFile="~/mbox/Internal.Master" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>

<asp:Content ID="LoginIn" ContentPlaceHolderID="InternalContent" runat="server">

    <ul class='custom-menu'>
        <li data-action="upload" data-toggle="modal" data-target="#uploadModel">Upload</li>
        <li data-action="file">New Folder</li>
        <li data-action="sharefile">New Shared Folder</li>
        <li data-action="delete">Delete</li>
    </ul>



    <div class="MainContent">
        <!--Upload Modal -->
        <div id="uploadModel" class="modal fade" role="dialog">
            <div class="modal-dialog">
                
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Upload a File</h4>
                    </div>
                    <div class="modal-body">
                        <span>Choose a file to upload:</span>
                        <asp:FileUpload ID="FileUpload" runat="server" />
                    </div>
                    <div class="modal-footer">
                       
                        <asp:Label ID="UploadStatus" runat="server" Text=""></asp:Label>
                        <asp:Button ID="NewUploadFile" runat="server" Text="Upload" OnClick="NewUploadFile_Click" />
                    </div>        
               </div>
            </div>
        </div>


        <!--Folder Modal -->
        <div id="folderModel" class="modal fade" role="dialog">
            <div class="modal-dialog">
                
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">New Folder</h4>
                    </div>
                    <div class="modal-body">
                        <span>Choose a file to upload:</span>
                        <input type="file" class="file-loading" />
                    </div>
                    <div class="modal-footer">
                        
                    </div>
                </div>
            
            </div>
        </div>
        <!--Shared Folder Modal -->
        <div id="sharefolderModel" class="modal fade" role="dialog">
            <div class="modal-dialog">
                
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">New Folder</h4>
                    </div>
                    <div class="modal-body">
                        <span>Choose a file to upload:</span>
                        <input type="file" class="file-loading" />
                    </div>
                    <div class="modal-footer">
                        
                    </div>
                </div>
            
            </div>
        </div>
        <div class="FileToolBar">
            <div style="margin-right: 2.5%;">
                <a onclick="">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/FileDelete.png") %>" title="Delete Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                <a data-toggle="modal" data-target="#sharefolderModel">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/NewSharedFolder.png") %>" title="New Shared Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                <a data-toggle="modal" data-target="#folderModel">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/NewFolder.png") %>" title="New Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                <a data-toggle="modal" data-target="#uploadModel">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/Upload.png") %>" title="Upload" data-toggle="tooltip" data-placement="bottom" /></a>
            </div>
        </div>
        <div class="FileContainer">
            <h2>Files</h2>
            <ul>
                
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
