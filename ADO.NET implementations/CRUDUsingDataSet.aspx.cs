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
    public partial class CRUDUsingDataSet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void GetDataFromDB()
        {
            string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection con = new SqlConnection(CS);

            string strSelectQuery = "Select * from tblStudents";

            SqlDataAdapter da = new SqlDataAdapter(strSelectQuery, con);

            DataSet ds = new DataSet();
            da.Fill(ds, "Students");

            ds.Tables["Students"].PrimaryKey = new DataColumn[] { ds.Tables["Students"].Columns["ID"] };
            Cache.Insert("DATASET", ds, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);

            gvStudents.DataSource = ds;
            gvStudents.DataBind();
        }

        private void GetDataFromCache()
        {
            if(Cache["DATASET"] != null)
            {
                DataSet ds = new DataSet();

                gvStudents.DataSource = ds;
                gvStudents.DataBind();
            }
        }

        protected void GetDataFromDb_Click(object sender, EventArgs e)
        {
            GetDataFromDB();
        }

        protected void gvStudents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            GetDataFromCache();
        }

        protected void gvStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (Cache["DATASET"] != null)
            {
                DataSet ds = (DataSet)Cache["DATASET"];
                DataRow dr = ds.Tables["Students"].Rows.Find(e.Keys["ID"]);
                dr["Name"] = e.NewValues["Name"];
                dr["Gender"] = e.NewValues["Gender"];
                dr["TotalMarks"] = e.NewValues["TotalMarks"];

                Cache.Insert("DATASET", ds, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                gvStudents.EditIndex = -1; //Bring row from the editing mode after updating
                GetDataFromCache();
            }
        }

        protected void gvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            GetDataFromCache();
        }

        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}