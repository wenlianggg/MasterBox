﻿<%@ Page Title="MasterBox Pricing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pricing.aspx.cs" Inherits="MasterBox.Pricing" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<div class="aboutHeadline">
		<h1>Pricing Plans</h1>
		<hr class="aboutHR" />
		<p class="lead">Roses are red, Violets are blue, I need more storage space, and so do you.</p>
	</div>

    <div>
        <div class="pricingLeft">
            <table class="fiveMbs">
                <tr class="tableHeaders">
                    <th>5MB</th>
                    <th>10MB</th>
                    <th>15MB</th>
                    <th>20MB</th>
                </tr>
                <tr class="tableData">
                    <td>Need more data?! WHY DO YOU NEED MORE SPACE?! Priced at the cheap cheap price of $100SGD

                    <br />
                    <br />

                    <asp:ImageButton
                    ID="PayPalBtn"
                    runat="server"
                    ImageUrl="https://www.paypalobjects.com/en_GB/i/btn/btn_buynow_LG.gif"
                    onclick="PayPalBtn_Click" />

                    </td>
                    <td>Need more data?! WHY DO YOU NEED MORE SPACE?!</td>
                    <td>Need more data?! WHY DO YOU NEED MORE SPACE?!</td>
                    <td>Need more data?! WHY DO YOU NEED MORE SPACE?!</td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
