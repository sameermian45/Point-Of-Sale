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
    public partial class frmStockRecord : Form
    {
        SqlDataReader rdr = null;
        SqlConnection con = null;
        SqlCommand cmd = null;
        ConnectionString cs = new ConnectionString();
        public frmStockRecord()
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

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtProductname.Text == "")
                {
                    MessageBox.Show("Please enter product name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductname.Focus();
                    return;
                }
                Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;
                rptStock rpt = new rptStock();
                //The report you created.
                cmd = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                POS_DBDataSet myDS = new POS_DBDataSet();
                //The DataSet you created.
                con = new SqlConnection(cs.DBConn);
                cmd.Connection = con;
                cmd.CommandText = "SELECT * from Stock,Product,Supplier where Stock.ProductID=Product.ProductID and Stock.SupplierID=Supplier.SupplierID and ProductName like '" + txtProductname.Text + "%' order by StockDate";
                cmd.CommandType = CommandType.Text;
                myDA.SelectCommand = cmd;
                myDA.Fill(myDS, "Stock");
                myDA.Fill(myDS, "Product");
                myDA.Fill(myDS, "Supplier");
                rpt.SetDataSource(myDS);
                frmStockReport frm = new frmStockReport();
                frm.crystalReportViewer1.ReportSource = rpt;
                frm.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            timer1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
              Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;
                rptStock rpt = new rptStock();
                //The report you created.
                cmd = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                POS_DBDataSet myDS = new POS_DBDataSet();
                //The DataSet you created.
                con = new SqlConnection(cs.DBConn);
                cmd.Connection = con;
                cmd.CommandText = "SELECT * from Stock,Product,Supplier where Stock.ProductID=Product.ProductID and Stock.SupplierID=Supplier.SupplierID and StockDate Between @d1 and @d2 order by StockDate";
                cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "StockDate").Value = dtpStockDateFrom.Value.Date;
                cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "StockDate").Value = dtpStockDateTo.Value.Date;
                cmd.CommandType = CommandType.Text;
                myDA.SelectCommand = cmd;
                myDA.Fill(myDS, "Stock");
                myDA.Fill(myDS, "Product");
                myDA.Fill(myDS, "Supplier");
                rpt.SetDataSource(myDS);
                frmStockReport frm = new frmStockReport();
                frm.crystalReportViewer1.ReportSource = rpt;
                frm.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
