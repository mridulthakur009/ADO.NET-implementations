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
    public partial class CacheDataSet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLoadData_Click(object sender, EventArgs e)
        {
            if (Cache["Data"] == null)
            {
                string cs = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                using(SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter sda = new SqlDataAdapter("Select * from tblProductInventory", con);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);

                    gvProducts.DataSource = ds;
                    gvProducts.DataBind();

                    Cache["Data"] = ds;
                    lblMessage.Text = "Data loadad from the database";
                }
            }
            else
            {
                gvProducts.DataSource = (DataSet)Cache["Data"];
                gvProducts.DataBind();
                lblMessage.Text = "Data is loaded from Cache";
            }
        }

        protected void btnClearnCache_Click(object sender, EventArgs e)
        {
            if(Cache["Data"] != null)
            {
                Cache.Remove("Data");
                lblMessage.Text = "DataSet removed from Cache";
            }
            else
            {
                lblMessage.Text = "There is nothing in the cache to remove";
            }
        }
    }
}