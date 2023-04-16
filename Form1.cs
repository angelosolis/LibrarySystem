using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class Form1 : Form
    {
        private SqlConnection connection;
        private SqlDataAdapter adapter;
        private DataTable table;
        public Form1()
        {
            InitializeComponent();
            // Create a connection to the database
            connection = new SqlConnection("Data Source=DESKTOP-96R3VE0\\SQLEXPRESS;Initial Catalog=master;Integrated Security=True");

            // Create a new SqlDataAdapter with a SELECT command that retrieves all rows from your table, and a DELETE command that deletes rows from the database
            adapter = new SqlDataAdapter("SELECT * FROM book", connection);
            SqlCommand deleteCommand = new SqlCommand("DELETE FROM book WHERE accession_number=@accession_number", connection);
            deleteCommand.Parameters.Add("@accession_number", SqlDbType.VarChar, 50, "accession_number");
            adapter.DeleteCommand = deleteCommand;

            // Create a new DataTable to hold the data from the database
            table = new DataTable();

            // Fill the DataTable with the data from the database
            adapter.Fill(table);

            // Set the DataGridView control's DataSource property to the DataTable
            grid1.DataSource = table;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'masterDataSet.book' table. You can move, or remove it, as needed.
            this.bookTableAdapter1.Fill(this.masterDataSet.book);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Create a new row in the DataTable and set its values
            DataRow row = table.NewRow();
            row["accession_number"] = txtno.Text;
            row["title"] = txttitle.Text;
            row["author"] = txtauthor.Text;

            // Add the new row to the DataTable
            table.Rows.Add(row);

            // Update the database with the changes
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Update(table);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Get the selected row from the DataGridView
            DataGridViewRow selectedRow = grid1.SelectedRows[0];

            // Get the index of the selected row in the DataTable
            int index = table.Rows.IndexOf((selectedRow.DataBoundItem as DataRowView).Row);

            // Update the values of the selected row in the DataTable
            table.Rows[index]["accession_number"] = txtno.Text;
            table.Rows[index]["title"] = txttitle.Text;
            table.Rows[index]["author"] = txtauthor.Text;

            // Update the database with the changes
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Update(table);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get the selected row from the DataGridView
            DataGridViewRow selectedRow = grid1.SelectedRows[0];

            // Get the index of the selected row in the DataTable
            int index = table.Rows.IndexOf((selectedRow.DataBoundItem as DataRowView).Row);

            // Delete the selected row from the DataTable
            table.Rows[index].Delete();

            // Update the database with the changes
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Update(table);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Get the search term from the txtSearch TextBox
                string searchTerm = txtSearch.Text;

                // Create a DataView object to filter the table
                DataView dataView = new DataView(table);
                dataView.RowFilter = string.Format("CONVERT(accession_number, 'System.String') LIKE '%{0}%' OR title LIKE '%{0}%' OR author LIKE '%{0}%'", searchTerm);

                // Set the DataGridView control's DataSource property to the filtered DataView
                grid1.DataSource = dataView;
            }
        }
    }
}
