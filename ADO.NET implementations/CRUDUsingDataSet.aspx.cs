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
        string CS = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void GetDataFromDB()
        {
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
                gvStudents.DataSource = (DataSet)Cache["DATASET"]; 
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
                gvStudents.EditIndex = -1; //Bring row out of editing mode
                GetDataFromCache();
            }
        }

        protected void gvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1; //Bring row out of editing mode
            GetDataFromCache();
        }

        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (Cache["DATASET"] != null)
            {
                DataSet ds = (DataSet)Cache["DATASET"];
                DataRow dr = ds.Tables["Students"].Rows.Find(e.Keys["ID"]);
                dr.Delete();

                Cache.Insert("DATASET", ds, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                
                GetDataFromCache();
            }
        }

        protected void UpdateDataInDb_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(CS);

            string strSelectQuery = "Select * from tblStudents";

            SqlDataAdapter da = new SqlDataAdapter(strSelectQuery, con);

            DataSet ds = (DataSet)Cache["DATASET"];

            string strUpdateCommand = "Update tblStudents set Name= @Name, Gender = @Gender, TotalMarks = @TotalMarks where Id = @Id";
            SqlCommand updateCommand = new SqlCommand(strUpdateCommand, con);
            updateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50, "Name");
            updateCommand.Parameters.Add("@Gender", SqlDbType.NVarChar, 20, "Gender");
            updateCommand.Parameters.Add("@TotalMarks", SqlDbType.Int, 0, "TotalMarks");
            updateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");

            da.UpdateCommand = updateCommand;

            string strDeleteCommand = "Delete from tblStudents where Id = @Id";
            SqlCommand deleteCommand = new SqlCommand(strDeleteCommand, con);
            deleteCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");

            da.DeleteCommand = deleteCommand;

            da.Update(ds, "Students");

            Label1.Text = "Database Table Updated";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET"];
            if (ds.HasChanges())
            {
                ds.RejectChanges();
                Cache.Insert("DATASET", ds, null, DateTime.Now.AddDays(1), System.Web.Caching.Cache.NoSlidingExpiration);
                GetDataFromCache();
                Label1.Text = "Changes Done";
                Label1.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                Label1.Text = "No changes to Undo";
                Label1.ForeColor = System.Drawing.Color.Red;
            }
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Cache["DATASET"];

            foreach(DataRow dr in ds.Tables["Students"].Rows)
            {
                if(dr.RowState == DataRowState.Deleted)
                {
                    Response.Write(dr["id", DataRowVersion.Original].ToString() + " - " + dr.RowState.ToString()+ "</br>");
                }
                else
                {
                Response.Write(dr["id"].ToString() + " - " + dr.RowState.ToString() + "</br>");
                }
            }
        }
    }
}