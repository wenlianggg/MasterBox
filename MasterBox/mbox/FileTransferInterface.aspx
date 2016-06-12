<%@ Page Language="C#" Title="" MasterPageFile="~/mbox/Internal.Master" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>

<asp:Content ID="LoginIn" ContentPlaceHolderID="InternalContent" runat="server">

    <ul class='custom-menu'>
        <li data-action="upload">Upload</li>
        <li data-action="file">New Folder</li>
        <li data-action="sharefile">New Shared Folder</li>
        <li data-action="delete">Delete</li>
    </ul>


    <div class="MainContent">
        <!-- Modal -->
        <div id="myModal" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <form id="Upload">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Upload a file</h4>
                    </div>
                    <div class="modal-body">
                        <label>Choose a file to upload:</label>
                        <input type="file" class="file-loading" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" onClick="this.form.reset()">Upload</button>
                    </div>
                </div>
            </form>
            </div>
        </div>
        <div class="FileToolBar">
            <div style="margin-right: 2.5%;">
                <a onclick="">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/FileDelete.png") %>" title="Delete Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                <a onclick="">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/NewSharedFolder.png") %>" title="New Shared Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                <a onclick="">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/NewFolder.png") %>" title="New Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                <a data-toggle="modal" data-target="#myModal">
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
