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
    public partial class StronglyTypedDS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string CS = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                SqlConnection connection = new SqlConnection(CS);

                string selectQuery = "Select * from tblStudents";

                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);

                DataSet dataSet = new DataSet();

                dataAdapter.Fill(dataSet, "Students");
                Session["DATASET"] = dataSet;

                //LINQ code:
                GridView1.DataSource = from dataRow in dataSet.Tables["Students"].AsEnumerable()
                                       select new Student
                                       {
                                           ID = Convert.ToInt32(dataRow["ID"]),
                                           Name = Convert.ToString(dataRow["Name"]),
                                           Gender = Convert.ToString(dataRow["Gender"]),
                                           TotalMarks = Convert.ToInt32(dataRow["TotalMarks"])

                                       };
                GridView1.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Not Strongly Types DataSet
            DataSet dataSet = (DataSet)Session["DATASET"];

            if(string.IsNullOrEmpty(TextBox1.Text))
            {
                GridView1.DataSource = from dataRow in dataSet.Tables["Students"].AsEnumerable()
                                       select new Student
                                       {
                                           ID = Convert.ToInt32(dataRow["ID"]),
                                           Name = Convert.ToString(dataRow["Name"]),
                                           Gender = Convert.ToString(dataRow["Gender"]),
                                           TotalMarks = Convert.ToInt32(dataRow["TotalMarks"])

                                       };
                GridView1.DataBind();  
            }
            else
            {
                GridView1.DataSource = from dataRow in dataSet.Tables["Students"].AsEnumerable()
                                       where dataRow["Name"].ToString().ToUpper().StartsWith(TextBox1.Text.ToUpper())
                                       select new Student
                                       {
                                           ID = Convert.ToInt32(dataRow["ID"]),
                                           Name = Convert.ToString(dataRow["Name"]),
                                           Gender = Convert.ToString(dataRow["Gender"]),
                                           TotalMarks = Convert.ToInt32(dataRow["TotalMarks"])

                                       };
                GridView1.DataBind();
            }
        }
    }
}