<%@ Page Title="Image Based Key" Language="C#" MasterPageFile="~/prefs/Preferences.master" AutoEventWireup="true" CodeBehind="prefsteg.aspx.cs" Inherits="MasterBox.Prefs.Steg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Preferences" runat="server">
	<div class="page-header">
          <h1><%: Page.Title %>
              <small>Use an image based key for resetting passwords</small>
          </h1>
    </div>
	<div class="panel panel-primary">
		<div class="panel-heading">
			<h3 class="panel-title">Image Based Key for Password Resetting <i class="fa fa-picture-o" aria-hidden="true"></i></h3>
		</div>
		<div class="panel-body">
			<strong>Description</strong><br />
			<p>Imaged based key uses an image and modifies it to become your password reset key.<br />
				With your password reset image, you can then reset your password with a new password sent to your email.
			</p>
			<strong>Select an image to be uploaded:</strong><br />
			<asp:FileUpload ID="UploadControl"  runat="server" AllowMultiple="false" accept="image/jpeg, image/png, image/bmp, image/gif" /><br />
			<asp:Button CssClass="btn btn-success" ID="Submit" Text="Upload" OnClick="SubmitFile" runat="server" />

			<br />
            <asp:Label ID="Msg" runat="server" ForeColor="red"/><br />
            <asp:Label ID="HashMsg" runat="server" ForeColor="green"/>
		</div>
	</div>
</asp:Content>
