using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace LibraryManagementSystem
{
    public partial class Dashboard : UserControl
    {

        public Dashboard()
        {
            InitializeComponent();

            displayAB();
            displayIB();
            displayRB();
        }

        public void refreshData()
        {

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }

            displayAB();
            displayIB();
            displayRB();
        }
        public void displayAB()
        {
            try
            {
                using (SqlConnection connect = DatabaseConnection.GetConnection())
                {
                    connect.Open();
                    string selectData = "SELECT COUNT(id) AS AvailableBooksCount " +
                                        "FROM books WHERE status = 'Available' AND date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        object result = cmd.ExecuteScalar();
                        int availableBooksCount = result != null ? Convert.ToInt32(result) : 0;

                        dashboard_AB.Text = availableBooksCount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while fetching available books count: {ex.Message}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void displayIB()
        {
            try
            {
                using (SqlConnection connect = DatabaseConnection.GetConnection())
                {
                    connect.Open();
                    string selectData = "SELECT COUNT(id) AS IssuedBooksCount FROM issues WHERE date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        object result = cmd.ExecuteScalar();
                        int issuedBooksCount = result != null ? Convert.ToInt32(result) : 0;

                        dashboard_IB.Text = issuedBooksCount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while fetching issued books count: {ex.Message}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void displayRB()
        {
            try
            {
                using (SqlConnection connect = DatabaseConnection.GetConnection())
                {
                    connect.Open();
                    string selectData = "SELECT COUNT(id) AS ReturnedBooksCount FROM issues WHERE status = 'Return' AND date_delete IS NULL";

                    using (SqlCommand cmd = new SqlCommand(selectData, connect))
                    {
                        object result = cmd.ExecuteScalar();
                        int returnedBooksCount = result != null ? Convert.ToInt32(result) : 0;

                        dashboard_RB.Text = returnedBooksCount.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while fetching returned books count: {ex.Message}", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
