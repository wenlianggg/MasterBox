<%@ Page Title="IP Address Blocking" Language="C#" MasterPageFile="~/admin/Admin.master" AutoEventWireup="true" CodeBehind="ipblocking.aspx.cs" Inherits="MasterBox.Admin.IPBlocking" %>
<asp:Content ContentPlaceHolderID="AdminPanelContentPH" ID="PanelContent" runat="server">
	<br /><br />
    <!--- Block users interface --->
    <!--- Block by IP only --->
	<div ID="Message" Visible="false" runat="server">
		<strong>Result:</strong>
		<asp:Label runat="server" ID="Msg" />
	</div>
	<div class="col-md-4 col-sm-12">
		<div class="panel panel-danger">
			<div class="panel-heading">
				<strong>Block user by IP address</strong>
			</div> 
			<div class="panel-body">
				IP Address:
				<asp:TextBox runat="server" ID="IPAddrTxt" CssClass="form-control" /><br />
				Duration:
				<asp:TextBox runat="server" ID="IPDurationTxt" CssClass="form-control" /><br />
				Reason:
				<asp:TextBox runat="server" ID="IPReasonTxt" CssClass="form-control" /><br />
				<asp:Button runat="server" ID="IPSubmitBtn" Text="Block" CssClass="btn btn-danger" OnClick="IPSubmitBtn_Click"/>
			</div>
		</div>
	</div>
    <!--- Block by Username only --->
	<div class="col-md-4 col-sm-12">
		<div class="panel panel-danger">
			<div class="panel-heading">
				<strong>Block user by username</strong>
			</div> 
			<div class="panel-body">
				Username:
				<asp:TextBox runat="server" ID="UUserTxt" CssClass="form-control" /><br />
				Duration:		
				<asp:TextBox runat="server" ID="UDurationTxt" CssClass="form-control" /><br />
				Reason:
				<asp:TextBox runat="server" ID="UReasonTxt" CssClass="form-control" /><br />			
				<asp:Button runat="server" ID="USubmitBtn" Text="Block" CssClass="btn btn-danger" OnClick="USubmitBtn_Click"/>
			</div>
		</div>
	</div>
	<!--- Block by IP + User combination --->
	<div class="col-md-4 col-sm-12">
		<div class="panel panel-danger">
			<div class="panel-heading">
				<strong>Block user by Username and IP</strong>
			</div> 
			<div class="panel-body">
				Username:
				<asp:TextBox runat="server" ID="IPUUserTxt" CssClass="form-control" /><br />
				IP Address:
				<asp:TextBox runat="server" ID="IPUIPTxt" CssClass="form-control" /><br />
				Duration:
				<asp:TextBox runat="server" ID="IPUDurationTxt" CssClass="form-control" /><br />
				Reason:
				<asp:TextBox runat="server" ID="IPUReasonTxt" CssClass="form-control" /><br />
				<asp:Button runat="server" ID="IPUSubmitBtn" Text="Block" CssClass="btn btn-danger" OnClick="IPUSubmitBtn_Click"/>
			</div>
		</div>
	</div>

	<!--- List of IPBlocked --->
    <div class="col-md-12 col-sm-12">

    </div>
</asp:Content>