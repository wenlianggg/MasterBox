<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="subsmgmt.aspx.cs" Inherits="MasterBox.Admin.SubsMgmt" %>

<asp:Content ID="SubsMgmtContent" ContentPlaceHolderID="AdminPanelContentPH" runat="server">
    <br />
    <br />
    <div class="panel panel-info">
        <div class="panel-heading"><strong>Manage User Subscriptions</strong></div>
        <div class="panel-body">
        </div>
    </div>
    <div class="panel panel-info">
        <div class="panel-heading"><strong>Coupons Management</strong></div>
        <div class="panel-body">
            <asp:GridView runat="server" ID="CouponTable" class="table table-bordered" OnRowDataBound="CouponRowDataBound" OnSelectedIndexChanged="Selected" EnablePersistedSelection="false">
                <SelectedRowStyle BackColor="LightCyan" ForeColor="DarkBlue" Font-Bold="true" />
                <Columns>
                    <asp:CommandField ShowSelectButton="true" HeaderText="Select Coupon" SelectText="Select" />
                </Columns>
            </asp:GridView>
            <asp:TextBox ID="CouponValue" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
            <asp:DropDownList ID="Days" CssClass="form-control" runat="server">
                <asp:ListItem Value="0">No. Of Days</asp:ListItem>
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
                <asp:ListItem>16</asp:ListItem>
                <asp:ListItem>17</asp:ListItem>
                <asp:ListItem>18</asp:ListItem>
                <asp:ListItem>19</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>21</asp:ListItem>
                <asp:ListItem>22</asp:ListItem>
                <asp:ListItem>23</asp:ListItem>
                <asp:ListItem>24</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="Couponlbl" runat="server" CssClass="label label-danger"></asp:Label>
            <asp:Label ID="InvisCpnLbl" runat="server" Visible="false"></asp:Label>
            <br />
            <asp:Button runat="server" ID="Generate" Text="Generate A Coupon Code" CssClass="btn btn-default-blue" OnClick="GenerateCode" CausesValidation="false" Style="margin-top: 5px; margin-bottom: 5px;" />
            <div class="CancelAdd" style="margin-top: 5px; margin-bottom: 5px;">
                <asp:Button runat="server" ID="Add" Text="Add Coupon!" CssClass="btn btn-success" OnClick="AddCoupon" ValidationGroup="Grp1" />
                <asp:Button runat="server" ID="Remove" Text="Delete Coupon!" CssClass="btn btn-danger" CausesValidation="false" OnClick="RemoveCoupon"/>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Grp1" ControlToValidate="CouponValue" ErrorMessage="Please check if you have selected number of days to add or if you have generated the coupon code!"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="panel panel-info">
        <div class="panel-heading"><strong>Send a coupon to a lucky user!</strong></div>
        <div class="panel-body">
            <h4><asp:Label runat="server" ID="userlbl"></asp:Label></h4>
            <asp:TextBox runat="server" style="margin-top: 5px; margin-bottom: 5px;" ID="username" CssClass="form-control" Enabled="false"></asp:TextBox>
            <asp:DropDownList runat="server" ID="Unredeemed" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Value="0" Text="Select a coupon to send!" Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Button runat ="server" ID="RandUser" OnClick="GetRandUser" CssClass="btn btn-info" Text="Send to a a random lucky user!"/>
        </div>
    </div>
</asp:Content>

