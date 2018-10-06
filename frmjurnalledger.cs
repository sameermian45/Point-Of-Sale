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
    public partial class frmjurnalledger : Form
    {
        SqlDataReader rdr;
        DataTable dTable;
        SqlConnection con = null;
        SqlDataAdapter adp;
        DataSet ds;
        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        ConnectionString cs = new ConnectionString();
        public frmjurnalledger()
        {
            InitializeComponent();
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
          
        }

        
        

        private void TabPage1_Click(object sender, EventArgs e)
        {

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

       
        
        private void frmjurnalledger_Load(object sender, EventArgs e)
        {
            
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            
            try
            {

                
                textBox3.Visible = true;
                textBox2.Visible = true;
                txtCustomerID.Visible = true;
                groupBox7.Visible = true;
                con = new SqlConnection(cs.DBConn);
                con.Open();

                cmd = new SqlCommand("SELECT RTRIM(ExpenseNo) as [Expense ID],RTRIM(ExpenseDate) as [Expense Date],RTRIM(ExpencseDetail) as [Expense Details],RTRIM(Amount) as [Expense Amount],RTRIM(ReturnAmount) as [Return Amount],RTRIM(RADetails) as [Retuen Amount Details] from Expense where ExpenseDate between @d1 and @d2 order by ExpenseDate desc", con);
                cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "ExpenseDate").Value = dtpInvoiceDateFrom.Value.Date;
                cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "ExpenseDate").Value = dtpInvoiceDateTo.Value.Date;
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Expense");
                DataGridView1.DataSource = myDataSet.Tables["Expense"].DefaultView;
                Decimal sum = 0;
                Decimal sum1 = 0;
                foreach (DataGridViewRow r in this.DataGridView1.Rows)
                {
                    Decimal i = Convert.ToDecimal(r.Cells[3].Value);
                    Decimal j = Convert.ToDecimal(r.Cells[4].Value);
                    sum = sum + i;
                    sum1 = sum1 + j;
                }
                textBox6.Text = sum.ToString();
                textBox7.Text = sum1.ToString();
                
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {


               // GroupBox3.Visible = true;
                con = new SqlConnection(cs.DBConn);
                con.Open();

                cmd = new SqlCommand("SELECT RTRIM(ExpenseNo) as [Expense ID],RTRIM(ExpenseDate) as [Expense Date],RTRIM(ExpencseDetail) as [Expense Details],RTRIM(Amount) as [Expense Amount] from personalExpense where ExpenseDate between @d1 and @d2 order by ExpenseDate desc", con);
                cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "ExpenseDate").Value = dtpInvoiceDateFrom.Value.Date;
                cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "ExpenseDate").Value = dtpInvoiceDateTo.Value.Date;
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "personalExpense");
                dataGridView4.DataSource = myDataSet.Tables["personalExpense"].DefaultView;
                Decimal sum = 0;
                foreach (DataGridViewRow r in this.dataGridView4.Rows)
                {
                    Decimal i = Convert.ToDecimal(r.Cells[3].Value);
                    sum = sum + i;
                }
                textBox1.Text = sum.ToString();
                textBox8.Text = Convert.ToDecimal((Convert.ToDecimal(textBox6.Text) + Convert.ToDecimal(textBox1.Text))).ToString();
                textBox10.Text = Convert.ToDecimal(textBox7.Text).ToString();
                textBox5.Text = Convert.ToDecimal((Convert.ToDecimal(textBox8.Text) - Convert.ToDecimal(textBox10.Text))).ToString();
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
                cmd = new SqlCommand("SELECT RTRIM(invoiceNo) as [Order No],RTRIM(InvoiceDate) as [Order Date],RTRIM(SubTotal) as [SubTotal],RTRIM(DiscountPer) as [Discount %],RTRIM(DiscountAmount) as [Discount Amount],RTRIM(GrandTotal) as [Grand Total],RTRIM(TotalPayment) as [Total Payment],RTRIM(PaymentDue) as [Payment Due],Remarks from Invoice_Info where InvoiceDate between @d1 and @d2 order by InvoiceDate desc", con);
                cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "InvoiceDate").Value = dtpInvoiceDateFrom.Value.Date;
                cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "InvoiceDate").Value = dtpInvoiceDateTo.Value.Date;
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Invoice_Info");
                dataGridView2.DataSource = myDataSet.Tables["Invoice_Info"].DefaultView;
                Decimal sum = 0;
                foreach (DataGridViewRow r in this.dataGridView2.Rows)
                {
                    Decimal i = Convert.ToDecimal(r.Cells[5].Value);
                    sum = sum + i;
                }
                textBox2.Text = sum.ToString();
                con.Close();



                // textBox5.Text = Convert.ToInt32((Convert.ToInt32(textBox6.Text) - Convert.ToDouble(textBox7.Text))).ToString();
                txtCustomerID.Text =  Convert.ToDecimal(textBox5.Text).ToString();
                textBox3.Text = Convert.ToDecimal((Convert.ToDecimal(textBox2.Text) - Convert.ToDecimal(txtCustomerID.Text))).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void Button2_Click_1(object sender, EventArgs e)
        {
            DataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView4.DataSource = null;
            dtpInvoiceDateFrom.Text = DateTime.Today.ToString();
            dtpInvoiceDateTo.Text = DateTime.Today.ToString();
            textBox3.Visible = false;
            textBox2.Visible = false;
            txtCustomerID.Visible = false;
            groupBox7.Visible = false;
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            frmAllBalance frm = new frmAllBalance();
            frm.Show();
        }

        private void TabPage1_Click_1(object sender, EventArgs e)
        {

        }

        private void txtCustomerID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
