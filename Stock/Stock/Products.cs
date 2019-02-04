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

namespace Stock
{
    public partial class Products : Form
    {
        public Products()
        {
            InitializeComponent();
        }
        public void Products_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-AV3FS03\\REDASQL;Initial Catalog=Stock;Integrated Security=True");
            conn.Open();
            bool Status = false;
            if (comboBox1.SelectedIndex == 0)
            {
                Status = true;
            }
            else
            {
                Status = false;
            }
            var sqlQuery = "";
            if (IfProductsExist(conn, textBox1.Text))
            {
                sqlQuery = @"UPDATE [Products] SET [ProductName] = '" + textBox2.Text + "' ,[ProductStatus] = '" + Status + "' WHERE [ProductCode] = '" + textBox1.Text + "'";
            }
            else
            {
                sqlQuery = @"INSERT INTO Products ([ProductCode],[ProductName],[ProductStatus]) VALUES
                ('" + textBox1.Text + "', '" + textBox2.Text + "','" + Status + "')";
            }
            SqlCommand cmd=new SqlCommand(sqlQuery, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
            LoadData();
            //Reading data
        }
        private bool IfProductsExist(SqlConnection conn, string productCode)
        {
            SqlDataAdapter sda = new SqlDataAdapter("SELECT 1 FROM Products where ProductCode='"+ productCode +"'", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public void LoadData()
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-AV3FS03\\REDASQL;Initial Catalog=Stock;Integrated Security=True");
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM Products", conn);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            dataGridView1.Rows.Clear();
            foreach (DataRow item in dt.Rows)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = item["ProductCode"].ToString();
                dataGridView1.Rows[n].Cells[1].Value = item["ProductName"].ToString();
                if ((bool)item["ProductStatus"])
                {
                    dataGridView1.Rows[n].Cells[2].Value = "active";
                }
                else
                {
                    dataGridView1.Rows[n].Cells[2].Value = "deactive";
                }
            }
        }
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "active")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }

        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "active")
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection("Data Source=DESKTOP-AV3FS03\\REDASQL;Initial Catalog=Stock;Integrated Security=True");
            var sqlQuery = "";
            if (IfProductsExist(conn, textBox1.Text))
            {
                conn.Open();
                sqlQuery = @"DELETE FROM [Products] WHERE ProductCode='" + textBox1.Text + "'";
                SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
            }
            else
            {
                MessageBox.Show("le produit is not exist...");
            }
        }
    } 
}
