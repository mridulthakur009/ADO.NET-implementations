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
    public partial class dSImplementations : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string CS = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlDataAdapter da = new SqlDataAdapter("spGetProductAndCategoriesData", con);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                da.Fill(ds);

                //Renaming the datatable
                ds.Tables[0].TableName = "Product";
                ds.Tables[1].TableName = "Category";

                //GridViewProducts.DataSource = ds.Tables[0]; //We can call data table from their indexes too
                GridViewProducts.DataSource = ds.Tables["Product"];

                GridViewProducts.DataBind();

                //GridViewCategories.DataSource = ds.Tables[1]; //We can call data table from their indexes too
                GridViewCategories.DataSource = ds.Tables["Category"];
                                
                GridViewCategories.DataBind();

            }

        }
    }
}