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
    class DataAddBooks
    {
        public int ID { set; get; }
        public string BookTitle { set; get; }
        public string Author { set; get; }
        public string Pulished { set; get; }
        public string image { set; get; }
        public string Status { set; get; }

        public List<DataAddBooks> addBooksData()
        {
            List<DataAddBooks> listData = new List<DataAddBooks>();

            try
            {
                using (SqlConnection connect = DatabaseConnection.GetConnection())
                {
                    connect.Open();

                    string selectData = "SELECT * FROM books WHERE date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataAddBooks dab = new DataAddBooks
                                {
                                    ID = (int)reader["id"],
                                    BookTitle = reader["book_title"].ToString(),
                                    Author = reader["author"].ToString(),
                                    Pulished = reader["published_date"].ToString(),
                                    image = reader["image"].ToString(),
                                    Status = reader["status"].ToString()
                                };

                                listData.Add(dab);
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
