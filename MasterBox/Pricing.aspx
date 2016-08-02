<%@ Page Title="MasterBox Pricing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pricing.aspx.cs" Inherits="MasterBox.Pricing" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function showPopup() {
            $('#myModal').modal('show');
        }
    </script>
    <div class="aboutHeadline">
        <h1 class="seizure">Pricing Plans</h1>
        <hr class="aboutHR" />
        <p class="lead">Roses are red, Violets are blue, I need more storage space, and so do you.</p>
    </div>

    <div>
        <div class="pricingLeft">
            <div class="table-responsive">
                <table class="fiveMbs table">
                    <tr class="tableHeaders">
                        <th>10MB</th>
                        <th>15MB</th>
                        <th>20MB</th>
                        <th>30MB</th>
                    </tr>
                    <tr class="tableData">
                        <td>Need more data?! WHY DO YOU NEED MORE SPACE?!<br />
                            <br />
                            <a id="ThisLogin" class="btn btn-default" runat="server" href="~/auth/signin">Sign Up Now!&raquo;</a>
                            <asp:ImageButton
                                ID="PayPalBtn10MB"
                                ItemSize="10"
                                ItemID="UK3FFRB96SXZJ"
                                runat="server"
                                ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_buynow_LG.gif" 
                                CommandName="PopUpModal"
                                OnCommand="PayPalBtn_Command"
                                CausesValidation="false"
                               /></td>
                        <td>Need more data?! WHY DO YOU NEED MORE SPACE?!<br />
                            <br />
                            <a id="LoginLink2" class="btn btn-default" runat="server" href="~/auth/signin">Sign Up Now!&raquo;</a>
                            <asp:ImageButton
                                ID="PayPalBtn15MB"
                                ItemSize="15"
                                ItemID="VDDFG6SSMYVLC"
                                runat="server"
                                ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_buynow_LG.gif"
                                CommandName="PopUpModal"
                                OnCommand="PayPalBtn_Command"
                                CausesValidation="false"
                                /></td>
                        <td>Need more data?! WHY DO YOU NEED MORE SPACE?!<br />
                            <br />
                            <a id="LoginLink3" class="btn btn-default" runat="server" href="~/auth/signin">Sign Up Now!&raquo;</a>
                            <asp:ImageButton
                                ID="PayPalBtn20MB"
                                ItemSize="20"
                                ItemID="M66YBRV8N2NBU"
                                runat="server"
                                ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_buynow_LG.gif"
                                CommandName="PopUpModal"
                                OnCommand="PayPalBtn_Command"
                                CausesValidation="false"
                                /></td>
                        <td>Need more data?! WHY DO YOU NEED MORE SPACE?!<br />
                            <br />
                            <a id="LoginLink" class="btn btn-default" runat="server" href="~/auth/signin">Sign Up Now!&raquo;</a>
                            <asp:ImageButton
                                ID="PayPalBtn30MB"
                                ItemSize="30"
                                ItemID="E3SP9YWU962SQ"
                                runat="server"
                                ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_buynow_LG.gif"
                                CommandName="PopUpModal"
                                OnCommand="PayPalBtn_Command"
                                CausesValidation="false"
                                 />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <!-- Modal -->
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
                    <br />                    
                    <asp:Label runat="server">Business ID: </asp:Label><asp:Label runat="server" ID="businesslbl"></asp:Label>
                    <br />
                    <asp:Label runat="server">Item Selected in MBs: </asp:Label><asp:Label runat="server" ID="itemNamelbl"></asp:Label>
                    <br />
                    <asp:Label runat="server">Item Price (SGD): </asp:Label><asp:Label runat="server" ID="itemAmountlbl"></asp:Label>
                    <br />
                    <asp:Label runat="server">Item ID: </asp:Label><asp:Label runat="server" ID="itemIdlbl"></asp:Label>
                    <br />
                    <asp:Label ID="Msg" ForeColor="red" runat="server" />
                    <asp:RequiredFieldValidator id="RequiredFieldValidator1" runat="server"
							ControlToValidate="OTPValue"
							ValidatorGroup="valGroup1"
							ErrorMessage="Please ensure your OTP is entered."
							ForeColor="Red">
						</asp:RequiredFieldValidator>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="cancel" runat="server" class="btn btn-warning" Text="Cancel" data-dismiss="modal" CausesValidation="false"/>
                    <asp:Button runat="server" class="btn btn-success" Text="Submit" OnCommand="PayPalBtn_Command" CommandName="PayForMember"/>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
