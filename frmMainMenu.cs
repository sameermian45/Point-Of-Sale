using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmMainMenu : Form
    {
        SqlDataReader rdr = null;
        SqlConnection con = null;
        SqlCommand cmd = null;
        ConnectionString cs = new ConnectionString();
        public frmMainMenu()
        {
            InitializeComponent();
        }

        
        private void productToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmProduct frm = new frmProduct();
            frm.Show();
        }

        private void notepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Notepad.exe");
        }

        private void calculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Calc.exe");
        }

        

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategory frm = new frmCategory();
            frm.Show();
        }

        private void companyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSubCategory frm = new frmSubCategory();
            frm.Show();
        }

        

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmCategory o1 = new frmCategory();
            o1.Hide();
            frmSubCategory o2 = new frmSubCategory();
            o2.Hide();
            frmProduct o3 = new frmProduct();
            o3.Hide();
            
            frmSuppliersRecord o8 = new frmSuppliersRecord();
            o8.Hide();
            frmExpenseRecord1 o9 = new frmExpenseRecord1();
            o9.Hide();
            frmSalesRecord2 o10 = new frmSalesRecord2();
            o10.Hide();
            frmLogin frm = new frmLogin();
            frm.Show();
            frm.txtUserName.Text = "";
            frm.txtPassword.Text = "";
            frm.ProgressBar1.Visible = false;
            frm.txtUserName.Focus();
        }

        private void frmMainMenu_Load(object sender, EventArgs e)
        {
         ToolStripStatusLabel4.Text = System.DateTime.Now.ToString();
         GetData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ToolStripStatusLabel4.Text = System.DateTime.Now.ToString();
        }

        private void productsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmProduct frm = new frmProduct();
            frm.Show();
        }

        private void productsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
           
            frmProductsRecord frm = new frmProductsRecord();
            frm.Show();
        }

     
        private void stockToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmStock frm = new frmStock();
            frm.label8.Text = lblUser.Text;
            frm.Show();
        }

        private void stockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmStock frm = new frmStock();
            frm.label8.Text = lblUser.Text;
            frm.Show();
        }

        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                String sql = "SELECT Product.ProductID,BrandName,RetailPrice,Price,sum(Quantity),sum(Price*Quantity),ExpDate1,Type from Temp_Stock,Product where Temp_Stock.ProductID=Product.ProductID group by Product.productID,BrandName,RetailPrice,Price,Quantity,ExpDate1,Type having(Quantity>0)  order by BrandName";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5], rdr[6],rdr[7]);
                }
                    
                RowsColor();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

   
        private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSales frm = new frmSales();
            frm.label6.Text = lblUser.Text;
            frm.Show();
        }

        private void salesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSales frm = new frmSales();
            frm.label6.Text = lblUser.Text;
            frm.Show();
        }

        private void salesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmSalesRecord1 frm = new frmSalesRecord1();
            frm.Show();
        }

        private void loginDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLoginDetails frm = new frmLoginDetails();
            frm.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
            con = new SqlConnection(cs.DBConn);
                con.Open();
                String sql = "SELECT Product.ProductID,BrandName,RetailPrice,Price,sum(Quantity),sum(Price*Quantity),ExpDate1,Type from Temp_Stock,Product where Temp_Stock.ProductID=Product.ProductID and BrandName like '" + txtProductName.Text + "%'group by Product.productID,BrandName,RetailPrice,Price,Quantity,ExpDate1,Type having(Quantity>0)  order by BrandName";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5], rdr[6],rdr[7]);
                }
                
                con.Close();
                RowsColor();
            }
                
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmMainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Dispose();
            this.Hide();
            frmCategory o1 = new frmCategory();
            o1.Hide();
            frmSubCategory o2 = new frmSubCategory();
            o2.Hide();
            frmProduct o3 = new frmProduct();
            o3.Hide();

            frmSuppliersRecord o8 = new frmSuppliersRecord();
            o8.Hide();
            frmExpenseRecord1 o9 = new frmExpenseRecord1();
            o9.Hide();
            frmSalesRecord2 o10 = new frmSalesRecord2();
            o10.Hide();
            frmLogin frm = new frmLogin();
            frm.Show();
            frm.txtUserName.Text = "";
            frm.txtPassword.Text = "";
            frm.ProgressBar1.Visible = false;
            frm.txtUserName.Focus();
            
        }

        private void profileEntryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSuppliers frm = new frmSuppliers();
            frm.Show();
        }

        private void suppliersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmSuppliersRecord frm = new frmSuppliersRecord();
            frm.Show();
        }

        private void stockToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmStockRecord frm = new frmStockRecord();
            frm.Show();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            
        }

        

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            
        }

        private void expencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void jurnalLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void balanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAllBalance frm = new frmAllBalance();
            frm.Show();
        }

        private void toolStripMenuItem4_Click_1(object sender, EventArgs e)
        {
            
        }

        public void RowsColor()
        {
            try
            {
   
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    int val = Int32.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    if (val <= 5)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    if (val >= 5 && val <= 30){
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void itemSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void balanceToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmBalance frm = new frmBalance();
            frm.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            
        }

        private void balanceHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemSize frm = new ItemSize();
            frm.Show();
        }

        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
        {
            

        }

        private void staffAttandanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void advanceSalaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripMenuItem4_Click_2(object sender, EventArgs e)
        {
            this.Hide();
            frmBillUpdate frm = new frmBillUpdate();
            frm.Show();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            frmupdatemdsn frm = new frmupdatemdsn();
            frm.Show();
        }


       

       

        

        private void companySuppilersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSuppliers frm = new frmSuppliers();
            frm.Show();
        }

        

       

        
        

        

        private void companyAdjestedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ItemSize frm = new ItemSize();
            frm.Show();
        }

        private void companySuppliersExpencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmpersonalUse frm = new frmpersonalUse();
            frm.Show();
        }

        private void utalityExpencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExpences frm = new frmExpences();
            frm.Show();
        }

        private void generalLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmjurnalledger frm = new frmjurnalledger();
            frm.Show();
        }

        private void expenceRecordViewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmExpenseRecord frm = new frmExpenseRecord();
            frm.Show();
        }
    }
}
