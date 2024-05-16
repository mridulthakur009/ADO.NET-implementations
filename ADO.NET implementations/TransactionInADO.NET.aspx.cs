using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADO.NET_implementations
{
    public partial class TransactionInADO_NET : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAccountsData();
            }
        }
        private void GetAccountsData()
        {
            string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand("Select * from Accounts", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    if (rdr["AccountNumber"].ToString() == "A1")
                    {
                        lblAccountNumber1.Text = "A1";
                        lblName1.Text = rdr["CustomerName"].ToString();
                        lblBalance1.Text = rdr["Balance"].ToString();
                    }
                    else
                    {
                        lblAccountNumber2.Text = "A2";
                        lblName2.Text = rdr["CustomerName"].ToString();
                        lblBalance2.Text = rdr["Balance"].ToString();
                    }
                }
            }
        }

        protected void btnTransfer_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["CS"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand("Update Accounts set Balance = Balance - 10 where AccountNumber = 'A1'", con, transaction);
                    cmd.ExecuteNonQuery();
                    
                    cmd = new SqlCommand("Update Accounts set Balance = Balance + 10 where AccountNumber = 'A2'", con, transaction);
                    cmd.ExecuteNonQuery();
                    
                    transaction.Commit();
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "Transaction committed";
                }
                catch
                {
                    transaction.Rollback();
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "Transaction rolled back";
                }
            }
            GetAccountsData();
        }
    }
}