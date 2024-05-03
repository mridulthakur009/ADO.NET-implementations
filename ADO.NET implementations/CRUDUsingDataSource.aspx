<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CRUDUsingDataSource.aspx.cs" Inherits="ADO.NET_implementations.CRUDUsingDS" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-family: Arial">
            <br />
            <asp:Button ID="btnGetDataFromDB" runat="server" Text="Get Data from Database"
                OnClick="btnGetDataFromDB_Click" />
            <br />
            <br />
            <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                    <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                    <asp:BoundField DataField="Gender" HeaderText="Gender" SortExpression="Gender" />
                    <asp:BoundField DataField="TotalMarks" HeaderText="TotalMarks" SortExpression="TotalMarks" />
                </Columns>
            </asp:GridView>

            <br />

            <asp:Button ID="btnUpdateDatabaseTable" runat="server"
                Text="Update Database Table" OnClick="btnUpdateDatabaseTable_Click" />
            <br />
            <br />
            <asp:Label ID="lblStatus" runat="server"></asp:Label>
            <br />
            <br />
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DBConnectionString %>" DeleteCommand="DELETE FROM [tblStudents] WHERE [ID] = @ID" InsertCommand="INSERT INTO [tblStudents] ([Name], [Gender], [TotalMarks]) VALUES (@Name, @Gender, @TotalMarks)" SelectCommand="SELECT * FROM [tblStudents]" UpdateCommand="UPDATE [tblStudents] SET [Name] = @Name, [Gender] = @Gender, [TotalMarks] = @TotalMarks WHERE [ID] = @ID">
                <DeleteParameters>
                    <asp:Parameter Name="ID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="Name" Type="String" />
                    <asp:Parameter Name="Gender" Type="String" />
                    <asp:Parameter Name="TotalMarks" Type="Int32" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="Name" Type="String" />
                    <asp:Parameter Name="Gender" Type="String" />
                    <asp:Parameter Name="TotalMarks" Type="Int32" />
                    <asp:Parameter Name="ID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
