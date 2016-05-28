<%@ Page Language="C#" %>

<html>
<head>
	<title>Forms Authentication - Default Page</title>
</head>

<script runat="server">
	void Page_Load(object sender, EventArgs e) {
		FormsAuthentication.SignOut();
		Response.Redirect("SignIn.aspx");
	}

	void Signout_Click(object sender, EventArgs e) {

	}
</script>

<body>
	<h3>Logging you out...</h3>
	<form id="Form1" runat="server">
		<asp:Button ID="Submit1" OnClick="Signout_Click"
			Text="Sign Out" runat="server" /><p>
	</form>
</body>
</html>
