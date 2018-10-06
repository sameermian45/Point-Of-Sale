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
    public partial class frmExpenseRecord : Form
    {
        SqlDataReader rdr;
       DataTable dTable;
    SqlConnection con = null;
    SqlDataAdapter adp;
    DataSet ds;
    SqlCommand cmd = null;
    DataTable dt= new DataTable();
    ConnectionString cs = new ConnectionString();
        public frmExpenseRecord()
        {
            InitializeComponent();
        }

        public void FillCombo()
        {

            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                adp = new SqlDataAdapter();
                adp.SelectCommand = new SqlCommand("SELECT distinct SupplierName FROM Supplier ", con);
                ds = new DataSet("ds");
                adp.Fill(ds);
                dTable = ds.Tables[0];
                cmbCustomerName.Items.Clear();
                foreach (DataRow drow in dTable.Rows)
                {
                    cmbCustomerName.Items.Add(drow[0].ToString());

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmExpenseRecord_Load(object sender, EventArgs e)
        {
            FillCombo();
            
           
        }

        







        
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                
                GroupBox3.Visible = true;
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
                textBox3.Text = sum.ToString();
                textBox4.Text = sum1.ToString();
                txtCustomerID.Text = Convert.ToDecimal((Convert.ToDecimal(textBox3.Text) - Convert.ToDecimal(textBox4.Text))).ToString();
                
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        
        
         
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void GroupBox3_Enter(object sender, EventArgs e)
        {

        }




      
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void txtCustomerID_TextChanged(object sender, EventArgs e)
        {

        }

        private void TabPage1_Click(object sender, EventArgs e)
        {

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (DataGridView1.DataSource == null)
            {
                MessageBox.Show("Sorry nothing to export into excel sheet..", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            Excel.Application xlApp = new Excel.Application();

            try
            {
                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = DataGridView1.RowCount - 1;
                colsTotal = DataGridView1.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = DataGridView1.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = DataGridView1.Rows[I].Cells[j].Value;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //RELEASE ALLOACTED RESOURCES
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                xlApp = null;
            }


        }

        private void Button7_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
            DataGridView1.DataSource = null;
            dtpInvoiceDateFrom.Text = DateTime.Today.ToString();
            dtpInvoiceDateTo.Text = DateTime.Today.ToString();
            GroupBox3.Visible = false;
            DataGridView1.DataSource = null;
           // dataGridView5.DataSource = null;
        }

        private void Button9_Click(object sender, EventArgs e)
        {
           
            DataGridView3.DataSource = null;
            cmbCustomerName.Text = "";
            groupBox4.Visible = false;
           // dataGridView6.DataSource = null;

        }

        private void GroupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void cmbCustomerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
              
                groupBox4.Visible = true;
                cmbCustomerName.Text = cmbCustomerName.Text.Trim();
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(ExpenseNo) as [Bill ID],RTRIM(SuplierName) as [Name],RTRIM(ExpenseDate) as [Expense Date],RTRIM(ExpencseDetail) as [Expense Details],RTRIM(Total) as [Total Amount],RTRIM(Amount) as [Amount Received],RTRIM(Remaining) as [Remaining Amount] from personalExpense where ExpenseDate between @d1 and @d2 and SuplierName='" + cmbCustomerName.Text.Trim() + "' order by ExpenseDate", con);
                cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "ExpenseDate").Value = dateTimePicker2.Value.Date;
                cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "ExpenseDate").Value = dateTimePicker1.Value.Date;
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "personalExpense");
                DataGridView3.DataSource = myDataSet.Tables["personalExpense"].DefaultView;
                Int64 sum = 0;
                Int64 sum1 = 0;

                foreach (DataGridViewRow r in this.DataGridView3.Rows)
                {
                    Int64 i = Convert.ToInt64(r.Cells[4].Value);
                    Int64 j = Convert.ToInt64(r.Cells[5].Value);
                    sum = sum + i;
                    sum1 = sum1 + j;

                }
                textBox6.Text = sum.ToString();
                textBox8.Text = sum1.ToString();
                textBox9.Text = Convert.ToDecimal((Convert.ToDecimal(textBox6.Text) - Convert.ToDecimal(textBox8.Text))).ToString();

                
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

                
                
         
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void DataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void TabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (DataGridView3.DataSource == null)
            {
                MessageBox.Show("Sorry nothing to export into excel sheet..", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int rowsTotal = 0;
            int colsTotal = 0;
            int I = 0;
            int j = 0;
            int iC = 0;
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            Excel.Application xlApp = new Excel.Application();

            try
            {
                Excel.Workbook excelBook = xlApp.Workbooks.Add();
                Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelBook.Worksheets[1];
                xlApp.Visible = true;

                rowsTotal = DataGridView3.RowCount - 1;
                colsTotal = DataGridView3.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = DataGridView3.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = DataGridView3.Rows[I].Cells[j].Value;
                    }
                }
                _with1.Rows["1:1"].Font.FontStyle = "Bold";
                _with1.Rows["1:1"].Font.Size = 12;

                _with1.Cells.Columns.AutoFit();
                _with1.Cells.Select();
                _with1.Cells.EntireColumn.AutoFit();
                _with1.Cells[1, 1].Select();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //RELEASE ALLOACTED RESOURCES
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                xlApp = null;
            }

        }
    }
}
