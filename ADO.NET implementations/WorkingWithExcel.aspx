<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkingWithExcel.aspx.cs" Inherits="ADO.NET_implementations.WorkingWithExcel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
<div>  
       <%-- file upload control, using to upload the file which will be read and get file information--%>  
       <asp:FileUpload ID="fileSelect" runat="server" />    
       <br />
       <br />
       <%-- click this button to run read method--%>  
       <asp:Button ID="btnRead" runat="server" Text="ReadStart" OnClick="btnRead_Click" />  
       <br />
       <br />
       <asp:GridView ID="GridView1" runat="server">
       </asp:GridView>
</div> 
    </form>
</body>
</html>
