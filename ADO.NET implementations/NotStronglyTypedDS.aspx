<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotStronglyTypedDS.aspx.cs" Inherits="ADO.NET_implementations.StronglyTypedDS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-family: Arial">
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

            &nbsp;&nbsp;&nbsp;

            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />

            <br />

            <asp:GridView ID="GridView1" runat="server"></asp:GridView>

        </div>
    </form>
</body>
</html>
