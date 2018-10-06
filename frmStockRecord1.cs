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
    public partial class frmStockRecord1 : Form
    {
        SqlDataReader rdr = null;
        SqlConnection con = null;
        SqlCommand cmd = null;
        ConnectionString cs = new ConnectionString();
        public frmStockRecord1()
        {
            InitializeComponent();
        }
        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(StockID) as [Stock ID],RTRIM(StockDate) as [Stock Date],RTRIM(Product.ProductID) as [Product Id],RTRIM(ProductName) as [Producr Name],RTRIM(CompanyName) as [Company Name],RTRIM(Price) as [Retail Price],RTRIM(RetailPrice) as [Purchsed Price],RTRIM(Supplier.SupplierID) as [Supplier Id],RTRIM(SupplierName) as [Supplier Name],RTRIM(Quantity) as [Quantity],RTRIM(ExpDate) as [Expier date], RTRIM(SupplierAmount) as [Supplier Amount] from Stock,Product,Supplier where Stock.ProductID=Product.ProductID and Stock.SupplierID=Supplier.SupplierID order by ProductName", con);
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();    
                myDA.Fill(myDataSet, "Stock");
                myDA.Fill(myDataSet, "Product");
                myDA.Fill(myDataSet, "Supplier");
                dataGridView1.DataSource = myDataSet.Tables["Supplier"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Product"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Stock"].DefaultView;
           
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmStockRecord_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void txtProductname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();

                cmd = new SqlCommand("SELECT RTRIM(StockID) as [Stock ID],RTRIM(StockDate) as [Stock Date],RTRIM(Product.ProductID) as [Product Id],RTRIM(ProductName) as [Producr Name],RTRIM(CompanyName) as [Company Name],RTRIM(Price) as [Retail Price],RTRIM(RetailPrice) as [Purchsed Price],RTRIM(Supplier.SupplierID) as [Supplier Id],RTRIM(SupplierName) as [Supplier Name],RTRIM(Quantity) as [Quantity],RTRIM(ExpDate) as [Expier date], RTRIM(SupplierAmount) as [Supplier Amount] from Stock,Product,Supplier where Stock.ProductID=Product.ProductID and Stock.SupplierID=Supplier.SupplierID and productname like '" + txtProductname.Text + "%' order by ProductName", con);
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Stock");
                myDA.Fill(myDataSet, "Product");
                myDA.Fill(myDataSet, "Supplier");
                dataGridView1.DataSource = myDataSet.Tables["Supplier"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Product"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Stock"].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string strRowNumber = (e.RowIndex + 1).ToString();
            SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);
            if (dataGridView1.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
            {
                dataGridView1.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
            }
            Brush b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
     
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow dr = dataGridView1.SelectedRows[0];
                this.Hide();
                frmStock frm = new frmStock();
                frm.Show();
                frm.txtStockID.Text = dr.Cells[0].Value.ToString();
                frm.dtpStockDate.Text = dr.Cells[1].Value.ToString();
                frm.txtProductID.Text = dr.Cells[2].Value.ToString();
                frm.txtProductName.Text = dr.Cells[3].Value.ToString();
                frm.textBox4.Text = dr.Cells[4].Value.ToString();
                frm.textBox3.Text = dr.Cells[5].Value.ToString();
                frm.textBox2.Text = dr.Cells[6].Value.ToString();
                frm.textBox1.Text = dr.Cells[11].Value.ToString();
                frm.textBox5.Text = dr.Cells[11].Value.ToString();
                frm.txtSupplierID.Text = dr.Cells[7].Value.ToString();
                frm.cmbSupplierName.Text = dr.Cells[8].Value.ToString();
                frm.dateTimePicker1.Text = dr.Cells[10].Value.ToString();

                frm.txtQty.Text = dr.Cells[9].Value.ToString();
                frm.txtQty1.Text = dr.Cells[9].Value.ToString();
                frm.btnUpdate.Enabled = true;
                frm.btnDelete.Enabled = true;
                frm.btnSave.Enabled = false;
                frm.label8.Text = label1.Text;
           }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmStockRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmStock frm = new frmStock();
            frm.label8.Text = label1.Text;
            frm.Show();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
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

                rowsTotal = dataGridView1.RowCount - 1;
                colsTotal = dataGridView1.Columns.Count - 1;
                var _with1 = excelWorksheet;
                _with1.Cells.Select();
                _with1.Cells.Delete();
                for (iC = 0; iC <= colsTotal; iC++)
                {
                    _with1.Cells[1, iC + 1].Value = dataGridView1.Columns[iC].HeaderText;
                }
                for (I = 0; I <= rowsTotal - 1; I++)
                {
                    for (j = 0; j <= colsTotal; j++)
                    {
                        _with1.Cells[I + 2, j + 1].value = dataGridView1.Rows[I].Cells[j].Value;
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

        private void button2_Click(object sender, EventArgs e)
        {
            txtProductname.Text = "";
            dtpStockDateFrom.Text = System.DateTime.Today.ToString();
            dtpStockDateTo.Text = System.DateTime.Today.ToString();
            GetData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(StockID) as [Stock ID],RTRIM(StockDate) as [Stock Date],RTRIM(Product.ProductID) as [Product Id],RTRIM(ProductName) as [Producr Name],RTRIM(CompanyName) as [Company Name],RTRIM(Price) as [Retail Price],RTRIM(RetailPrice) as [Purchsed Price],RTRIM(Supplier.SupplierID) as [Supplier Id],RTRIM(SupplierName) as [Supplier Name],RTRIM(Quantity) as [Quantity],RTRIM(ExpDate) as [Expier date], RTRIM(SupplierAmount) as [Supplier Amount] from Stock,Product,Supplier where Stock.ProductID=Product.ProductID and Stock.SupplierID=Supplier.SupplierID and StockDate between @d1 and @d2 order by ProductName", con);
                // cmd = new SqlCommand("SELECT RTRIM(StockID) as [Stock ID],RTRIM(StockDate) as [Stock Date],RTRIM(Product.ProductID) as [Product Id],RTRIM(ProductName) as [Producr Name],RTRIM(CompanyName) as [Company Name],RTRIM(Price) as [Retail Price],RTRIM(RetailPrice) as [Purchsed Price],RTRIM(Supplier.SupplierID) as [Supplier Id],RTRIM(SupplierName) as [Supplier Name],RTRIM(Quantity) as [Quantity],RTRIM(ExpDate) as [Expier date], RTRIM(SupplierAmount) as [Supplier Amount] from Stock,Product,Supplier where Stock.ProductID=Product.ProductID and Stock.SupplierID=Supplier.SupplierID and StockDate between @d1 and @d2 order by ProductName", con);
                cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "StockDate").Value = dtpStockDateFrom.Value.Date;
                cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "StockDate").Value = dtpStockDateTo.Value.Date;
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Stock");
                myDA.Fill(myDataSet, "Product");
                myDA.Fill(myDataSet, "Supplier");
                dataGridView1.DataSource = myDataSet.Tables["Supplier"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Product"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Stock"].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
