<%@ Page Title="User Dashboard" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="prefgeneral.aspx.cs" Inherits="MasterBox.Prefs.FileSetting_General" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="SetGeneral" ContentPlaceHolderID="Preferences" runat="server">
    <div class="page-header">
        <h1><%: Page.Title %>
        </h1>
    </div>
    <ol class="breadcrumb" style="margin-bottom: 5px;">
        <li><a href="<%= ResolveUrl("~/Default") %>">MasterBox</a></li>
        <li>User Preferences</li>
        <li class="active"><%: Page.Title %></li>
    </ol>
    <div class="row">
        <div class="panel panel-primary">
            <div class="panel-heading">
			    <h3 class="panel-title">
                   Data Summary
			    </h3>
		    </div>
            <asp:Chart ID="DataChart" runat="server" Width="1100px" Height="400px"
                Palette="SemiTransparent" TextAntiAliasingQuality="Normal" ImageStorageMode="UseImageLocation" Visible="True">
                <Series>
                    <asp:Series Name="Date" ChartType="Line"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1" BackColor="Transparent"></asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>

    </div>
</asp:Content>

