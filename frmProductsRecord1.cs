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
using System.IO;
namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmProductsRecord1 : Form
    {
       SqlDataReader rdr = null;
       SqlConnection con = null;
       SqlCommand cmd = null;
       ConnectionString cs = new ConnectionString();
        public frmProductsRecord1()
        {
            InitializeComponent();
        }
        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
               // cmd = new SqlCommand("SELECT RTRIM(ProductID),RTRIM(ProductName),RTRIM(Category.ID),RTRIM(CategoryName),RTRIM(SubCategory.ID),RTRIM(SubCategoryName),RTRIM(Price),RTRIM(RetailPrice),RTRIM(CompanyName) from Product,Category,SubCategory where Product.CategoryID=Category.ID and Product.SubCategoryID=SubCategory.ID order by Productname", con);

                cmd = new SqlCommand("SELECT RTRIM(ProductID) as[ProductID], RTRIM(ProductName) as [Product Name],RTRIM(BrandName) as [Brand Name],RTRIM(Type) as [Type],RTRIM(Price) as [Retail Price],RTRIM(RetailPrice) as [Purchased Price],RTRIM(CompanyName) as [Company Name],RTRIM(CompanyAdress) as [Company Adress],RTRIM(Category.ID) as [Category Id],RTRIM(CategoryName) as [Category Name],RTRIM(SubCategory.ID) as [SubCategory Id],RTRIM(SubCategoryName) as [SubCategory Name],RTRIM(Features) as [Discription],RTRIM(CompanyAdress) as [Company Adress] from Product,Category,SubCategory where Product.CategoryID=Category.ID and Product.SubCategoryID=SubCategory.ID order by Productname", con);
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Product");
                myDA.Fill(myDataSet, "Category");
                myDA.Fill(myDataSet, "SubCategory");
                dataGridView1.DataSource = myDataSet.Tables["SubCategory"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Category"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Product"].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmProductsRecord_Load(object sender, EventArgs e)
        {
            GetData();
        }

       
        private void Button3_Click(object sender, EventArgs e)
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

        private void frmProductsRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmStock frm = new frmStock();
            frm.label8.Text = label1.Text;
            frm.Show();
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
                frm.txtProductID.Text = dr.Cells[0].Value.ToString();   // ProdcutID
                frm.txtProductName.Text = dr.Cells[2].Value.ToString(); //product Name
                frm.textBox3.Text = dr.Cells[4].Value.ToString(); // Retail price
                frm.textBox2.Text = dr.Cells[5].Value.ToString(); // Purchase Price 
                frm.textBox4.Text = dr.Cells[6].Value.ToString();// Compnay Name
                frm.textBox7.Text = dr.Cells[3].Value.ToString();// medicine type
                frm.label8.Text = label1.Text;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtProductname_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(ProductID) as[ProductID], RTRIM(ProductName) as [Product Name],RTRIM(BrandName) as [Brand Name],RTRIM(Type) as [Type],RTRIM(Price) as [Retail Price],RTRIM(RetailPrice) as [Purchased Price],RTRIM(CompanyName) as [Company Name],RTRIM(CompanyAdress) as [Company Adress],RTRIM(Category.ID) as [Category Id],RTRIM(CategoryName) as [Category Name],RTRIM(SubCategory.ID) as [SubCategory Id],RTRIM(SubCategoryName) as [SubCategory Name],RTRIM(Features) as [Discription],RTRIM(CompanyAdress) as [Company Adress] from Product,Category,SubCategory where Product.CategoryID=Category.ID and Product.SubCategoryID=SubCategory.ID and  ProductName like'" + txtProductname.Text + "%' order by ProductName", con);
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Product");
                myDA.Fill(myDataSet, "Category");
                myDA.Fill(myDataSet, "SubCategory");
                dataGridView1.DataSource = myDataSet.Tables["SubCategory"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Category"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Product"].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCategory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(ProductID) as[ProductID], RTRIM(ProductName) as [Product Name],RTRIM(BrandName) as [Brand Name],RTRIM(Type) as [Type],RTRIM(Price) as [Retail Price],RTRIM(RetailPrice) as [Purchased Price],RTRIM(CompanyName) as [Company Name],RTRIM(CompanyAdress) as [Company Adress],RTRIM(Category.ID) as [Category Id],RTRIM(CategoryName) as [Category Name],RTRIM(SubCategory.ID) as [SubCategory Id],RTRIM(SubCategoryName) as [SubCategory Name],RTRIM(Features) as [Discription],RTRIM(CompanyAdress) as [Company Adress] from Product,Category,SubCategory where Product.CategoryID=Category.ID and Product.SubCategoryID=SubCategory.ID and  CategoryName like'" + txtCategory.Text + "%' order by CategoryName", con);
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Product");
                myDA.Fill(myDataSet, "Category");
                myDA.Fill(myDataSet, "SubCategory");
                dataGridView1.DataSource = myDataSet.Tables["SubCategory"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Category"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Product"].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSubCategory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(ProductID) as[ProductID], RTRIM(ProductName) as [Product Name],RTRIM(BrandName) as [Brand Name],RTRIM(Type) as [Type],RTRIM(Price) as [Retail Price],RTRIM(RetailPrice) as [Purchased Price],RTRIM(CompanyName) as [Company Name],RTRIM(CompanyAdress) as [Company Adress],RTRIM(Category.ID) as [Category Id],RTRIM(CategoryName) as [Category Name],RTRIM(SubCategory.ID) as [SubCategory Id],RTRIM(SubCategoryName) as [SubCategory Name],RTRIM(Features) as [Discription],RTRIM(CompanyAdress) as [Company Adress] from Product,Category,SubCategory where Product.CategoryID=Category.ID and Product.SubCategoryID=SubCategory.ID and  SubCategoryName like '" + txtSubCategory.Text + "%' order by SubCategoryName", con);
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Product");
                myDA.Fill(myDataSet, "Category");
                myDA.Fill(myDataSet, "SubCategory");
                dataGridView1.DataSource = myDataSet.Tables["SubCategory"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Category"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Product"].DefaultView;
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void BrandName_TextChanged(object sender, EventArgs e)
        {

            try
            {

                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(ProductID) as[ProductID], RTRIM(ProductName) as [Product Name],RTRIM(BrandName) as [Brand Name],RTRIM(Type) as [Type],RTRIM(Price) as [Retail Price],RTRIM(RetailPrice) as [Purchased Price],RTRIM(CompanyName) as [Company Name],RTRIM(CompanyAdress) as [Company Adress],RTRIM(Category.ID) as [Category Id],RTRIM(CategoryName) as [Category Name],RTRIM(SubCategory.ID) as [SubCategory Id],RTRIM(SubCategoryName) as [SubCategory Name],RTRIM(Features) as [Discription],RTRIM(CompanyAdress) as [Company Adress] from Product,Category,SubCategory where Product.CategoryID=Category.ID and Product.SubCategoryID=SubCategory.ID and  BrandName like'" + BrandName.Text + "%' order by BrandName", con);
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Product");
                myDA.Fill(myDataSet, "Category");
                myDA.Fill(myDataSet, "SubCategory");
                dataGridView1.DataSource = myDataSet.Tables["SubCategory"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Category"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Product"].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
    }
}
