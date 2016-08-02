<%@ Page Title="Image Key" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="prefsteg.aspx.cs" Inherits="MasterBox.Prefs.Steg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Preferences" runat="server">
	<div class="page-header">
		<h1><%: Page.Title %>
			<small>Use an image based key for resetting passwords</small>
		</h1>
	</div>
	<div class="panel panel-info">
		<div class="panel-heading">
			<h3 class="panel-title"><i class="fa fa-picture-o" aria-hidden="true"></i> Image Based Key for Password Resetting</h3>
		</div>
		<div class="panel-body">
			<strong>Use an image as your backup password reset key</strong><br />
			<ol>
				<li>Upload an image to be modified randomly, but look exactly the same.</li>
				<li>Download the image and save it in a safe location.</li>
				<li>Upload the modified image and set it as your Image Key.</li>
				<li>Use the Image Key in case you forget your password, 2FA will still be required.</li>
				<li>Verify that your Image Key is valid using our verification tool.</li>
			</ol>
			<br />
			<strong>Select an image to be uploaded:</strong><br />
			<asp:FileUpload ID="UploadControl" runat="server" AllowMultiple="false" accept="image/jpeg, image/png, image/bmp, image/gif" />
			<br />
			<div class="panel panel-default">
				<div class="panel-heading">
					<h3 class="panel-title">Key Setup</h3>
				</div>
				<div class="panel-body">
					<asp:Button CssClass="btn btn-default" ID="SubmitForStegSetKey" Text="Upload, Set as Key" OnClick="UploadDoStegSetKey" runat="server" />
					<asp:Button CssClass="btn btn-primary" ID="DownloadFromSteg" Text="Download Image Key" OnClick="DownloadImage" runat="server" />
				</div>
			</div>

			<div class="panel panel-default">
				<div class="panel-heading">
					<h3 class="panel-title">Verify or Disable</h3>
				</div>
				<div class="panel-body">
					<asp:Button CssClass="btn btn-danger" ID="VerifyWithKey" Text="Verify with Key" OnClick="SubmitValidateImg" runat="server" />
					<asp:Button CssClass="btn btn-danger" ID="DisableKey" Text="Disable Image Key" OnClick="DisableImageKey" runat="server" />
				</div>
			</div>

			<div id="HasExisting" runat="server">
			</div>
			<asp:Label ID="Msg" runat="server" ForeColor="red" /><br />
			<asp:Label ID="HashMsg1" runat="server" ForeColor="green" /><br />
			<asp:Label ID="HashMsg2" runat="server" ForeColor="green" />
		</div>
	</div>
</asp:Content>
