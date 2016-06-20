﻿<%@ Page Title="MasterBox Pricing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pricing.aspx.cs" Inherits="MasterBox.Pricing" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
                        <th>5MB</th>
                        <th>10MB</th>
                        <th>15MB</th>
                        <th>20MB</th>
                    </tr>
                    <tr class="tableData">
                        <td>Need more data?! WHY DO YOU NEED MORE SPACE?!<br /><br />
                        <a ID="LoginLink" class="btn btn-default" runat="server" href="~/auth/signin">Sign Up Now!&raquo;</a>
                        <asp:ImageButton
                                ID="PayPalBtn5MB"  
                                runat="server"
                                ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_buynow_LG.gif"
                                OnClick="PayPalBtn5MB_Click" />
                        </td>
                        <td>Need more data?! WHY DO YOU NEED MORE SPACE?!<br /><br />
                        <a id="ThisLogin" class="btn btn-default" runat="server" href="~/auth/signin">Sign Up Now!&raquo;</a>
                          <asp:ImageButton
                                ID="PayPalBtn10MB"
                                runat="server"
                                ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_buynow_LG.gif"
                                OnClick="PayPalBtn10MB_Click" /></td>
                        <td>Need more data?! WHY DO YOU NEED MORE SPACE?!<br /><br />
                        <a ID="LoginLink2" class="btn btn-default" runat="server" href="~/auth/signin">Sign Up Now!&raquo;</a>
                          <asp:ImageButton
                                ID="PayPalBtn15MB"
                                runat="server"
                                ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_buynow_LG.gif"
                                OnClick="PayPalBtn15MB_Click" /></td>
                        <td>Need more data?! WHY DO YOU NEED MORE SPACE?!<br /><br />
                          <a ID="LoginLink3" class="btn btn-default" runat="server" href="~/auth/signin">Sign Up Now!&raquo;</a>
                          <asp:ImageButton
                                ID="PayPalBtn20MB"
                                runat="server"
                                ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_buynow_LG.gif"
                                OnClick="PayPalBtn20MB_Click" /></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
