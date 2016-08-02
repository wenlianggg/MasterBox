<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="subsmgmt.aspx.cs" Inherits="MasterBox.Admin.SubsMgmt" %>

<asp:Content ID="SubsMgmtContent" ContentPlaceHolderID="AdminPanelContentPH" runat="server">
    <br />
    <div class="panel panel-info">
        <div class="panel-heading">Manage User Subscriptions</div>
        <div class="panel-body">
        </div>
    </div>
    <div class="panel panel-info">
        <div class="panel-heading">Coupons Management</div>
        <div class="panel-body">
            <asp:GridView runat="server" ID="CouponTable" CssClass="table" OnRowDataBound="CouponRowDataBound" OnSelectedIndexChanged="Selected">
                   <selectedrowstyle backcolor="LightCyan" forecolor="DarkBlue" font-bold="true"/>
                <Columns>
                    <asp:CommandField ShowSelectButton="true" HeaderText="Select Coupon" SelectText="Select"/>
                </Columns>
            </asp:GridView>
            <asp:Label ID="Couponval" runat="server"></asp:Label>
            <asp:TextBox ID="CouponValue" runat="server" CssClass="form-control"></asp:TextBox>
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
            <br />
            <asp:Button runat="server" ID="Generate" Text="Generate A Coupon Code" CssClass="btn btn-default-blue" OnClick="GenerateCode" CausesValidation="false"/>
            <div class="CancelAdd" style="margin-top: 5px; margin-bottom: 5px;">
                <asp:Button runat="server" ID="Add" Text="Add Coupon!" CssClass="btn btn-success" OnClick="AddCoupon"/>
                <asp:Button runat="server" ID="Cancel" Text="Delete Coupon!" CssClass="btn btn-danger" CausesValidation="false"/>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="CouponValue" ErrorMessage="Please check if you have selected number of days to add or if you have generated the coupon code!"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
</asp:Content>

