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
    public partial class frmBillUpdate : Form
    {
        DataTable dTable;
        SqlConnection con = null;
        SqlDataAdapter adp;
        DataSet ds;
        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public frmBillUpdate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter Receipt ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }
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
                    
                    cmd = new SqlCommand("SELECT RTRIM(invoiceNo) as [Receipt No],RTRIM(InvoiceDate) as [Order Date],RTRIM(CustomerName) as [Customer Name],RTRIM(SubTotal) as [SubTotal],RTRIM(DiscountPer) as [Discount %],RTRIM(DiscountAmount) as [Discount Amount],RTRIM(GrandTotal) as [Total Amount],RTRIM(TotalPayment) as [Received Cash],RTRIM(PaymentDue) as [Payment Due],Remarks from Invoice_Info where  InvoiceNo like '" + textBox1.Text + "%' order by InvoiceDate desc", con);
                    // cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "InvoiceDate").Value = dtpInvoiceDateFrom.Value.Date;
                    // cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "InvoiceDate").Value = dtpInvoiceDateTo.Value.Date;
                    SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                    DataSet myDataSet = new DataSet();
                    myDA.Fill(myDataSet, "Invoice_Info");
                    DataGridView1.DataSource = myDataSet.Tables["Invoice_Info"].DefaultView;
                    Decimal sum = 0;
                    Decimal sum1 = 0;
                    Decimal sum2 = 0;

                    foreach (DataGridViewRow r in this.DataGridView1.Rows)
                    {
                        Decimal i = Convert.ToDecimal(r.Cells[6].Value);
                        Decimal j = Convert.ToDecimal(r.Cells[7].Value);
                        Decimal k = Convert.ToDecimal(r.Cells[8].Value);
                        sum = sum + i;
                        sum1 = sum1 + j;
                        sum2 = sum2 + k;

                    }
                    DataGridViewRow dr = DataGridView1.Rows[0];
                    textBox3.Text = sum.ToString();
                    textBox4.Text = sum1.ToString();
                    textBox5.Text = sum2.ToString();
                    txtRemarks.Text = dr.Cells[9].Value.ToString();


                    con.Close();
                    dataGridView2.Visible = true;
                    button3.Enabled = true;
                    GetData();
                }

                    else //((rdr != null))
                    {
                        MessageBox.Show("Recipt Id Invalid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Text = "";
                        textBox1.Focus();
                    }
                    return;
                }

                //GroupBox3.Visible = true;
                
          
            
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Reset()
        {
            textBox1.Text = "";
            textBox3.Text = "";
            txtRemarks.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox2.Text = "";
            textBox6.Text = "";
            dataGridView2.Visible=false;
            button3.Enabled = false;
            dtpInvoiceDate.Text = DateTime.Today.ToString();
            DataGridView1.DataSource = null;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please enter Amount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Focus();
                return;
            }
            try
            {

                con = new SqlConnection(cs.DBConn);
                con.Open();

                string cb = "insert into Receable(InvoiceDate,InvoiceNo,TotalPayment,PaymentDue) VALUES (@d1,@d2,@d3,@d4)";

                cmd = new SqlCommand(cb);

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1", dtpInvoiceDate.Text);
                cmd.Parameters.AddWithValue("@d2", textBox1.Text);
                cmd.Parameters.AddWithValue("@d3", textBox2.Text);
                cmd.Parameters.AddWithValue("@d4", textBox5.Text);
                cmd.ExecuteReader();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con.Close();
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                String cb = "update Invoice_info set GrandTotal=@d1,TotalPayment=@d2,PaymentDue=@d3,Remarks=@d4  where Invoiceno= '" + textBox1.Text + "'";
                cmd = new SqlCommand(cb);

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1", textBox3.Text);
                cmd.Parameters.AddWithValue("@d2", textBox6.Text);
                cmd.Parameters.AddWithValue("@d3", textBox5.Text);
                cmd.Parameters.AddWithValue("@d4", txtRemarks.Text);
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            Decimal val1 = 0;
            Decimal val2 = 0;
            Decimal.TryParse(textBox4.Text, out val1);
            Decimal.TryParse(textBox2.Text, out val2);
            Decimal I = (val1 + val2);
            textBox6.Text = I.ToString();
        }

        private void frmBillUpdate_Load(object sender, EventArgs e)
        {
            button3.Enabled = false;
            dataGridView2.Visible=false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            Decimal val1 = 0;
            Decimal val2 = 0;
            Decimal.TryParse(textBox3.Text, out val1);
            Decimal.TryParse(textBox6.Text, out val2);
            Decimal I = (val1 - val2);
            textBox5.Text = I.ToString();
        }

        public void GetData()
        {
            try
            {
              
                con = new SqlConnection(cs.DBConn);
                con.Open();
                String sql = "SELECT * from Receable where InvoiceNo='" + textBox1.Text + "' order by InvoiceDate";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView2.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView2.Rows.Add(rdr[1], rdr[2], rdr[3], rdr[4]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmBillUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmMainMenu frm = new frmMainMenu();
            frm.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                rptInvoice rpt = new rptInvoice();
                //The report you created.
                cmd = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                POS_DBDataSet myDS = new POS_DBDataSet();
                //The DataSet you created.
                con = new SqlConnection(cs.DBConn);
                cmd.Connection = con;
                cmd.CommandText = "SELECT * from product,invoice_info,productsold where invoice_info.invoiceno=productsold.invoiceno and ProductSold.ProductID=Product.ProductID and Invoice_info.invoiceNo='" + textBox1.Text + "'";
                cmd.CommandType = CommandType.Text;
                myDA.SelectCommand = cmd;
                myDA.Fill(myDS, "product");
                myDA.Fill(myDS, "Invoice_Info");
                myDA.Fill(myDS, "ProductSold");
                rpt.SetDataSource(myDS);
                frmInvoiceReport frm = new frmInvoiceReport();
                frm.crystalReportViewer1.ReportSource = rpt;
                frm.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow dr = DataGridView1.SelectedRows[0];
                
                frmProductSold frmSales = new frmProductSold();
                frmSales.Show();
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT Product.ProductID,ProductSold.Productname,ProductSold.Price,ProductSold.Quantity,ProductSold.TotalAmount from Invoice_Info,ProductSold,Product where Invoice_info.InvoiceNo=ProductSold.InvoiceNo and Product.ProductID=ProductSold.ProductID and invoice_Info.InvoiceNo='" + dr.Cells[0].Value.ToString() + "'", con);
                rdr = cmd.ExecuteReader();
                while (rdr.Read() == true)
                {
                    ListViewItem lst = new ListViewItem();
                    lst.SubItems.Add(rdr[0].ToString().Trim());
                    lst.SubItems.Add(rdr[1].ToString().Trim());
                    lst.SubItems.Add(rdr[2].ToString().Trim());
                    lst.SubItems.Add(rdr[3].ToString().Trim());
                    lst.SubItems.Add(rdr[4].ToString().Trim());
                    frmSales.ListView1.Items.Add(lst);
                }
                
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
    }
}
    

