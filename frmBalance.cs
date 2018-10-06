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
    public partial class frmBalance : Form
    {

        ConnectionString cs = new ConnectionString();
        SqlDataReader rdr = null;
        SqlConnection con = null;
        SqlCommand cmd = null;
        DataTable dt = new DataTable();
       
        public frmBalance()
        {
            InitializeComponent();
        }

        private void Reset()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
                 
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please enter user name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Focus();
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Please enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox3.Focus();
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
                uName.Value = textBox2.Text;
                uPassword.Value = textBox3.Text;
                myCommand.Parameters.Add(uName);
                myCommand.Parameters.Add(uPassword);

                myCommand.Connection.Open();

                SqlDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);

                if (myReader.Read() == true)
                {

                    con = new SqlConnection(cs.DBConn);
                    con.Open();
                    string ct = "select usertype from Registration where Username='" + textBox2.Text + "' and Password='" + textBox3.Text + "'";
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

                        textBox2.Clear();
                        textBox3.Clear();
                        textBox2.Focus();
                    }
                    if (txtUserType.Text.Trim() == "Admin")
                    {


                        if (textBox1.Text == "")
                        {
                            MessageBox.Show("Please enter Balance.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            textBox1.Focus();
                            return;
                        }

                        
                        try
                        {
                            con = new SqlConnection(cs.DBConn);
                            con.Open();

                            string cb = "insert into BalanceHistory(Date,Amount,Name) VALUES (@d1,@d2,@d3)";

                            cmd = new SqlCommand(cb);

                            cmd.Connection = con;
                            
                            cmd.Parameters.AddWithValue("@d1", dtpInvoiceDate.Text);
                            cmd.Parameters.AddWithValue("@d2", textBox1.Text);
                            cmd.Parameters.AddWithValue("@d3", textBox2.Text);
                            cmd.ExecuteReader();
                            con.Close();

                            con = new SqlConnection(cs.DBConn);
                            con.Open();

                            string cb1 = "update Balance set BalanceAmount = BalanceAmount + " + textBox1.Text + " where id=1";

                            cmd = new SqlCommand(cb1);
                            cmd.Connection = con;
                            cmd.ExecuteReader();
                            MessageBox.Show("Successfully saved", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            con.Close();
                            Reset();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
        }
                       
                    }

                


                else
                {
                    MessageBox.Show("Login is Failed...Try again !", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    textBox2.Clear();
                    textBox3.Clear();
                    textBox2.Focus();

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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void frmBalance_Load(object sender, EventArgs e)
        {

        }

        private void frmBalance_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmMainMenu frm = new frmMainMenu();
            frm.Show();
        }
    }
}
