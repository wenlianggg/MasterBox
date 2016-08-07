<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="subsmgmt.aspx.cs" Inherits="MasterBox.Admin.SubsMgmt" %>

<asp:Content ID="SubsMgmtContent" ContentPlaceHolderID="AdminPanelContentPH" runat="server">
        <script>
        function showPopup() {
            $('#myModal').modal('show');
        }
    </script>
    <br />
    <br />
    <div class="panel panel-info">
        <div class="panel-heading"><strong>Manage User Subscriptions</strong></div>
        <div class="panel-body">
            <asp:GridView runat="server" ID="UserTable" class="table table-bordered" OnSelectedIndexChanged="UsrSelect" EnablePersistedSelection="false">
                <SelectedRowStyle BackColor="LightCyan" ForeColor="DarkBlue" Font-Bold="true" />
                <Columns>
                    <asp:CommandField ShowSelectButton="true" HeaderText="Select User" SelectText="Select" />
                </Columns>
            </asp:GridView>
            <br />
            <h4>
                <asp:Label runat="server" ID="SelectedUsrlbl" Class="label label-success"></asp:Label></h4>
            <asp:Label runat="server" ID="MbrTypelbl">Member Type:</asp:Label><asp:TextBox runat="server" ID="MbrTypeTxtBox" CssClass="form-control" Enabled="false"></asp:TextBox>
            <br />
            <asp:Label runat="server" ID="MbrStartlbl">Subscription Start:</asp:Label><asp:TextBox ID="StartDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            <br />
            <asp:Label runat="server" ID="MbrExplbl">Subscription Expiry:</asp:Label><asp:TextBox ID="EndDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
            <br />
            <asp:Button runat="server" ID="SaveChanges" class="btn btn-success" Text="Save Changes" OnCommand="ConfirmChanges" CommandName="PopUpModal" CausesValidation="false"/>
            <asp:Button runat="server" ID="DiscardSelection" OnClick="Discard" Text="Discard" class="btn btn-danger" Text5="Discard" CausesValidation="false"/>
        </div>
    </div>
    <div class="panel panel-info">
        <div class="panel-heading"><strong>Coupons Management</strong></div>
        <div class="panel-body">
            <asp:GridView runat="server" ID="CouponTable" class="table table-bordered" OnRowDataBound="CouponRowDataBound" OnSelectedIndexChanged="CpnSelect" EnablePersistedSelection="false">
                <SelectedRowStyle BackColor="LightCyan" ForeColor="DarkBlue" Font-Bold="true" />
                <Columns>
                    <asp:CommandField ShowSelectButton="true" HeaderText="Select Coupon" SelectText="Select" CausesValidation="false"/>
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
                <asp:Button runat="server" ID="Remove" Text="Delete Coupon!" CssClass="btn btn-danger" CausesValidation="false" OnClick="RemoveCoupon" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Grp1" ControlToValidate="CouponValue" ErrorMessage="Please check if you have selected number of days to add or if you have generated the coupon code!"></asp:RequiredFieldValidator>
            </div>
        </div>
    </div>
    <div class="panel panel-info">
        <div class="panel-heading"><strong>Send a coupon to a lucky user!</strong></div>
        <div class="panel-body">
            <h4>
                <asp:Label runat="server" ID="userlbl"></asp:Label></h4>
            <asp:TextBox runat="server" Style="margin-top: 5px; margin-bottom: 5px;" ID="username" CssClass="form-control" Enabled="false"></asp:TextBox>
            <asp:DropDownList runat="server" ID="Unredeemed" CssClass="form-control" AppendDataBoundItems="true">
                <asp:ListItem Value="0" Text="Select a coupon to send!" Selected="True"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Button runat="server" ID="RandUser" OnClick="GetRandUser" CssClass="btn btn-info" Text="Send to a a random lucky user!" CausesValidation="false"/>
        </div>
    </div>

        <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Please enter your OTP to continue</h4>
                </div>
                <div class="modal-body">
                    <asp:TextBox runat="server" ID="OTPValue" CssClass="form-control"></asp:TextBox>   
                    <asp:RequiredFieldValidator id="RequiredFieldValidator2" runat="server"
							ControlToValidate="OTPValue"
							ValidatorGroup="val2"
							ErrorMessage="Please ensure your OTP is entered."
							ForeColor="Red">
						</asp:RequiredFieldValidator>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="cancel" runat="server" class="btn btn-warning" Text="Cancel" ValidationGroup ="val2" data-dismiss="modal" CausesValidation="false"/>
                    <asp:Button runat="server" class="btn btn-success" Text="Submit" OnCommand="ConfirmChanges" CommandName="Confirm"/>
                </div>
            </div>

        </div>
    </div>
</asp:Content>

