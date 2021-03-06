﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DEBUG.aspx.cs" Inherits="MasterBox.Auth.DEBUG" %>

<!DOCTYPE html>

<script runat="server">
	


</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        WL's Features: <br />
        <ol>
            <li>Login Authentication</li>
            <li>User Registration</li>
            <li>Time based OTP Backend Implementation</li>
            <li>Encrypted user data store</li>
            <li>Account access logs</li>
            <li>IP address blocking based on failed login attempts</li>
            <li>Google reCAPTCHA Implementation</li>
            <li>Convinient user entity class</li>
            <li>MembershipProvider implementation for credential verification</li>
            <li>Database access class</li>
            <li>Database hosting setup and design (with roy)</li>
            <li>Microsoft azure hosting with SSL</li>
        </ol>
        TODO: <br />
        <ol>
            <li>Facial recognition features</li>
            <li>Complete registration features</li>
        </ol>
	<asp:Button runat="server" OnClick="btnEncrypt_Click" Text="Encrypt" />
	<asp:Button runat="server" OnClick="btnDecrypt_Click" Text="Decrypt" />
    </div>
    </form>
</body>
</html>
