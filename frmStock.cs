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
namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmStock : Form
    {
        SqlConnection con = null;
        SqlCommand cmd = null;
        SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public frmStock()
        {
            InitializeComponent();
        }
        public void FillCombo()
        {
            try
            {

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string ct = "select RTRIM(SupplierName) from Supplier order by SupplierName";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    cmbSupplierName.Items.Add(rdr[0]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmStock_Load(object sender, EventArgs e)
        {
     
            FillCombo();
        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "123456789".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        private void auto()
        {
            txtStockID.Text = "ST-" + GetUniqueKey(6);
        }
      
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmProductsRecord1 frm = new frmProductsRecord1();
            frm.label1.Text = label8.Text;
            frm.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtProductName.Text == "")
            {
                MessageBox.Show("Please retrieve product name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProductName.Focus();
                return;
            }
            if (txtQty.Text == "")
            {
                MessageBox.Show("Please enter quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQty.Focus();
                return;
            }
            if (cmbSupplierName.Text == "")
            {
                MessageBox.Show("Please select Supplier name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbSupplierName.Focus();
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please select Supplier name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Focus();
                return;
            }
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
               
                string ct = "select ProductID from temp_Stock where ProductID='" + txtProductID.Text + "'";
                cmd = new SqlCommand(ct);
                cmd.Connection = con;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                     con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb2 = "Update Temp_Stock set Quantity=Quantity + " + txtQty.Text + " where ProductID='"+ txtProductID.Text + "'";
                    cmd = new SqlCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();
                 
                }
                else
                {
                     con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb1 = "insert into Temp_Stock(ProductID,Quantity) VALUES ('" + txtProductID.Text + "'," + txtQty.Text + ")";
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                   
                    cmd.ExecuteReader();
                    con.Close(); 
                }
                auto();
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cb = "insert into Stock(StockID,ProductID,SupplierID,Quantity,StockDate,ExpDate,SupplierAmount) VALUES ('" + txtStockID.Text + "','" + txtProductID.Text + "','" + txtSupplierID.Text + "'," + txtQty.Text + ",@d1,'" + dateTimePicker1.Text + "', '" + textBox1.Text + "')";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1", dtpStockDate.Text);
                cmd.ExecuteReader();
                con.Close();
                {
                    con = new SqlConnection(cs.DBConn);
                    con.Open();

                    string cb1 = "update SaplierRemaing set Remaining  = Remaining+ " + textBox1.Text + " where SuplierName='" + cmbSupplierName.Text + "'";
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cb3 = "Update Product set Price='"+textBox3.Text+"',RetailPrice='"+textBox2.Text+"',ExpDate1='"+ dateTimePicker1.Text +"' where ProductID='" + txtProductID.Text + "'";
                cmd = new SqlCommand(cb3);
                cmd.Connection = con;

                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("Successfully saved", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      
        private void Reset()
        {
            dateTimePicker1.Text = DateTime.Today.ToString();
            textBox1.Text="";
            txtProductName.Text = "";
            txtQty.Text = "";
            cmbSupplierName.Text = "";
            txtStockID.Text = "";
            dtpStockDate.Text = DateTime.Today.ToString();
            
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                delete_records();
            }
        }
        private void delete_records()
        {

            try
            {

                int RowsAffected = 0;
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cb2 = "Update Temp_Stock set Quantity=Quantity - " + txtQty1.Text + " where ProductID='" + txtProductID.Text + "'";
                cmd = new SqlCommand(cb2);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cq = "delete from Stock where StockID='" + txtStockID.Text + "'";
                cmd = new SqlCommand(cq);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();
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
            }
        }

        private void frmStock_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmMainMenu frm = new frmMainMenu();
            frm.lblUser.Text = label8.Text;
            frm.Show();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmStockRecord1 frm = new frmStockRecord1();
            frm.label1.Text = label8.Text;
            frm.Show();
            frm.GetData();
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
           
                if (txtProductName.Text == "")
                {
                    MessageBox.Show("Please retrieve product name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtProductName.Focus();
                    return;
                }
                if (txtQty.Text == "")
                {
                    MessageBox.Show("Please enter quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtQty.Focus();
                    return;
                }
                if (cmbSupplierName.Text == "")
                {
                    MessageBox.Show("Please select Supplier name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbSupplierName.Focus();
                    return;
                }
                Decimal val1 = 0;
                Decimal val2 = 0;
                Decimal.TryParse(textBox6.Text, out val1);
                if (val1 < val2)
                {
                    MessageBox.Show("Sorry ! You cannot Overcome the Stock", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
                try
                {
                    con = new SqlConnection(cs.DBConn);
                    con.Open();

                    string ct = "select ProductID from temp_Stock where ProductID='" + txtProductID.Text + "'";
                    cmd = new SqlCommand(ct);
                    cmd.Connection = con;
                    rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        con = new SqlConnection(cs.DBConn);
                        con.Open();
                        string cb2 = "Update Temp_Stock set Quantity=Quantity + " + txtQty.Text + " - "+ txtQty1.Text + " where ProductID='" + txtProductID.Text + "'";
                        cmd = new SqlCommand(cb2);
                        cmd.Connection = con;
                        cmd.ExecuteReader();
                        con.Close();

                    }
                    else
                    {
                        con = new SqlConnection(cs.DBConn);
                        con.Open();
                        string cb1 = "insert into Temp_Stock(ProductID,Quantity) VALUES ('" + txtProductID.Text + "'," + txtQty.Text + ")";
                        cmd = new SqlCommand(cb1);
                        cmd.Connection = con;

                        cmd.ExecuteReader();
                        con.Close();
                    }
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb = "Update Stock set ProductID='" + txtProductID.Text + "',SupplierID='" + txtSupplierID.Text + "',Quantity=" + txtQty.Text + ",StockDate= '" + dtpStockDate.Text + "',ExpDate='" + dateTimePicker1.Text + "',SupplierAmount='"+textBox1.Text+"' where StockID='" + txtStockID.Text + "'";
                    cmd = new SqlCommand(cb);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();

                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb3 = "Update Product set ExpDate1='" + dateTimePicker1.Text + "',RetailPrice='" + textBox2.Text + "',Price='" + textBox3.Text + "' where ProductID='" + txtProductID.Text + "'";
                    cmd = new SqlCommand(cb3);
                    cmd.Connection = con;

                    cmd.ExecuteReader();
                    con.Close();
                    {
                        con = new SqlConnection(cs.DBConn);
                        con.Open();

                        string cb1 = "update SaplierRemaing set Remaining  = Remaining+ " + textBox6.Text + " where SuplierName='" + cmbSupplierName.Text + "'";
                        cmd = new SqlCommand(cb1);
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnUpdate.Enabled = false;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

               private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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
               
        private void cmbSupplierName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                con = new SqlConnection(cs.DBConn);

                con.Open();
                cmd = con.CreateCommand();

                cmd.CommandText = "SELECT SupplierID from Supplier WHERE SupplierName = '" + cmbSupplierName.Text + "'";
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    txtSupplierID.Text = rdr.GetString(0).Trim();
                }
                if ((rdr != null))
                {
                    rdr.Close();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            decimal val1 = 0;
            decimal val2 = 0;
            decimal.TryParse(textBox2.Text, out val1);
            decimal.TryParse(txtQty.Text, out val2);
            decimal I = (val1 * val2);
            textBox1.Text = I.ToString();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            decimal val1 = 0;
            decimal val2 = 0;
            decimal.TryParse(textBox1.Text, out val1);
            decimal.TryParse(textBox5.Text, out val2);
            decimal I = (val1 - val2);
            textBox6.Text = I.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

      
    }
}
