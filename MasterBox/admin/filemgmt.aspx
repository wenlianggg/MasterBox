<%@ Page Title="File Management" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="filemgmt.aspx.cs" Inherits="MasterBox.Admin.FileMgmt" %>

<asp:Content ContentPlaceHolderID="AdminPanelContentPH" ID="PanelContent" runat="server">
    <style>
        #custom-search-input {
            width: 369px;
            border: solid 1px #E4E4E4;
            border-radius: 6px;
            background-color: #fff;
        }

            #custom-search-input input {
                border: 0;
                box-shadow: none;
            }
    </style>
    <script>
        function showEmailPopup() {
            $('#emailModal').modal('show');
        }
    </script>
    <br />
    <br />
    <div id="emailModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h3 class="modal-title">Send Email: Reset Password</h3>
                </div>
                <div class="modal-body">
                  <asp:Table ID="EmailModalTable" runat="server">
                      <asp:TableHeaderRow>
                          <asp:TableCell>To: </asp:TableCell>
                          <asp:TableCell>
                              <asp:TextBox ID="ToEmailTxtBox" runat="server" Enabled="false"></asp:TextBox>
                          </asp:TableCell>
                      </asp:TableHeaderRow>
                      <asp:TableHeaderRow>

                      </asp:TableHeaderRow>
                  </asp:Table>
                </div>
                <div class="modal-footer">
                    
                </div>
            </div>
        </div>
    </div>

    <div class="panel panel-info">
        <div class="panel-heading">
            <strong>Folders Settings</strong>
        </div>
        <div class="panel-body">
            <div class="col-md-6">
                <div id="custom-search-input">
                    <div class="input-group col-md-12">
                        <asp:TextBox ID="searchTxt" CssClass="form-control input-lg" runat="server" placeholder="Serach"></asp:TextBox>
                        <span class="input-group-btn">
                            <asp:Button ID="SearchBtn" OnClick="SearchBtn_Click" CssClass="btn btn-info btn-lg" runat="server" Text="Search" />
                        </span>
                    </div>
                </div>
            </div>
            <div style="padding: 15px;">
                <asp:GridView runat="server" ID="userstable" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" DataKeyNames="userid, username" EmptyDataText="No data available.">
                    <Columns>
                        <asp:TemplateField HeaderText="UserID">
                            <ItemTemplate>
                                <asp:Label ID="LblUserID" runat="server" Text='<%# Eval("userid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Username">
                            <ItemTemplate>
                                <asp:LinkButton ID="UsersLinkBtn" OnCommand="UsersLinkBtn_Command" runat="server" CommandArgument='<%# Eval("userid") %>' Text='<%# Eval("username") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <div id="InformationPanel" runat="server" class="panel panel-info" style="border-width: 0 0 0 5px; border-color: #abd4e9; background-color: #f5f5f5; margin: 1%; padding: 1px; padding-left: 1%; display:none;">
                    <h4><u>User Information</u> </h4>
                    <asp:Table ID="ViewUser" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableCell>UserID: </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblUserId" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>Username: </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblUserName" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>E-mail: </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblUserEmail" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    <h4><u>Folder Information </u></h4>
                    <asp:Table ID="ViewFolder" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableCell>Folder Name:</asp:TableCell>
                            <asp:TableCell>
                                <asp:DropDownList ID="FolderNameOption" OnSelectedIndexChanged="FolderNameOption_SelectedIndexChanged" AutoPostBack="True"  runat="server"
                                    AutoEventWireup="true" EnableViewState="true">
                                </asp:DropDownList>
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>Date Created: </asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblFolderDate" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell>Number of files in folder:</asp:TableCell>
                            <asp:TableCell>
                                <asp:Label ID="LblFileinFolderNum" runat="server"></asp:Label>
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                        <asp:TableHeaderRow>
                            <asp:TableCell></asp:TableCell>
                            <asp:TableCell>
                                <asp:LinkButton ID="LnkBtnResetPass" CssClass="btn btn-info" OnClick="LnkBtnResetPass_Click" runat="server" Text="Reset Password"></asp:LinkButton>
                            </asp:TableCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
