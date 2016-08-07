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
    <br />
    <br />
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
                <asp:GridView runat="server" ID="userstable" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" DataKeyNames="userid, username," EmptyDataText="No data available.">
                    <Columns>
                        <asp:TemplateField HeaderText="UserID">
                            <ItemTemplate>
                                <asp:Label ID="LblUserID" runat="server" Text='<%# Eval("userid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Username">
                            <ItemTemplate>
                                <asp:LinkButton ID="UsersLinkBtn" OnCommand="UsersLinkBtn_Command"  runat="server"  CommandArgument='<%# Eval("userid") %>' Text='<%# Eval("username") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
