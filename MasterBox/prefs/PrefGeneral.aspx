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
                <h3 class="panel-title">Data Summary
                </h3>
            </div>
            <asp:Label ID="DataTitle" runat="server" Font-Size="Large" Text=""></asp:Label>
            <br />
            <asp:Chart ID="DataChart" runat="server" Width="1060" Height="400px" TextAntiAliasingQuality="Normal" ImageStorageMode="UseImageLocation" Visible="True">
                <Series>
                    <asp:Series Name="Data" ChartType="Bar"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="DataChartArea" BackColor="Transparent"></asp:ChartArea>
                </ChartAreas>
                <Titles>
                    <asp:Title Name="DataTitle" Text="Data Summary Chart" TextStyle="Embed" Font="Microsoft Sans Serif, 20pt"></asp:Title>
                </Titles>
            </asp:Chart>
            <br />

            <div class="panel panel-info">
                <div class="panel-heading"><h4>Storage Information</h4></div>
                <div class="panel-body">
                    <asp:Label ID="LblDataTrackerFileSize" Font-Size="Large" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="LblDataTrackerFileNum" Font-Size="Large" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="LblDataTrackerFolderNum" Font-Size="Large" runat="server"></asp:Label>
                </div>
            </div>
            
        </div>

    </div>
</asp:Content>

