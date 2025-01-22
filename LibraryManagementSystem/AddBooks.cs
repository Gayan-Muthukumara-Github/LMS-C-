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
using System.IO;

namespace LibraryManagementSystem
{
    public partial class AddBooks : UserControl
    {

        public AddBooks()
        {
            InitializeComponent();

            displayBooks();

        }

        public void refreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)refreshData);
                return;
            }

            displayBooks();
        }

        private String imagePath;
        private void addBooks_importBtn_Click(object sender, EventArgs e)
        {
            
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png";

                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dialog.FileName;
                    addBooks_picture.ImageLocation = imagePath;
                }
            }catch(Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addBooks_addBtn_Click(object sender, EventArgs e)
        {
            if (addBooks_picture.Image == null
                || addBooks_bookTitle.Text == ""
                || addBooks_author.Text == ""
                || addBooks_published.Value == null
                || addBooks_status.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    // Ensure connection is open using DatabaseConnection
                    using (SqlConnection connect = DatabaseConnection.GetConnection())
                    {
                        connect.Open();

                        // Prepare the file path for the image
                        string imageFileName = addBooks_bookTitle.Text.Trim() + "_" + addBooks_author.Text.Trim() + ".jpg";
                        string path = Path.Combine(@"D:\OneDrive - hSenid Business Solutions PLC\Desktop\Library-Management-System-using-CSharp-main\Library-Management-System-using-CSharp-main\LibraryManagementSystem\LibraryManagementSystem\Books_Directory\" +
                            addBooks_bookTitle.Text + addBooks_author.Text.Trim() + ".jpg");

                        string directoryPath = Path.GetDirectoryName(path);

                        // Ensure the directory exists
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        // Path to save the image
                        string filePath = Path.Combine(directoryPath, imageFileName);

                        // Copy the image to the directory
                        File.Copy(addBooks_picture.ImageLocation, filePath, true);

                        // SQL command to insert the book details
                        string insertData = "INSERT INTO books " +
                                            "(book_title, author, published_date, status, image, date_insert) " +
                                            "VALUES(@bookTitle, @author, @published_date, @status, @image, @dateInsert)";

                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@bookTitle", addBooks_bookTitle.Text.Trim());
                            cmd.Parameters.AddWithValue("@author", addBooks_author.Text.Trim());
                            cmd.Parameters.AddWithValue("@published_date", addBooks_published.Value);
                            cmd.Parameters.AddWithValue("@status", addBooks_status.Text.Trim());
                            cmd.Parameters.AddWithValue("@image", filePath);
                            cmd.Parameters.AddWithValue("@dateInsert", DateTime.Today);

                            cmd.ExecuteNonQuery();

                            // Refresh the book list display
                            displayBooks();

                            // Inform user of success
                            MessageBox.Show("Book added successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Clear the input fields
                            clearFields();
                        }
                    }
                }
                catch (DirectoryNotFoundException dirEx)
                {
                    MessageBox.Show("Directory error: " + dirEx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show("File error: " + ioEx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (SqlException sqlEx)
                {
                    MessageBox.Show("Database error: " + sqlEx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void clearFields()
        {
            addBooks_bookTitle.Text = "";
            addBooks_author.Text = "";
            addBooks_picture.Image = null;
            addBooks_status.SelectedIndex = -1;
        }

        public void displayBooks()
        {
            DataAddBooks dab = new DataAddBooks();
            List<DataAddBooks> listData = dab.addBooksData();

            dataGridView1.DataSource = listData;

        }

        private int bookID = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                bookID = (int)row.Cells[0].Value;
                addBooks_bookTitle.Text = row.Cells[1].Value.ToString();
                addBooks_author.Text = row.Cells[2].Value.ToString();
                addBooks_published.Text = row.Cells[3].Value.ToString();

                string imagePath = row.Cells[4].Value.ToString();


                if (imagePath != null || imagePath.Length >= 1)
                {
                    addBooks_picture.Image = Image.FromFile(imagePath);
                }
                else
                {
                    addBooks_picture.Image = null;
                }
                addBooks_status.Text = row.Cells[5].Value.ToString();
            }
        }

        private void addBooks_clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }
        private void addBooks_updateBtn_Click(object sender, EventArgs e)
        {
            // Check if required fields are empty
            if (string.IsNullOrEmpty(addBooks_bookTitle.Text) || string.IsNullOrEmpty(addBooks_author.Text) || addBooks_published.Value == null || string.IsNullOrEmpty(addBooks_status.Text))
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Confirm before updating
                DialogResult check = MessageBox.Show("Are you sure you want to update Book ID: " + bookID + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connect = DatabaseConnection.GetConnection())
                        {
                            connect.Open();

                            // Handle image update if a new image is selected
                            string imageFilePath = null;
                            if (addBooks_picture.Image != null)
                            {
                                // Save the new image if selected
                                string imageFileName = addBooks_bookTitle.Text.Trim() + "_" + addBooks_author.Text.Trim() + ".jpg";
                                string directoryPath = Path.Combine(Application.StartupPath, "Books_Directory");

                                if (!Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                imageFilePath = Path.Combine(directoryPath, imageFileName);
                                File.Copy(addBooks_picture.ImageLocation, imageFilePath, true); // Overwrite if exists
                            }

                            // SQL update query
                            string updateData = "UPDATE books SET book_title = @bookTitle, author = @author, published_date = @publishedDate, status = @status, date_update = @dateUpdate" +
                                                (imageFilePath != null ? ", image = @image" : "") +
                                                " WHERE id = @id";

                            using (SqlCommand cmd = new SqlCommand(updateData, connect))
                            {
                                // Add parameters
                                cmd.Parameters.AddWithValue("@bookTitle", addBooks_bookTitle.Text.Trim());
                                cmd.Parameters.AddWithValue("@author", addBooks_author.Text.Trim());
                                cmd.Parameters.AddWithValue("@publishedDate", addBooks_published.Value);
                                cmd.Parameters.AddWithValue("@status", addBooks_status.Text.Trim());
                                cmd.Parameters.AddWithValue("@dateUpdate", DateTime.Today);
                                cmd.Parameters.AddWithValue("@id", bookID);

                                // Add image parameter if it was updated
                                if (imageFilePath != null)
                                {
                                    cmd.Parameters.AddWithValue("@image", imageFilePath);
                                }

                                // Execute the update
                                cmd.ExecuteNonQuery();

                                // Refresh the book list
                                displayBooks();

                                // Notify user
                                MessageBox.Show("Book updated successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Clear input fields
                                clearFields();
                            }
                        }
                    }
                    catch (SqlException sqlEx)
                    {
                        MessageBox.Show("Database error: " + sqlEx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (IOException ioEx)
                    {
                        MessageBox.Show("File error: " + ioEx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An unexpected error occurred: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Update cancelled.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void addBooks_deleteBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(addBooks_bookTitle.Text) || string.IsNullOrEmpty(addBooks_author.Text) || addBooks_published.Value == null || string.IsNullOrEmpty(addBooks_status.Text))
            {
                MessageBox.Show("Please select an item first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Are you sure you want to DELETE Book ID: " + bookID + "?", "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connect = DatabaseConnection.GetConnection())
                        {
                            connect.Open();

                            // Update query for soft delete
                            string updateData = "UPDATE books SET date_delete = @dateDelete WHERE id = @id";

                            using (SqlCommand cmd = new SqlCommand(updateData, connect))
                            {
                                cmd.Parameters.AddWithValue("@dateDelete", DateTime.Today);
                                cmd.Parameters.AddWithValue("@id", bookID);

                                cmd.ExecuteNonQuery();

                                // Refresh the list after delete
                                displayBooks();

                                // Notify user
                                MessageBox.Show("Book deleted successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Clear fields after deletion
                                clearFields();
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
                }
                else
                {
                    MessageBox.Show("Delete operation cancelled.", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

    }
}
