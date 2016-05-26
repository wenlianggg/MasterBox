<%@ Page Language="C#" %>

<html>
<head>
	<title>Forms Authentication - Default Page</title>
</head>

<script runat="server">
	void Page_Load(object sender, EventArgs e) {
		Welcome.Text = "Hello, " + Context.User.Identity.Name;
	}

	void Signout_Click(object sender, EventArgs e) {
		FormsAuthentication.SignOut();
		Response.Redirect("Logon.aspx");
	}
</script>

<body>
	<h3>Using Forms Authentication</h3>
	<asp:Label ID="Welcome" runat="server" />
	<form id="Form1" runat="server">
		<asp:Button ID="Submit1" OnClick="Signout_Click"
			Text="Sign Out" runat="server" /><p>
	</form>
</body>
</html>
