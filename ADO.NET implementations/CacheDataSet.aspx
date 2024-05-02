<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CacheDataSet.aspx.cs" Inherits="ADO.NET_implementations.CacheDataSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <br />
            <asp:Button ID="btnLoadData" runat="server" Text="Load Data"
                OnClick="btnLoadData_Click" />
            <asp:Button ID="btnClearnCache" runat="server" Text="Clear Cache"
                OnClick="btnClearnCache_Click" />
            <br />
            <br />
            <br />
            <asp:Label ID="lblMessage" runat="server"></asp:Label>
            <br />
            <br />
            <br />
            <asp:GridView ID="gvProducts" runat="server">
            </asp:GridView>
        </div>
    </form>
</body>
</html>
