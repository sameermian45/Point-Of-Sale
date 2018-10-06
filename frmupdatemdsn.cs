using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmupdatemdsn : Form
    {
        DataTable dTable;
        SqlConnection con = null;
        SqlDataAdapter adp;
        DataSet ds;
        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public frmupdatemdsn()
        {
            InitializeComponent();
        }

        private void frmupdatemdsn_Load(object sender, EventArgs e)
        {

        }

        

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow dr = dataGridView1.SelectedRows[0];
                this.Hide();
                frmMsdn frmSales = new frmMsdn();
                frmSales.Show();
                frmSales.textBox1.Text = dr.Cells[0].Value.ToString();
                frmSales.textBox2.Text = dr.Cells[1].Value.ToString();
                frmSales.textBox3.Text = dr.Cells[2].Value.ToString();
                frmSales.textBox4.Text = dr.Cells[3].Value.ToString();
                frmSales.textBox5.Text = dr.Cells[4].Value.ToString();
                frmSales.textBox6.Text = dr.Cells[5].Value.ToString();
                
              

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select InvoiceNo from Invoice_Info where InvoiceNo='" + textBox1.Text + "'";

                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    con = new SqlConnection(cs.DBConn);
                    con.Open();

                    cmd = new SqlCommand("SELECT RTRIM(InvoiceNo) as [Invoice No], RTRIM(ProductID) as [ProductID],RTRIM(ProductName) as [Product Name],RTRIM(Price) as [Price],RTRIM(Quantity) as [Quantity],RTRIM(TotalAmount) as [TotalAmount] from ProductSold where  InvoiceNo like '" + textBox1.Text + "%' order by ProductID desc", con);
                    SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                    DataSet myDataSet = new DataSet();
                    myDA.Fill(myDataSet, "Invoice_Info");
                    dataGridView1.DataSource = myDataSet.Tables["Invoice_Info"].DefaultView;
                    con.Close();

                }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
