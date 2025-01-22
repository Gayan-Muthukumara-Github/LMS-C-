using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LibraryManagementSystem
{
    class DataIssueBooks
    {

        public int ID { set; get; }
        public string IssueID { set; get; }
        public string Name { set; get; }
        public string Contact { set; get; }
        public string Email { set; get; }
        public string BookTitle { set; get; }
        public string Author { set; get; }
        public string DateIssue { set; get; }
        public string DateReturn { set; get; }
        public string Status { set; get; }

        public List<DataIssueBooks> ReturnIssueBooksData()
        {
            List<DataIssueBooks> listData = new List<DataIssueBooks>();

            try
            {
                using (SqlConnection connect = DatabaseConnection.GetConnection())
                {
                    connect.Open();

                    string selectData = "SELECT * FROM issues WHERE status = 'Not Return' AND date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataIssueBooks dib = new DataIssueBooks
                                {
                                    ID = (int)reader["id"],
                                    IssueID = reader["issue_id"].ToString(),
                                    Name = reader["full_name"].ToString(),
                                    Contact = reader["contact"].ToString(),
                                    Email = reader["email"].ToString(),
                                    BookTitle = reader["book_title"].ToString(),
                                    Author = reader["author"].ToString(),
                                    DateIssue = reader["issue_date"].ToString(),
                                    DateReturn = reader["return_date"].ToString(),
                                    Status = reader["status"].ToString()
                                };

                                listData.Add(dib);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error: " + sqlEx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return listData;
        }
        public List<DataIssueBooks> IssueBooksData()
        {
            List<DataIssueBooks> listData = new List<DataIssueBooks>();

            try
            {
                using (SqlConnection connect = DatabaseConnection.GetConnection())
                {
                    connect.Open();

                    string selectData = "SELECT * FROM issues WHERE date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataIssueBooks dib = new DataIssueBooks
                                {
                                    ID = (int)reader["id"],
                                    IssueID = reader["issue_id"].ToString(),
                                    Name = reader["full_name"].ToString(),
                                    Contact = reader["contact"].ToString(),
                                    Email = reader["email"].ToString(),
                                    BookTitle = reader["book_title"].ToString(),
                                    Author = reader["author"].ToString(),
                                    DateIssue = reader["issue_date"].ToString(),
                                    DateReturn = reader["return_date"].ToString(),
                                    Status = reader["status"].ToString()
                                };

                                listData.Add(dib);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Database error: " + sqlEx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return listData;
        }

    }
}
