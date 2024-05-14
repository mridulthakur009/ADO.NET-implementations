using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADO.NET_implementations
{
    public partial class LoadBulkData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection())
            {
                DataSet ds = new DataSet();
                ds.ReadXml(Server.MapPath("~/Data.xml"));

                DataTable dtDept = ds.Tables["Department"];
                DataTable dtEmp = ds.Tables["Employee"];

                con.Open();

                using (SqlBulkCopy sbc = new SqlBulkCopy(con))
                {
                    sbc.DestinationTableName = "Departments";
                    sbc.ColumnMappings.Add("ID", "ID");
                    sbc.ColumnMappings.Add("Name", "Name");
                    sbc.ColumnMappings.Add("Location", "Location");

                    sbc.WriteToServer(dtDept);
                }
                using (SqlBulkCopy sbc = new SqlBulkCopy(con))
                {
                    sbc.DestinationTableName = "Employees";
                    sbc.ColumnMappings.Add("ID", "ID");
                    sbc.ColumnMappings.Add("Name", "Name");
                    sbc.ColumnMappings.Add("Gender", "Gender");
                    sbc.ColumnMappings.Add("DepartmentId", "DepartmentId");

                    sbc.WriteToServer(dtEmp);
                }
            }
        }
    }
}