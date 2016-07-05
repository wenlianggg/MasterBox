<%@ Page Language="C#" Title="" MasterPageFile="~/mbox/Internal.Master" AutoEventWireup="true" CodeBehind="FileTransferInterface.aspx.cs" Inherits="MasterBox.FileTransferInterface" %>

<asp:Content ID="LoginIn" ContentPlaceHolderID="InternalContent" runat="server">
    <ul class='custom-menu'>
        <li data-action="upload" data-toggle="modal" data-target="#uploadModel">Upload</li>
        <li data-action="file">New Folder</li>
        <li data-action="sharefile">New Shared Folder</li>
        <li data-action="delete">Delete</li>
    </ul>

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>

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
                        <asp:Button ID="NewUploadFile" runat="server" Text="Upload" OnClick="NewUploadFile_Click" ValidationGroup="UploadFileValidation" AutoPostBack="true" />
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
                        <asp:RequiredFieldValidator ID="PasswordValidator" runat="server"
                            ValidationGroup="NewFolder"
                            ControlToValidate="encryptionPass"
                            ErrorMessage="Please Enter a password"
                            ForeColor="Red" Enabled="true">
                        </asp:RequiredFieldValidator>
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
                        <asp:Button ID="NewFolder" runat="server" Text="Create" OnClick="CreateNewFolder_Click" ValidationGroup="NewFolder" AutoPostBack="true" />
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
                        <h4 class="modal-title">New Shared Folder</h4>
                    </div>
                    <div class="modal-body">
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="Delete" runat="server" Text="Delete" OnClick="Delete_Click" ValidationGroup="DeleteValidation" />
                    </div>
                </div>

            </div>
        </div>
        <!--Shared Folder Modal -->
        <div id="deleteModel" class="modal fade" role="dialog">
            <div class="modal-dialog">

                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Delete</h4>
                    </div>
                    <div class="modal-body">
                        <span>Choose Location: </span>
                        <asp:DropDownList ID="DeleteLocation" runat="server"
                            AutoEventWireup="true" EnableViewState="true">
                        </asp:DropDownList>
                        <br />
                        <span>Choose File to delete: </span>
                        <asp:GridView ID="DeleteView" runat="server" AutoGenerateColumns="False" DataKeyNames="filename">
                            <Columns>
                                <asp:TemplateField HeaderText="File-Name">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="FolderLinkButton" runat="server" Text='<%# Eval("filename") %>' FolderID='<%# Eval("fileid") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>


                    </div>
                    <div class="modal-footer">
                    </div>
                </div>

            </div>
        </div>



        <div class="FileToolBar">
            <div style="margin-right: 2.5%;">
                <asp:LinkButton ID="DeleteFile&Folder" runat="server" data-toggle="modal" data-target="#deleteModel">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/FileDelete.png") %>" title="Delete Folder" data-toggle="tooltip" data-placement="bottom" />
                </asp:LinkButton>
                <asp:LinkButton ID="CreateNewSharedFolder" runat="server" data-toggle="modal" data-target="#sharefolderModel">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/NewSharedFolder.png") %>" title="New Shared Folder" data-toggle="tooltip" data-placement="bottom" />
                </asp:LinkButton>
                <asp:LinkButton ID="CreateNewFolder" runat="server" data-toggle="modal" data-target="#folderModel">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/NewFolder.png") %>" title="New Folder" data-toggle="tooltip" data-placement="bottom" />
                </asp:LinkButton>
                <asp:LinkButton ID="UploadFile" runat="server" data-toggle="modal" data-target="#uploadModel">
                    <img class="FileIcon" src="<%= Page.ResolveUrl("~/images/Logged/Upload.png") %>" title="Upload" data-toggle="tooltip" data-placement="bottom" data-backdrop="static" />
                </asp:LinkButton>
            </div>
        </div>

        <div class="FileContainer">
            <h1>Files</h1>
            <asp:GridView ID="FileTableView" runat="server" AutoGenerateColumns="False" DataKeyNames="fileid, filename">
                <Columns>
                    <asp:TemplateField HeaderText="Master Folder" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Large">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="DownloadFile" Text='<%# Eval("filename") %>' FileID='<%# Eval("fileid") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:GridView ID="FolderTableView" runat="server" AutoGenerateColumns="False" DataKeyNames="foldername">
                <Columns>
                    <asp:TemplateField HeaderText="Folders" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Large">
                        <ItemTemplate>
                            <asp:LinkButton ID="FolderLinkButton" runat="server" OnClick="OpenFolder" Text='<%# Eval("foldername") %>' FolderID='<%# Eval("folderid") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />


        </div>

        <div class="FileTreeContainer">
            <asp:Label ID="FolderHeader" runat="server" Font-Size="XX-Large"></asp:Label>
            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="fileid, filename,filesize">
                <Columns>
                    <asp:TemplateField HeaderText="File-Name" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Large">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton3" runat="server" OnClick="DownloadFile" Text='<%# Eval("filename") %>' FileID='<%# Eval("fileid") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File-Size" ControlStyle-Font-Size="Medium" HeaderStyle-Font-Size="Large">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton4" runat="server" Text='<%# Eval("filesize") %>' FileID='<%# Eval("fileid") %>' Enabled="False" EnableTheming="False" EnableViewState="False"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>



        </div>
    </div>

    <script>
        // Toggle for encryption option
        function encryptionChk(val) {
            if (val == "no") {
                $(".pwdfield").attr('readonly', "readonly");
                $(".pwdfield").attr('disabled', "disabled");
                ValidatorEnable(document.getElementById('<%=PasswordValidator.ClientID%>'), false);
            } else {
                $(".pwdfield").removeAttr('readonly');
                $(".pwdfield").removeAttr('disabled');
                ValidatorEnable(document.getElementById('<%=PasswordValidator.ClientID%>'), true);

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
