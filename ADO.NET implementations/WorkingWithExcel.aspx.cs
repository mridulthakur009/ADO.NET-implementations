using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ADO.NET_implementations
{
    public partial class WorkingWithExcel : System.Web.UI.Page
    {
        //Declare Variable (property)  
        string currFilePath = string.Empty; //File Full Path  
        string currFileExtension = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnRead.Click += new EventHandler(btnRead_Click);
        }
        protected void btnRead_Click(object sender, EventArgs e)
        {
            Upload(); //Upload File Method  
            if (this.currFileExtension == ".xlsx" || this.currFileExtension == ".xls")
            {
                DataTable dt = ReadExcelToTable(currFilePath); //Read Excel File (.XLS and .XLSX Format)  
            }
            else if (this.currFileExtension == ".csv")
            {
                DataTable dt = ReadExcelWithStream(currFilePath); //Read .CSV File  
            }
        }
        ///<summary>  
        ///Upload File to Temporary Category  
        ///</summary>  
        private void Upload()
        {
            HttpPostedFile file = this.fileSelect.PostedFile;
            string fileName = file.FileName;
            string tempPath = System.IO.Path.GetTempPath(); //Get Temporary File Path  
            fileName = System.IO.Path.GetFileName(fileName); //Get File Name (not including path)  
            this.currFileExtension = System.IO.Path.GetExtension(fileName); //Get File Extension  
            this.currFilePath = tempPath + fileName; //Get File Path after Uploading and Record to Former Declared Global Variable  
            file.SaveAs(this.currFilePath); //Upload  
        }

        ///<summary>  
        ///Method to Read XLS/XLSX File  
        ///</summary>  
        ///<param name="path">Excel File Full Path</param>  
        ///<returns></returns>  
        private DataTable ReadExcelToTable(string path)
        {
            //Connection String  
            string conStr = "";
            string Extension = this.currFileExtension;
            switch (Extension)
            {
                case ".xls": //1997-2003
                    conStr = "Provider=Microsoft.JET.OLEDB.4.0;Data Source="
                        + path +
                        ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
                    break;
                case ".xlsx": //2007
                    conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" 
                        + path + 
                        ";Extended Properties=\"Excel 8.0;HDR=YES;\"";
                    break;
            }
            //string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';"; // Extra blank space cannot appear in Office 2007 and the last version. And we need to pay attention on semicolon.  
            //string connstring1 = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';";   

            //string connstring2 = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + path + ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';"; //This connection string is appropriate for Office 2007 and the older version. We can select the most suitable connection string according to Office version or our program.  
            using (OleDbConnection conn = new OleDbConnection(conStr))
            {
                try
                {

                    conn.Open();
                    DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" }); //Get All Sheets Name  
                    string firstSheetName = sheetsName.Rows[0][2].ToString(); //Get the First Sheet Name  
                    string sql = string.Format("SELECT * FROM [{0}],firstSheetName"); //Query String  
                    OleDbDataAdapter ada = new OleDbDataAdapter(sql, conStr);
                    DataSet set = new DataSet();
                    ada.Fill(set);
                    return set.Tables[0];
                }
                catch (OleDbException oledb)
                {
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        ///<summary>  
        ///Method to Read CSV Format  
        ///</summary>  
        ///<param name="path">Read File Full Path</param>  
        ///<returns></returns>  
        private DataTable ReadExcelWithStream(string path)
        {
            DataTable dt = new DataTable();
            bool isDtHasColumn = false; //Mark if DataTable Generates Column  
            StreamReader reader = new StreamReader(path, System.Text.Encoding.Default); //Data Stream  
            while (!reader.EndOfStream)
            {
                string message = reader.ReadLine();
                string[] splitResult = message.Split(new char[] { ',' }, StringSplitOptions.None); //Read One Row and Separate by Comma, Save to Array  
                DataRow row = dt.NewRow();
                for (int i = 0; i < splitResult.Length; i++)
                {
                    if (!isDtHasColumn) //If not Generate Column  
                    {
                        dt.Columns.Add("column" + i, typeof(string));
                    }
                    row[i] = splitResult[i];
                }
                dt.Rows.Add(row); //Add Row  
                isDtHasColumn = true; //Mark the Existed Column after Read the First Row, Not Generate Column after Reading Later Rows  
            }
            return dt;
        }
    }
}