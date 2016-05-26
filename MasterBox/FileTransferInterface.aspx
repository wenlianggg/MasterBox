<%@ Page Language="C#" Title="" MasterPageFile="~/Internal.Master" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>

<asp:Content ID="LoginIn" ContentPlaceHolderID="InternalContent" runat="server">
        <div class="MainContent">
            <div class="FileToolBar">
                <div style="margin-right: 2.5%;">
                    <a onclick="">
                        <img class="FileIcon" src="images/Logged/FileDelete.png" title="Delete Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                    <a onclick="">
                        <img class="FileIcon" src="images/Logged/NewSharedFolder.png" title="New Shared Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                    <a onclick="">
                        <img class="FileIcon" src="images/Logged/NewFolder.png" title="New Folder" data-toggle="tooltip" data-placement="bottom" /></a>
                    <a onclick="">
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
