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
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void signIn_btn_Click(object sender, EventArgs e)
        {
            LoginForm lForm = new LoginForm();
            lForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void register_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(register_email.Text) ||
                string.IsNullOrWhiteSpace(register_username.Text) ||
                string.IsNullOrWhiteSpace(register_password.Text))
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SqlConnection connect = DatabaseConnection.GetConnection())
            {
                try
                {
                    connect.Open();

                    // Check if the username is already taken
                    string checkUsernameQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (SqlCommand checkCMD = new SqlCommand(checkUsernameQuery, connect))
                    {
                        checkCMD.Parameters.AddWithValue("@username", register_username.Text.Trim());
                        int count = (int)checkCMD.ExecuteScalar();

                        if (count >= 1)
                        {
                            MessageBox.Show($"{register_username.Text.Trim()} is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Insert the new user into the database
                    string insertDataQuery = "INSERT INTO users (email, username, password, date_register) " +
                                             "VALUES(@register_email, @register_username, @register_password, @register_date)";
                    using (SqlCommand insertCMD = new SqlCommand(insertDataQuery, connect))
                    {
                        insertCMD.Parameters.AddWithValue("@register_email", register_email.Text.Trim());
                        insertCMD.Parameters.AddWithValue("@register_username", register_username.Text.Trim());
                        insertCMD.Parameters.AddWithValue("@register_password", register_password.Text.Trim());
                        insertCMD.Parameters.AddWithValue("@register_date", DateTime.Now);

                        insertCMD.ExecuteNonQuery();

                        MessageBox.Show("Registered successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Redirect to the login form
                        LoginForm lForm = new LoginForm();
                        lForm.Show();
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to the database: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void register_showPass_CheckedChanged(object sender, EventArgs e)
        {
            register_password.PasswordChar = register_showPass.Checked ? '\0' : '*';
        }
    }
}
