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
    public partial class frmExpences : Form
    {

        
    


        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public frmExpences()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dtpInvoiceDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }



        
        public void GetData()
        {
                 try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                String sql = "SELECT BalanceAmount from Balance where id='1' ";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                textBox2.Clear();
                if (rdr.Read() == true)
                {
                    textBox2.Text = rdr["BalanceAmount"].ToString();

                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmExpences_Load(object sender, EventArgs e)
        {
            txtSaleQty.Visible = false;
            textBox1.Visible = false;
            txtFeatures.Visible = false;
            button4.Enabled = false;
            textBox3.Visible = false;
            richTextBox1.Visible = false;
            label13.Visible = false;
            label6.Visible = false;
            textBox4.Visible = false;
            label9.Visible = false;
            button6.Enabled = false;
            

        }
            
            
            
          
              
        

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmUserRecord1 frm = new frmUserRecord1();
            //frm.label1.Text = label6.Text;
            frm.Visible = true;
            
        }


        
        
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmBalance frm = new frmBalance();
            frm.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSaleQty_TextChanged(object sender, EventArgs e)
        {
            Decimal val1 = 0;
            Decimal val2 = 0;
            Decimal.TryParse(textBox2.Text, out val1);
            Decimal.TryParse(txtSaleQty.Text, out val2);
            Decimal I = (val1 - val2);
            textBox1.Text = I.ToString();
        }

        private void txtSaleQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void auto()
        {
            txtInvoiceNo.Text = "EX-" + GetUniqueKey(6);

        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars = "123456".ToCharArray();
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
        private void Reset()
        {
            GetData();
            txtInvoiceNo.Text = "";
            dtpInvoiceDate.Text = DateTime.Today.ToString();
            txtCustomerName.Text = "";
           // txtCustomerID.Text = "";
            txtFeatures.Text = "";
            txtSaleQty.Text = "";
           // txtCustomerID.Text = "";
            textBox1.Text = "";
            textBox3.Text = "0";
            richTextBox1.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            button4.Enabled = false;
            button3.Enabled = true;
           // Button1.Enabled = true;
            button2.Enabled = true;
            button4.Enabled = false;
            textBox3.Visible = false;
            richTextBox1.Visible = false;
            label13.Visible = false;
            label6.Visible = false;
            textBox4.Visible = false;
            label9.Visible = false;
            textBox1.Visible = true;
            txtFeatures.Visible = true;
            label11.Visible = true;
            label8.Visible = true;
            button6.Enabled = false;
            button5.Enabled = true;
            txtSaleQty.Visible = false;
            textBox1.Visible = false;
            txtFeatures.Visible = false;
            button4.Enabled = false;
            textBox3.Visible = false;
            richTextBox1.Visible = false;
            label13.Visible = false;
            label6.Visible = false;
            textBox4.Visible = false;
            label9.Visible = false;
                

        }
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {


                if (txtCustomerName.Text == "")
                {
                    MessageBox.Show("Please retrieve User Name Details", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    return;
                }

                if (txtSaleQty.Text == "")
                {
                    MessageBox.Show("Please enter Expense Amount", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtSaleQty.Focus();
                    return;
                }
                 if (txtFeatures.Text == "")
                {
                    MessageBox.Show("Please enter Expense Details", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFeatures.Focus();
                    return;
                }

                 Decimal val1 = 0;
                 Decimal val2 = 0;
                 Decimal.TryParse(txtSaleQty.Text, out val1);
                 Decimal.TryParse(textBox2.Text, out val2);
                 if (val1 > val2)
                 {
                     MessageBox.Show("Sorry ! No Balance", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                     return;
                 }

                 auto();

                 con = new SqlConnection(cs.DBConn);
                 con.Open();

                 string cb = "insert Into Expense(ExpenseNo,ExpenseDate,UserName,ExpencseDetail,Amount,ReturnAmount,RADetails) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";

                 cmd = new SqlCommand(cb);
                
                 cmd.Connection = con;
                 cmd.Parameters.AddWithValue("@d1", txtInvoiceNo.Text);
                 cmd.Parameters.AddWithValue("@d2", dtpInvoiceDate.Text);
                 cmd.Parameters.AddWithValue("@d3", txtCustomerName.Text);
                 cmd.Parameters.AddWithValue("@d4", txtFeatures.Text);
                 cmd.Parameters.AddWithValue("@d5", txtSaleQty.Text);
                 cmd.Parameters.AddWithValue("@d6", textBox3.Text);
                 cmd.Parameters.AddWithValue("@d7", richTextBox1.Text);

                 cmd.ExecuteReader();
                // MessageBox.Show("Successfully saved", "Expense Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 
                 if (con.State == ConnectionState.Open)
                 {
                     con.Close();
                 }

                 con.Close();
                {
                    con = new SqlConnection(cs.DBConn);
                    con.Open();

                    string cb1 = "update Balance set BalanceAmount = " + textBox1.Text + " where id='1'";
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }


             
                MessageBox.Show("Successfully Placed", "Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            Decimal val1 = 0;
            Decimal val2 = 0;
            Decimal.TryParse(textBox3.Text, out val1);
            Decimal.TryParse(textBox2.Text, out val2);
            Decimal I = (val1 + val2);
            textBox4.Text = I.ToString();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            frmUserRecord1 frm = new frmUserRecord1();
            frm.Show();
           
        }

       

        

        private void button7_Click(object sender, EventArgs e)
        {
            Reset();
           
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

                if (textBox3.Text == "")
                {
                    MessageBox.Show("Please enter returen amount", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //Button1.Focus();
                    return;
                }

                if (richTextBox1.Text == "")
                {
                    MessageBox.Show("Please enter returen amount details Details", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtFeatures.Focus();
                    return;
                }

                try
                      {

                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cb = "Update Expense set ReturnAmount=@d2,RADetails=@d3  where ExpenseNo=@d1";

                cmd = new SqlCommand(cb);

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1", txtInvoiceNo.Text);
                cmd.Parameters.AddWithValue("@d2", textBox3.Text);
                cmd.Parameters.AddWithValue("@d3", richTextBox1.Text);
                cmd.ExecuteReader();
                con.Close();
                {
                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string cb1 = "update Balance set BalanceAmount = " + textBox4.Text + " where id='1'";
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }



                MessageBox.Show("Successfully Placed", "Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            GetData();
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (txtCustomerName.Text == "")
            {
                MessageBox.Show("Please enter User Details", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (textBox5.Text == "")
            {
                MessageBox.Show("Please enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox5.Focus();
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
                uName.Value = txtCustomerName.Text;
                uPassword.Value = textBox5.Text;
                myCommand.Parameters.Add(uName);
                myCommand.Parameters.Add(uPassword);

                myCommand.Connection.Open();

                SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                if (myReader.Read() == true)
                {

                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string ct = "select usertype from Registration where Username='" + txtCustomerName.Text + "' and Password='" + textBox5.Text + "'";
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

                    if (txtUserType.Text.Trim() == "Cashier")
                    {
                        MessageBox.Show("Sorry You Don not have Permission !", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);


                        textBox5.Clear();
                        textBox5.Focus();
                    }
                    if (txtUserType.Text.Trim() == "Admin")
                    {
                        //GetData1();
                        txtSaleQty.Visible = true;
                        textBox1.Visible = true;
                        txtFeatures.Visible = true;
                        button5.Enabled = false;
                        //textBox7.Visible = true;
                        //textBox8.Visible = true;
                        //textBox9.Visible = true;
                        //textBox10.Visible = true;
                        //textBox11.Visible = true;
                        //textBox12.Visible = true;
                        //textBox13.Visible = true;
                        button2.Enabled = false;
                        //Button1.Enabled = false;
                    }

                }


                else
                {
                    MessageBox.Show("Login is Failed...Try again !", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    textBox5.Clear();
                    textBox5.Focus();

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

        private void button6_Click(object sender, EventArgs e)
        {

            if (txtCustomerName.Text == "")
            {
                MessageBox.Show("Please enter User Details", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox5.Text == "")
            {
                MessageBox.Show("Please enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox5.Focus();
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
                uName.Value = txtCustomerName.Text;
                uPassword.Value = textBox5.Text;
                myCommand.Parameters.Add(uName);
                myCommand.Parameters.Add(uPassword);

                myCommand.Connection.Open();

                SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                if (myReader.Read() == true)
                {

                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string ct = "select usertype from Registration where Username='" + txtCustomerName.Text + "' and Password='" + textBox5.Text + "'";
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

                    if (txtUserType.Text.Trim() == "Cashier")
                    {
                        MessageBox.Show("Sorry You Don not have Permission !", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);


                        textBox5.Clear();
                        textBox5.Focus();
                    }
                    if (txtUserType.Text.Trim() == "Admin")
                    {
                        button3.Enabled = false;
                        //Button1.Enabled = false;
                        button4.Enabled = true;
                        textBox3.Visible = true;
                        txtSaleQty.Visible = true;
                        richTextBox1.Visible = true;
                        label13.Visible = true;
                        label6.Visible = true;
                        textBox4.Visible = true;
                        label9.Visible = true;
                        textBox1.Visible = false;
                        txtFeatures.Visible = false;
                        label11.Visible = false;
                        label8.Visible = false;
                        button6.Enabled = false;
                        button2.Enabled = false;
                    }

                }


                else
                {
                    MessageBox.Show("Login is Failed...Try again !", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    textBox5.Clear();
                    textBox5.Focus();

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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }
        }
    }


