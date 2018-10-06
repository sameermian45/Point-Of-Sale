using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmSales : Form
    {

        DataTable dTable;
        SqlDataAdapter adp;
        DataSet ds;
        DataTable dt = new DataTable();
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();

        public frmSales()
        {
            InitializeComponent();
        }
        
        private void auto()
        {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            //SELECT TotalBalance from Balance
            String sql = "SELECT top 1 InvoiceNo from Invoice_Info order by InvoiceNo DESC ";
            cmd = new SqlCommand(sql, con);
            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            // cmbCustomerName.Clear();
            if (rdr.Read() == true)
            {
                textBox4.Text = rdr["InvoiceNo"].ToString();
            }
            con.Close();
            
            txtInvoiceNo.Text = "OD-" + GetUniqueKey();

        }
        public string GetUniqueKey()
        {
            string[] pars = textBox4.Text.Trim().Split('-');
            string rol = pars[1];
            var lastAddedId = rol;
            int val = 1;
            var I = ((Convert.ToInt32(lastAddedId)) + (Convert.ToInt32(val)));
            var result = I.ToString().PadLeft(6, '0');
            return result.ToString();
        }
        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox3.Text == "")
                {
                    MessageBox.Show("Please enter Customer Name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox3.Focus();
                    return;
                }    

               
                 if (txtDiscountPer.Text == "")
                {
                    MessageBox.Show("Please enter discount percentage", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDiscountPer.Focus();
                    return;
                }
                if (txtTotalPayment.Text == "")
                {
                    MessageBox.Show("Please enter received payment", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTotalPayment.Focus();
                    return;
                }
                
                if (ListView1.Items.Count == 0)
                {
                    MessageBox.Show("sorry no product added", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

               auto();
               
                con = new SqlConnection(cs.DBConn);
                con.Open();

                string cb = "insert Into Invoice_Info(InvoiceNo,InvoiceDate,SubTotal,DiscountPer,DiscountAmount,GrandTotal,TotalPayment,PaymentDue,Remarks,CustomerName) VALUES ('" + txtInvoiceNo.Text + "','" + dtpInvoiceDate.Text + "'," + txtSubTotal.Text + "," + txtDiscountPer.Text + "," + txtDiscountAmount.Text + "," + txtTotal.Text + "," + txtTotalPayment.Text + "," + txtPaymentDue.Text + ",'" + txtRemarks.Text + "','" + textBox3.Text + "')";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                cmd.ExecuteReader();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Close();


                for (int i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new SqlConnection(cs.DBConn);

                    string cd = "insert Into ProductSold(InvoiceNo,ProductID,ProductName,Quantity,Price,TotalAmount,Type) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                    cmd = new SqlCommand(cd);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("d1", txtInvoiceNo.Text);
                    cmd.Parameters.AddWithValue("d2", ListView1.Items[i].SubItems[1].Text);
                    cmd.Parameters.AddWithValue("d3", ListView1.Items[i].SubItems[2].Text);
                    cmd.Parameters.AddWithValue("d4", ListView1.Items[i].SubItems[4].Text);
                    cmd.Parameters.AddWithValue("d5", ListView1.Items[i].SubItems[3].Text);
                    cmd.Parameters.AddWithValue("d6", ListView1.Items[i].SubItems[5].Text);
                    cmd.Parameters.AddWithValue("d7", ListView1.Items[i].SubItems[6].Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                for (int i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb1 = "update temp_stock set Quantity = Quantity - " + ListView1.Items[i].SubItems[4].Text + " where ProductID= '" + ListView1.Items[i].SubItems[1].Text + "'";
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                con = new SqlConnection(cs.DBConn);
                con.Open();

                string cb2 = "insert Into Customer(CustomerName) VALUES ('" + textBox3.Text + "')";
                cmd = new SqlCommand(cb2);
                cmd.Connection = con;
                cmd.ExecuteReader();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Close();

                con = new SqlConnection(cs.DBConn);
                con.Open();

                string cb4 = "insert into Receable(InvoiceDate,InvoiceNo,TotalPayment,PaymentDue) VALUES (@d1,@d2,@d3,@d4)";

                cmd = new SqlCommand(cb4);

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1", dtpInvoiceDate.Text);
                cmd.Parameters.AddWithValue("@d2", txtInvoiceNo.Text);
                cmd.Parameters.AddWithValue("@d3", txtTotalPayment.Text);
                cmd.Parameters.AddWithValue("@d4", txtPaymentDue.Text);
                cmd.ExecuteReader();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con.Close();

                Save.Enabled = false;
                btnPrint.Enabled = true;
                GetData();
                MessageBox.Show("Successfully Placed", "Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmInvoice_Load(object sender, EventArgs e)
        {
          
            GetData();
            
            
        }

        public void RowsColor()
        {
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                int val = Int32.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                if (val <= 5)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }

                if (val >= 5 && val <= 30)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                }
             }
        }
      public void Calculate()
        {
           
            if (txtDiscountPer.Text != "")
            {
                txtDiscountAmount.Text = Convert.ToDouble(((Convert.ToDouble(txtSubTotal.Text)) * Convert.ToDouble(txtDiscountPer.Text) / 100)).ToString();

               // txtDiscountAmount.Text = Convert.ToInt32(((Convert.ToInt32(txtSubTotal.Text) ) * Convert.ToDouble(txtDiscountPer.Text) / 100)).ToString();
            }
          //  int val1= 0;
            double val1 = 0;
           // int val2 = 0; 
            double val2 = 0;
            double val3 = 0;
           // int val4 = 0;
           // int val5= 0;
            double val4 = 0;
            double val5 = 0;
            double receivedCash=0;
            double.TryParse(txtSubTotal.Text, out val2);
            double.TryParse(txtDiscountAmount.Text, out val3);
            double.TryParse(txtTotal.Text, out val4);
            double.TryParse(txtTotalPayment.Text, out val5);
            val4 = val1 + (val2 - val3);
            txtTotal.Text = val4.ToString();
            double.TryParse(txtTotalPayment.Text,out receivedCash);
            double I = (val4-receivedCash);
            txtPaymentDue.Text = I.ToString();
}
        private void txtSaleQty_TextChanged(object sender, EventArgs e)
        {
            double val1 = 0;
            int val2 = 0;
            double.TryParse(txtPrice.Text, out val1);
            int.TryParse(txtSaleQty.Text, out val2);
            double I = (val1 * val2);
            txtTotalAmount.Text = I.ToString(); 
            /*int val1 = 0;
            int val2 = 0;
            int.TryParse(txtPrice.Text, out val1);
            int.TryParse(txtSaleQty.Text, out val2);
            int I = (val1 * val2);
            txtTotalAmount.Text = I.ToString();*/
        }

        public double subtot()
        {
            int i = 0;
            int j = 0;
           // int k = 0;
            double k = 0;

            i = 0;
            j = 0;
            k = 0;


            try
            {
           
                j = ListView1.Items.Count;
                for (i = 0; i <= j - 1; i++)
                {
                   k= k + Convert.ToDouble(ListView1.Items[i].SubItems[5].Text);      
                 // k = k + Convert.ToInt32(ListView1.Items[i].SubItems[5].Text);
                }
               
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return k;

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (txtProductName.Text=="")
                {
                    MessageBox.Show("Please retrieve product name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (txtSaleQty.Text=="")
                {
                    MessageBox.Show("Please enter no. of sale quantity", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSaleQty.Focus();
                    return;
                }
                int SaleQty = Convert.ToInt32(txtSaleQty.Text);
                if (SaleQty == 0)
                {
                    MessageBox.Show("no. of sale quantity can not be zero", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSaleQty.Focus();
                    return;
                }
              
                if (ListView1.Items.Count==0)
                {
                   
                    ListViewItem lst = new ListViewItem();
                    lst.SubItems.Add(txtProductID.Text);
                    lst.SubItems.Add(txtProductName.Text);
                    lst.SubItems.Add(txtPrice.Text);
                    lst.SubItems.Add(txtSaleQty.Text);
                    lst.SubItems.Add(txtTotalAmount.Text);
                    lst.SubItems.Add(textBox1.Text);

                    ListView1.Items.Add(lst);
                    txtSubTotal.Text = subtot().ToString();
                  
                    Calculate();
                    Calculate();
                    txtProductName.Text = "";
                    txtProductID.Text = "";
                    txtPrice.Text = "";
                    txtAvailableQty.Text = "";
                    txtSaleQty.Text = "";
                    txtTotalAmount.Text = "";
                    txtProduct.Text = "";
                    textBox1.Text = "";
                   
                    return;
                }

                for (int j = 0; j <= ListView1.Items.Count - 1; j++)
                {
                    if (ListView1.Items[j].SubItems[1].Text == txtProductID.Text)
                    {
                        ListView1.Items[j].SubItems[1].Text = txtProductID.Text;
                        ListView1.Items[j].SubItems[2].Text = txtProductName.Text;
                        ListView1.Items[j].SubItems[3].Text = txtPrice.Text;
                        ListView1.Items[j].SubItems[4].Text = (Convert.ToInt32(ListView1.Items[j].SubItems[4].Text) + Convert.ToInt32(txtSaleQty.Text)).ToString();
                        ListView1.Items[j].SubItems[5].Text = (Convert.ToInt32(ListView1.Items[j].SubItems[5].Text) + Convert.ToInt32(txtTotalAmount.Text)).ToString();
                        ListView1.Items[j].SubItems[6].Text = textBox1.Text;
                        txtSubTotal.Text = subtot().ToString();
                        Calculate();
                        txtProductName.Text = "";
                        txtProductID.Text = "";
                        txtPrice.Text = "";
                        txtAvailableQty.Text = "";
                        txtSaleQty.Text = "";
                        txtTotalAmount.Text = "";
                        textBox1.Text = "";
                      
                        return;

                    }
                }
                   
                    ListViewItem lst1 = new ListViewItem();

                    lst1.SubItems.Add(txtProductID.Text);
                    lst1.SubItems.Add(txtProductName.Text);
                    lst1.SubItems.Add(txtPrice.Text);
                    lst1.SubItems.Add(txtSaleQty.Text);
                    lst1.SubItems.Add(txtTotalAmount.Text);
                    lst1.SubItems.Add(textBox1.Text);
                    ListView1.Items.Add(lst1);
                    txtSubTotal.Text = subtot().ToString();
                    Calculate();
                    txtProductName.Text = "";
                    txtProductID.Text = "";
                    txtPrice.Text = "";
                    txtAvailableQty.Text = "";
                    txtSaleQty.Text = "";
                    txtTotalAmount.Text = "";
                    textBox1.Text="";
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListView1.Items.Count == 0)
                {
                    MessageBox.Show("No items to remove", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    
                    int itmCnt = 0;
                    int i = 0;
                    int t = 0;

                    ListView1.FocusedItem.Remove();
                    itmCnt = ListView1.Items.Count;
                    t = 1;

                    for (i = 1; i <= itmCnt + 1; i++)
                    {
                        
                        t = t + 1;

                    }
                    txtSubTotal.Text = subtot().ToString();
                    Calculate();
                }

                btnRemove.Enabled = false;
                if (ListView1.Items.Count == 0)
                {
                    txtSubTotal.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = true;
            //textBox1.Text = ListView1.SelectedItems.ToString();// SelectedIndices.ToString();       
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                String sql = "SELECT Product.ProductID,BrandName,Price,RetailPrice,sum(Quantity),Type from Temp_Stock,Product where Temp_Stock.ProductID=Product.ProductID and (BrandName like '" + txtProduct.Text + "%' ) group by Product.ProductID,BrandName,Price,RetailPrice,Quantity,Type having(Quantity>0) order by BrandName";
                //String sql = "SELECT Product.ProductID,ProductName,Price,RetailPrice,sum(Quantity),Unit,BatchNo from Temp_Stock,Product where Temp_Stock.ProductID=Product.ProductID and (ProductName like '" + txtProduct.Text + "%' or Product.UPC = '" + txtProduct.Text + "') group by Product.ProductID,productname,Price,RetailPrice,Unit,BatchNo,Quantity having(Quantity>0) order by ProductName";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4],rdr[5]);
                }
                con.Close();
                RowsColor();
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



        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    DataGridViewRow dr = dataGridView1.SelectedRows[0];
                    txtProductID.Text = dr.Cells[0].Value.ToString();
                    txtProductName.Text = dr.Cells[1].Value.ToString();
                    txtPrice.Text = dr.Cells[3].Value.ToString();
                    txtAvailableQty.Text = dr.Cells[4].Value.ToString();
                    txtSaleQty.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow dr = dataGridView1.SelectedRows[0];
                txtProductID.Text = dr.Cells[0].Value.ToString();
                txtProductName.Text = dr.Cells[1].Value.ToString();
                txtPrice.Text = dr.Cells[2].Value.ToString();
                txtAvailableQty.Text = dr.Cells[4].Value.ToString();
                textBox1.Text = dr.Cells[5].Value.ToString();
                txtSaleQty.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void GetData1()
        {
            ListView1.Items.Clear();
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();

                cmd = new SqlCommand("SELECT Product.ProductID,ProductSold.Productname,ProductSold.Price,ProductSold.Quantity,ProductSold.TotalAmount from Invoice_Info,ProductSold,Product where Invoice_info.InvoiceNo=ProductSold.InvoiceNo and Product.ProductID=ProductSold.ProductID and invoice_Info.InvoiceNo='" + txtInvoiceNo.Text + "'", con);
                rdr = cmd.ExecuteReader();
                while (rdr.Read() == true)
                {
                    ListViewItem lst = new ListViewItem();
                    lst.SubItems.Add(rdr[0].ToString().Trim());
                    lst.SubItems.Add(rdr[1].ToString().Trim());
                    lst.SubItems.Add(rdr[2].ToString().Trim());
                    lst.SubItems.Add(rdr[3].ToString().Trim());
                    lst.SubItems.Add(rdr[4].ToString().Trim());
                    ListView1.Items.Add(lst);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
               String sql = "SELECT Product.ProductID,BrandName,Price,RetailPrice,sum(Quantity),Type from Temp_Stock,Product where Temp_Stock.ProductID=Product.ProductID group by Product.ProductID,BrandName,Price,RetailPrice,Quantity,Type having(Quantity>0)  order by BrandName";
               // String sql = "SELECT Product.ProductID,BrandName,Price,RetailPrice,sum(Quantity),Type,ProductName from Temp_Stock,Product where Temp_Stock.ProductID=Product.ProductID group by Product.ProductID,BrandName,Price,RetailPrice,Quantity,Type,ProductName having(Quantity>0)  order by BrandName";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5]);
                   //dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5], rdr[6]);
                }
                con.Close();
                RowsColor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Reset()
        {
            txtUserName.Text="";
            txtPassword.Text="";
            txtUserName.Text="";
            txtInvoiceNo.Text = "";
            dtpInvoiceDate.Text = DateTime.Today.ToString();
            txtProductName.Text = "";
            txtProductID.Text = "";
            txtPrice.Text = "";
            txtAvailableQty.Text = "";
            txtSaleQty.Text = "";
            txtTotalAmount.Text = "";
            ListView1.Items.Clear();
            txtDiscountAmount.Text = "";
            txtDiscountPer.Text = "0";
            txtSubTotal.Text = "";
            txtTotal.Text = "";
            textBox3.Text = "";
            txtTotalPayment.Text = "";
            txtPaymentDue.Text = "";
            txtProduct.Text = "";
            txtRemarks.Text = "";
            Save.Enabled = true;
            Delete.Enabled = false;
            btnUpdate.Enabled = false;
            btnRemove.Enabled = false;
            btnPrint.Enabled = false;
            ListView1.Enabled = true;
            Button7.Enabled = true;
            button3.Enabled = false;
            button5.Enabled = false;
            RowsColor();
            GetData();
           
        }

        private void NewRecord_Click(object sender, EventArgs e)
        {
            Reset();
            Reset();
            
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                MessageBox.Show("Please enter user name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Please enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }
            try
            {
                SqlConnection myConnection = default(SqlConnection);
                myConnection = new SqlConnection(cs.DBConn);

                SqlCommand myCommand = default(SqlCommand);

                myCommand = new SqlCommand("SELECT Username,password FROM Registration WHERE Username = @username AND password = @UserPassword", myConnection);
                SqlParameter uName = new SqlParameter("@username", SqlDbType.VarChar);
                SqlParameter uPassword = new SqlParameter("@UserPassword", SqlDbType.VarChar);
                uName.Value = txtUserName.Text;
                uPassword.Value = txtPassword.Text;
                myCommand.Parameters.Add(uName);
                myCommand.Parameters.Add(uPassword);

                myCommand.Connection.Open();

                SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                if (myReader.Read() == true)
                {
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string ct = "select usertype from Registration where Username='" + txtUserName.Text + "' and Password='" + txtPassword.Text + "'";
                    cmd = new SqlCommand(ct);
                    cmd.Connection = con;
                    rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        txtUserType.Text = (rdr.GetString(0));
                    }
                    if ((rdr != null))
                    {
                        rdr.Close();
                    }

                    if (txtUserType.Text.Trim() == "Admin")
                    {
                        delete_records();
                    }
                    if (txtUserType.Text.Trim() == "Cashier")
                    {
                        MessageBox.Show("Sorry you don't have permission", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        txtUserName.Clear();
                        txtPassword.Clear();
                        txtUserName.Focus();
                    }

                }


                else
                {
                    MessageBox.Show("Login is Failed...Try again !", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtUserName.Clear();
                    txtPassword.Clear();
                    txtUserName.Focus();

                }
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Dispose();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        private void delete_records()
        {


            try
            {

                int RowsAffected = 0;
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cq1 = "delete from productSold where InvoiceNo='" + txtInvoiceNo.Text + "'";
                cmd = new SqlCommand(cq1);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cq2 = "delete from Receable where InvoiceNo='" + txtInvoiceNo.Text + "'";
                cmd = new SqlCommand(cq2);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cq = "delete from Invoice_Info where InvoiceNo='" + txtInvoiceNo.Text + "'";
                cmd = new SqlCommand(cq);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();
                for (int i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb1 = "update temp_stock set Quantity = Quantity + " + ListView1.Items[i].SubItems[4].Text + " where ProductID= '" + ListView1.Items[i].SubItems[1].Text + "'";
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }


                if (RowsAffected > 0)
                {
                    MessageBox.Show("Successfully deleted", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("No Record found", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
               // Reset();
            }
        }
        private void frmInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmMainMenu frm = new frmMainMenu();
            frm.lblUser.Text = label6.Text;
            frm.Show();
        }

        private void txtTotalPayment_TextChanged(object sender, EventArgs e)
        {
            double val1 = 0;
            double val2 = 0;
            double.TryParse(txtTotal.Text, out val1);
            double.TryParse(txtTotalPayment.Text, out val2);
            double I = (val1 - val2);
            txtPaymentDue.Text = I.ToString();
            
           // int val1 = 0;
           // int val2 = 0;
           //  int.TryParse(txtTotal.Text, out val1);
           /// int.TryParse(txtTotalPayment.Text, out val2);
           // int I = (val1 - val2);
           // txtPaymentDue.Text = I.ToString();
            // * /
        }

       

        private void txtSaleQty_Validating(object sender, CancelEventArgs e)
        {

            int val1 = 0;
            int val2 = 0;
            int.TryParse(txtAvailableQty.Text, out val1);
            int.TryParse(txtSaleQty.Text, out val2);
            if (val2 > val1)
            {
                MessageBox.Show("Selling quantities are more than available quantities", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSaleQty.Text = "";
                txtTotalAmount.Text = "";
                txtSaleQty.Focus();
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                timer1.Enabled = true;
                rptInvoice rpt = new rptInvoice();
                //The report you created.
                cmd = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                POS_DBDataSet myDS = new POS_DBDataSet();
                //The DataSet you created.
                con = new SqlConnection(cs.DBConn);
                cmd.Connection = con;
                cmd.CommandText = "SELECT * from invoice_info,productsold where invoice_info.invoiceno=productsold.invoiceno  and Invoice_info.invoiceNo='" + txtInvoiceNo.Text + "'";
                cmd.CommandType = CommandType.Text;
                myDA.SelectCommand = cmd;
                myDA.Fill(myDS, "Invoice_Info");
                myDA.Fill(myDS, "ProductSold");
                rpt.SetDataSource(myDS);
                frmInvoiceReport frm = new frmInvoiceReport();
                frm.crystalReportViewer1.ReportSource = rpt;
                frm.Visible=true;
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
           // update Supplier set Suppliername=@d2,address=@d3,City=@d4,ContactNo=@d5 where SupplierID=@d1
            try
            {
            con = new SqlConnection(cs.DBConn);
            con.Open();
            String cb = "update Invoice_info set SubTotal=" + txtSubTotal.Text + ", DiscountPer=" + txtDiscountPer.Text + ",DiscountAmount=" + txtDiscountAmount.Text + ",GrandTotal= " + txtTotal.Text + ",TotalPayment= " + txtTotalPayment.Text + ",PaymentDue= " + txtPaymentDue.Text + ",Remarks='" + txtRemarks.Text + "',CustomerName='" + textBox3.Text + "' where Invoiceno= '" + txtInvoiceNo.Text + "'";
            cmd = new SqlCommand(cb);
            cmd.Connection = con;
            cmd.ExecuteReader();
            con.Close();
            for (int i = 0; i <= ListView1.Items.Count - 1; i++)
            {

                con = new SqlConnection(cs.DBConn);
                con.Open();

                string ct = "select ProductID, InvoiceNo from ProductSold where InvoiceNo=@d1 and ProductID=@d2 ";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("d1", txtInvoiceNo.Text);
                cmd.Parameters.AddWithValue("d2", ListView1.Items[i].SubItems[1].Text);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {

                    con = new SqlConnection(cs.DBConn);

                    string cd = "update ProductSold set InvoiceNo=@d1, ProductID=@d2, ProductName=@d3,Quantity=@d4,Price=@d5,TotalAmount=@d6,Type=@d7 where InvoiceNo=@d1 and ProductID=@d2  ";
                    cmd = new SqlCommand(cd);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("d1", txtInvoiceNo.Text);
                    cmd.Parameters.AddWithValue("d2", ListView1.Items[i].SubItems[1].Text);
                    cmd.Parameters.AddWithValue("d3", ListView1.Items[i].SubItems[2].Text);
                    cmd.Parameters.AddWithValue("d4", ListView1.Items[i].SubItems[4].Text);
                    cmd.Parameters.AddWithValue("d5", ListView1.Items[i].SubItems[3].Text);
                    cmd.Parameters.AddWithValue("d6", ListView1.Items[i].SubItems[5].Text);
                    cmd.Parameters.AddWithValue("d7", ListView1.Items[i].SubItems[6].Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                else
                {
                    con = new SqlConnection(cs.DBConn);

                    string cd = "insert Into ProductSold(InvoiceNo,ProductID,ProductName,Quantity,Price,TotalAmount,Type) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";
                    cmd = new SqlCommand(cd);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("d1", txtInvoiceNo.Text);
                    cmd.Parameters.AddWithValue("d2", ListView1.Items[i].SubItems[1].Text);
                    cmd.Parameters.AddWithValue("d3", ListView1.Items[i].SubItems[2].Text);
                    cmd.Parameters.AddWithValue("d4", ListView1.Items[i].SubItems[4].Text);
                    cmd.Parameters.AddWithValue("d5", ListView1.Items[i].SubItems[3].Text);
                    cmd.Parameters.AddWithValue("d6", ListView1.Items[i].SubItems[5].Text);
                    cmd.Parameters.AddWithValue("d7", ListView1.Items[i].SubItems[6].Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    
                        con = new SqlConnection(cs.DBConn);
                        con.Open();
                        string cb1 = "update temp_stock set Quantity = Quantity - " + ListView1.Items[i].SubItems[4].Text + " where ProductID= '" + ListView1.Items[i].SubItems[1].Text + "'";
                        cmd = new SqlCommand(cb1);
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                        con.Close();
                    
                }
            }


            

            

                    con = new SqlConnection(cs.DBConn);
            con.Open();
            String cb2 = "update Receable set TotalPayment=" + txtTotalPayment.Text + ",PaymentDue=" + txtPaymentDue.Text + " where Invoiceno= '" + txtInvoiceNo.Text + "'";
            cmd = new SqlCommand(cb2);
            cmd.Connection = con;
            cmd.ExecuteReader();
            con.Close();

            GetData();
            btnUpdate.Enabled = false;
            MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        catch (Exception ex)
            {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSalesRecord1 frm = new frmSalesRecord1();
            frm.DataGridView1.DataSource = null;
            frm.dtpInvoiceDateFrom.Text = DateTime.Today.ToString();
            frm.dtpInvoiceDateTo.Text = DateTime.Today.ToString();
           // frm.GroupBox3.Visible = false;
            frm.label9.Text = label6.Text;
            frm.Show();
        }

        private void txtSaleQty_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTotalPayment_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtTaxPer_KeyPress(object sender, KeyPressEventArgs e)
        {
            // allows 0-9, backspace, and decimal
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtDiscountPer_TextChanged(object sender, EventArgs e)
        {
            Calculate();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void payment_Click(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            frmDemand frm = new frmDemand();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Calc.exe");
        }

        private void button3_Click(object sender, EventArgs e)
        {

                //con = new SqlConnection(cs.DBConn);

                //string cd = "Delete from  ProductSold where InvoiceNo=@d1 and ProductID=@d2 ";
                //cmd = new SqlCommand(cd);
                //cmd.Connection = con;
                //cmd.Parameters.AddWithValue("d1", txtInvoiceNo.Text);
                //ListView1.FocusedItem.
                //cmd.Parameters.AddWithValue("d2", ListView1.Items[i].SubItems[1].Text);
                //con.Open();
                //cmd.ExecuteNonQuery();
                //con.Close();
            


        }

        private void txtDiscountPer_KeyPress(object sender, KeyPressEventArgs e)
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

        private void dataGridView1_Enter(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            try
            {
                //DataGridViewRow dr = dataGridView1.SelectedCells[];
                //txtProductID.Text = dr.Cells[0].Value.ToString();
                //txtProductName.Text = dr.Cells[1].Value.ToString();
                //txtPrice.Text = dr.Cells[2].Value.ToString();
                //txtAvailableQty.Text = dr.Cells[4].Value.ToString();
                //txtSaleQty.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            frmupdatemdsn frm = new frmupdatemdsn();
            frm.Show();
            frm.textBox1.Text = txtInvoiceNo.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            GetData();
            GetData1();

        }

        
        
        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmProductsRecord frm = new frmProductsRecord();
            frm.Show();
        }

        private void txtPaymentDue_TextChanged(object sender, EventArgs e)
        {

        }

        

        
    }
}