﻿<%@ Page Language="C#" Title="" MasterPageFile="~/Internal.Master" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>


<asp:Content ContentPlaceHolderID="HeadContent" runat="server" ID="FileTrfPH">
    <script type="text/javascript">
        function showPopupFile() {
            $('#fileModal').modal('show');
        }   
        function showPopupFileName(){
            $('#filenameModal').modal('show');
        }
        function showPopupFolderFileName(){
            $('#folderfilenameModal').modal('show');
        }
        function showPopupFolderFile(){
            $('#folderfileModal').modal('show'); 
        }
        function showPopupFolder(){
            $('#folderModal').modal('show');
        }
        function showPopupPassword() {
            $('#folderPasswordModal').modal('show');
        }
        function showPopupFolderShare(){
            $('#filesharemodal').modal('show');
        }
    </script>
</asp:Content>

<asp:Content ID="FileTransferNavBar" ContentPlaceHolderID="NavBar" runat="server">
    <li><a runat="server" id="FBItem" href="~/prefs/prefgeneral.aspx">
        <asp:Label ID="Preferences" runat="server" Text="Preferences" /></a></li>
</asp:Content>

<asp:Content ID="LoginIn" ContentPlaceHolderID="InternalContent" runat="server">

    <!--Upload Modal -->
    <div id="uploadModel" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title">Upload a File</h3>
                </div>
                <div class="modal-body">
                    <span>Choose a file to upload:</span>
                    <asp:FileUpload ID="FileUpload" runat="server" />
                    <asp:RequiredFieldValidator ID="FildUploadValidator" runat="server"
                        ValidationGroup="UploadFileValidation"
                        ValidateEmptyText="true"
                        ControlToValidate="FileUpload"
                        ErrorMessage="Please select a file"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                    <br />
                    <span>Choose Location: </span>
                    <asp:DropDownList ID="UploadLocation" runat="server"
                        AutoEventWireup="true" EnableViewState="true">
                    </asp:DropDownList>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="NewUploadFile" CssClass="btn btn-default" runat="server" Text="Upload" OnClick="NewUploadFile_Click" ValidationGroup="UploadFileValidation" AutoPostBack="true" />
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
                    <span>Folder Name:</span>
                    <asp:TextBox ID="FolderName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="FolderNameValidator" runat="server"
                        ValidationGroup="NewFolder"
                        ControlToValidate="FolderName"
                        ErrorMessage="Please Enter A Folder Name"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                    <br />
                    <span>Personal Encryption: </span>
                    <asp:RadioButtonList ID="encryptionOption" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Yes" Value="yes" Selected="True" />
                        <asp:ListItem Text="No" Value="no" />
                    </asp:RadioButtonList>

                    <asp:RequiredFieldValidator ID="EncryptionOptionValidator" runat="server"
                        ValidationGroup="NewFolder"
                        ControlToValidate="encryptionOption"
                        ErrorMessage="Please select encryption option"
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>

                    <br />
                    <span>Password: </span>
                    <asp:TextBox ID="encryptionPass" CssClass="pwdfield form-control"
                        TextMode="Password" runat="server" />
                    <asp:RegularExpressionValidator ID="PassValid" runat="server"
                        ControlToValidate="encryptionPass"
                        ValidationGroup="NewFolder" Enabled="true"
                        ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$"
                        ErrorMessage="Password must contain: Minimum 8 characters atleast 1 Alphabet and 1 Number"
                        ForeColor="Red" />
                    <br />
                    <span>Confirm-Password: </span>
                    <asp:TextBox ID="encryptionPassCfm" CssClass="pwdfield form-control"
                        TextMode="Password" runat="server" onchange="validateCfmPassword(this)" />
                    <asp:RequiredFieldValidator ID="CfmPasswordValidator" runat="server"
                        ValidationGroup="NewFolder"
                        ControlToValidate="encryptionPassCfm"
                        ErrorMessage="Password does not match"
                        ForeColor="Red" Enabled="false">
                    </asp:RequiredFieldValidator>
                    <asp:Label ID="Test" runat="server"></asp:Label>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="NewFolder" runat="server" CssClass="btn btn-default" Text="Create" OnClick="CreateNewFolder_Click" ValidationGroup="NewFolder" AutoPostBack="true" />
                </div>
            </div>

        </div>
    </div>


    <!--Open File in MasterFolder Modal -->
    <div id="fileModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Delete or download?</h4>
                </div>
                <div class="modal-body">
                    <asp:Table ID="MasterFileModalTable" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableCell>File ID: </asp:TableCell>
                            <asp:TableCell><asp:Label ID="LblFileID" runat="server"></asp:Label></asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>File Name: </asp:TableCell>
                            <asp:TableCell><asp:Label ID="LblFileName" runat="server"></asp:Label></asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>File Type: </asp:TableCell>
                            <asp:TableCell><asp:Label ID="LblFileType" runat="server"></asp:Label></asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>File Size: </asp:TableCell>
                            <asp:TableCell><asp:Label ID="LblFileSize" runat="server"></asp:Label><span> MB</span></asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>Last Modified: </asp:TableCell>
                            <asp:TableCell><asp:Label ID="LblFileTimeStamp" runat="server"></asp:Label></asp:TableCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnDownload" CssClass="btn btn-default" CommandName="Download" runat="server" Text="Download" OnCommand="File_Command" />
                    <asp:Button ID="BtnDelete" CssClass="btn btn-default" OnClientClick="return confirm('Are you sure?');" CommandName="Delete" runat="server" Text="Delete" OnCommand="File_Command" />
                </div>
            </div>
        </div>
    </div>

    <!--Open File in Folder Modal -->
    <div id="folderfileModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Delete or download?</h4>
                </div>
                <div class="modal-body">
                    <span>Folder Name: </span>
                    <asp:Label ID="LblFileFolderName" runat="server"></asp:Label>
                    <span>File ID: </span>
                    <asp:Label ID="LblFolderFileId" runat="server"></asp:Label>
                    <br />
                    <span>File Name: </span>
                    <asp:Label ID="LblFolderFileName" runat="server"></asp:Label>
                    <br />
                    <span>File Type: </span>
                    <asp:Label ID="LblFolderFileType" runat="server"></asp:Label>
                    <br />
                    <span>File Size: </span>
                    <asp:Label ID="LblFolderFileSize" runat="server"></asp:Label><span> MB</span>
                    <br />
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnDownloadFileFolder" CssClass="btn btn-default" CommandName="DownloadFolderFile" OnCommand="FileFolder_Command"  runat="server" Text="Download"  />
                    <asp:Button ID="BtnDeleteFileFolder" CssClass="btn btn-default" CommandName="DeleteFolderFile"  OnCommand="FileFolder_Command" OnClientClick="return confirm('Are you sure?');" runat="server" Text="Delete"/>
                </div>
            </div>
        </div>
    </div>

     <!--Open Checking of File Name Modal -->
     <div id="filenameModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Upload</h4>
                </div>
                <div class="modal-body">
                    <p>File upload already exist, do you wish to overwrite it or upload new?</p>
                    <asp:RadioButtonList ID="RdBtnFileName" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Change" Value="change" Selected="True" />
                        <asp:ListItem Text="Overwrite" Value="overwrite" />
                    </asp:RadioButtonList>
                    <asp:Label ID="LblFileIDCheck" runat="server" Visible="false"></asp:Label>
                    <span>Current file name: </span>
                    <asp:TextBox ID="TxtBoxCurrentFileName" runat="server" Enabled="false"></asp:TextBox>
                    <br />
                    <br />
                    <span>New file name: </span>
                    <asp:TextBox ID="TxtBoxFileNameCheck" runat="server"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnUploadFile" OnClick="BtnUploadFile_Click" CssClass="btn btn-default" runat="server" Text="Upload" ValidationGroup="CheckFileName"  />
                </div>
            </div>
        </div>
    </div>

    <!--Open Checking of File Name in folder Modal -->
     <div id="folderfilenameModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Upload</h4>
                </div>
                <div class="modal-body">
                    <p>File upload already exist, do you wish to overwrite it or upload new?</p>
                    <asp:RadioButtonList ID="RdBtnFolderFileName" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Change" Value="change" Selected="True" />
                        <asp:ListItem Text="Overwrite" Value="overwrite" />
                    </asp:RadioButtonList>
                    <asp:Label ID="LblFileFolderNameCheck" runat="server" Visible="false"></asp:Label>
                    <asp:Label ID="LblFileFolderIDCheck" runat="server" Visible="false"></asp:Label>
                    <span>Current file name: </span>
                    <asp:TextBox ID="TxtBoxCurrecntFolderFileName" runat="server" Enabled="false"></asp:TextBox>
                    <br />
                    <br />
                    <span>New file name: </span>
                    <asp:TextBox ID="TxtBoxFolderFileNameCheck" runat="server"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnUploadFileToFolder" OnClick="BtnUploadFileToFolder_Click" CssClass="btn btn-default" runat="server" Text="Upload"  ValidationGroup="CheckFolderFileName" />
                </div>
            </div>
        </div>
    </div>

    <!--Open Folder without password Modal -->
    <div id="folderModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4>Folder</h4>
                </div>
                <div class="modal-body">
                    <asp:Table ID="FolderWithoutPassTable" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableCell>Folder Name: </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblFolderName" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>Date Created: </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblFolderTimeStamp" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    <asp:Label ID="LblFolderID" runat="server" Visible="False"></asp:Label>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnOpenFolder" CommandName="OpenFolder" OnCommand="BtnFolderWithoutPass_Command" CssClass="btn btn-default" runat="server" Text="Open" />
                    <asp:Button ID="BtnDeleteFolder" CommandName="DeleteFolder" OnCommand="BtnFolderWithoutPass_Command" OnClientClick="return confirm('Are you sure?');" CssClass="btn btn-default" runat="server" Text="Delete" />
                </div>
            </div>
        </div>
    </div>

    <!--Open Folder with password Modal -->
    <div id="folderPasswordModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4>Folder</h4>
                </div>
                <div class="modal-body">
                    <asp:Table ID="FolderPasswordModalTable" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableCell>Folder Name: </asp:TableCell>
                            <asp:TableCell><asp:Label ID="LblFolderNamePass" runat="server"></asp:Label></asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>Password: </asp:TableCell>
                            <asp:TableCell><asp:TextBox TextMode="Password" ID="TxtBoxPassword" runat="server"></asp:TextBox></asp:TableCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnCheckPasswordFolder" CommandName="OpenFolder" OnCommand="BtnFolderWithPass_Command" runat="server" CssClass="btn btn-default" Text="Open" />
                    <asp:Button ID="BtnDeleteFolderWithPassw" CommandName="DeleteFolder" OnCommand="BtnFolderWithPass_Command"  runat="server" CssClass="btn btn-default" Text="Delete" />
                </div>
            </div>
        </div>
    </div>

    <!--Folder Sharing Modal -->
    <div id="filesharemodal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4>Folder Sharing</h4>
                </div>
                <div class="modal-body">
                    <span>Enter recipient's username: </span>
                    <asp:TextBox ID="TextBoxEmailShare" runat="server"></asp:TextBox>
                    <br />
                </div>
                <div class="modal-footer">
                    <asp:Button ID="BtnShare" OnCommand="SendShare" CssClass="btn btn-default" runat="server" Text="Share" />
                </div>
            </div>
        </div>
    </div>

    <div class="MainContent">
        <div class="FileToolBar">
            <div style="margin-right: 2.5%;">
                <asp:LinkButton ID="CreateNewFolder" runat="server" data-toggle="modal" data-target="#folderModel" data-backdrop="static">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/NewFolder.png") %>" title="New Folder" data-toggle="tooltip" data-placement="bottom" />
                </asp:LinkButton>
                <asp:LinkButton ID="UploadFile" runat="server" data-toggle="modal" data-target="#uploadModel">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/Upload.png") %>" title="Upload" data-toggle="tooltip" data-placement="bottom" data-backdrop="static" />
                </asp:LinkButton>
            </div>
        </div>

        <div class="FileContainer">
            <div class="page-header">
                <h1>Files</h1>
            </div>

            <asp:GridView ID="FileTableView" CssClass="datagrid" HeaderStyle-CssClass="datagridHeader" RowStyle-CssClass="datagridRows" runat="server" AutoGenerateColumns="False" DataKeyNames="fileid, filename" ShowHeaderWhenEmpty="True">
                <Columns>
                    <asp:TemplateField HeaderText="Master Folder">
                        <ItemTemplate>
                            <asp:LinkButton ID="FileLinkButton" CommandName="ShowPopup" OnCommand="File_Command" CommandArgument='<%# Eval("fileid") %>' runat="server" Text='<%# Eval("filename") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />

            <asp:GridView ID="FolderTableView" CssClass="datagrid" HeaderStyle-CssClass="datagridHeader" RowStyle-CssClass="datagridRows" runat="server" AutoGenerateColumns="False" DataKeyNames="foldername,folderencryption" ShowHeaderWhenEmpty="True">
                <Columns>
                    <asp:TemplateField HeaderText="Folders">
                        <ItemTemplate>
                            <asp:LinkButton ID="FolderLinkButton" OnCommand="FolderLinkButton_Command" runat="server" CommandArgument='<%# Eval("folderid") %>' FolderEncryption='<%# Eval("folderencryption") %>' Text='<%# Eval("foldername") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />

            <asp:GridView ID="SharedFolderTableView" CssClass="datagrid" HeaderStyle-CssClass="datagridHeader" RowStyle-CssClass="datagridRows" runat="server" AutoGenerateColumns="False" DataKeyNames="foldername,folderencryption" ShowHeaderWhenEmpty="True">
                <Columns>
                    <asp:TemplateField HeaderText="Shared Folders">
                        <ItemTemplate>
                            <asp:LinkButton ID="FolderLinkButton" OnCommand="SharedFolderLinkButton_Command" runat="server" CommandArgument='<%# Eval("folderid") %>' FolderEncryption='<%# Eval("folderencryption") %>' Text='<%# Eval("foldername") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
        </div>

        <div class="FileTreeContainer">
            <asp:Label ID="FolderHeader" runat="server" Font-Size="XX-Large"></asp:Label>
            <br />
            <asp:GridView ID="FolderFileTableView" CssClass="datagrid" 
                HeaderStyle-CssClass="datagridHeader" 
                RowStyle-CssClass="datagridRows" runat="server" 
                AutoGenerateColumns="False" 
                DataKeyNames="fileid,filename,filesize,filetimestamp" 
                ShowHeaderWhenEmpty="True">
                <Columns>
                    <asp:TemplateField HeaderText="File-Name" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Large">
                        <ItemTemplate>
                            <asp:LinkButton ID="LnkFolderFile" CommandName="OpenFolderFile" OnCommand="FileFolder_Command" CommandArgument='<%# Eval("fileid") %>' Text='<%# Eval("filename") %>' FolderID='<%# Eval("folderid") %>' runat="server" ></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="File-Size" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Large">
                        <ItemTemplate>
                            <asp:Label ID="LblFolderFilesize" runat="server" Text='<%# Eval("filesize") %>'></asp:Label> KB                         
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Last Modified" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Large">
                        <ItemTemplate>
                            <asp:Label ID="LblFolderFileTimeStamp" runat="server" Text='<%# Eval("filetimestamp") %>'></asp:Label>                          
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Button ID="BtnShareToOther" OnCommand="BtnShareFolder" runat="server" CssClass="btn btn-default" Text="Share Folder" Visible ="false" />
            <asp:Label ID ="TempFolder" runat="server" Visible="false"></asp:Label>
        </div>
    </div>
    <script>
        // Toggle for encryption option
        function encryptionChk(val) {
            if (val == "no") {
                $(".pwdfield").attr('readonly', "readonly");
                $(".pwdfield").attr('disabled', "disabled");
                <%= PassValid.ClientID %>.enabled = "False";           
            } else {
                $(".pwdfield").removeAttr('readonly');
                $(".pwdfield").removeAttr('disabled');
                <%= PassValid.ClientID %>.enabled = "True";
            }
        }

        $(function () {
            $("input[type='radio']").on('click', function (e) {
                getCheckedRadio($(this).attr("name"), $(this).val(), this.checked);
            });
        });
        function getCheckedRadio(group, item, value) {
            encryptionChk(item);
        }

        // To validate Confirm Password
        $(document).ready(function () {
            $('#<%=NewFolder.ClientID %>').click(function (event) {
                var pass = document.getElementById('<%=encryptionPass.ClientID%>').value;
                var passcfm = document.getElementById('<%=encryptionPassCfm.ClientID%>').value
                if (pass != passcfm) {
                    alert("Please confirm password again");
                    return false;
                } else {
                    return true;
                }
            });
        });                     
    </script>
</asp:Content>
