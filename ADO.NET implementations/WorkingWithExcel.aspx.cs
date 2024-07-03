using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;

namespace ADO.NET_implementations
{
    public partial class WorkingWithExcel : System.Web.UI.Page
    {
        // Declare variables
        string currFilePath = string.Empty;
        string currFileExtension = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnRead.Click += new EventHandler(btnRead_Click);
        }

        protected void btnRead_Click(object sender, EventArgs e)
        {
            Upload(); // Upload File Method  

            if (this.currFileExtension == ".xlsx" || this.currFileExtension == ".xls")
            {
                ReadExcelToTable(currFilePath); // Read Excel File (.XLS and .XLSX Format)  
            }
            else if (this.currFileExtension == ".csv")
            {
                DataTable dt = ReadExcelWithStream(currFilePath); // Read .CSV File  
                // Use 'dt' DataTable as needed (e.g., bind to GridView)
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
        }

        /// <summary>  
        /// Upload File to Temporary Category  
        /// </summary>  
        private void Upload()
        {
            HttpPostedFile file = this.fileSelect.PostedFile;
            string fileName = Path.GetFileName(file.FileName);
            string tempPath = Path.GetTempPath(); // Get Temporary File Path  
            this.currFileExtension = Path.GetExtension(fileName); // Get File Extension  
            this.currFilePath = Path.Combine(tempPath, fileName); // Get File Path after Uploading and Record to Former Declared Global Variable  
            file.SaveAs(this.currFilePath); // Upload  
        }

        /// <summary>  
        /// Method to Read XLS/XLSX File  
        /// </summary>  
        /// <param name="path">Excel File Full Path</param>  
        private void ReadExcelToTable(string path)
        {
            // Connection String  
            string conStr = "";
            switch (this.currFileExtension)
            {
                case ".xls": // 1997-2003
                    conStr = $"Provider=Microsoft.JET.OLEDB.4.0;Data Source={path};Extended Properties='Excel 8.0;HDR=YES;'";
                    break;
                case ".xlsx": // 2007
                    conStr = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                    break;
                default:
                    throw new NotSupportedException("File extension not supported.");
            }

            using (OleDbConnection conn = new OleDbConnection(conStr))
            {
                try
                {
                    conn.Open();
                    DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    string firstSheetName = sheetsName.Rows[0]["TABLE_NAME"].ToString();

                    // Fix SQL query
                    string sql = $"SELECT * FROM [{firstSheetName}]";

                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            ArrayList al = new ArrayList();
                            while (reader.Read())
                            {
                                string str = "";
                                
                                int columnCount = reader.FieldCount;   
                                

                                for (int i = 0; i < columnCount; i++)
                                {
                                    string str1 = reader.GetName(i);
                                    string columnValue = reader[i].ToString();
                                    str += str1+ ": " + columnValue + " -;- ";
                                }

                            }
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    // Handle exceptions
                    throw new Exception("Error reading Excel file", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>  
        /// Method to Read CSV Format  
        /// </summary>  
        /// <param name="path">Read File Full Path</param>  
        /// <returns></returns>  
        private DataTable ReadExcelWithStream(string path)
        {
            DataTable dt = new DataTable();
            bool isDtHasColumn = false;

            using (StreamReader reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    string message = reader.ReadLine();
                    string[] splitResult = message.Split(new char[] { ',' }, StringSplitOptions.None);

                    if (!isDtHasColumn)
                    {
                        // Generate columns if not already done
                        for (int i = 0; i < splitResult.Length; i++)
                        {
                            dt.Columns.Add("column" + i, typeof(string));
                        }
                        isDtHasColumn = true;
                    }

                    DataRow row = dt.NewRow();
                    for (int i = 0; i < splitResult.Length; i++)
                    {
                        row[i] = splitResult[i];
                    }
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }
    }
}
