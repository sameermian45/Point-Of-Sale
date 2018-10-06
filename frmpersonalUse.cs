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
    public partial class frmpersonalUse : Form
    {
        SqlCommand cmd;
        SqlConnection con;
        SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public frmpersonalUse()
        {
            InitializeComponent();
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
        private void Reset()
        {
            GetData();
            cmbSupplierName.Text = "";
            dtpInvoiceDate.Text = DateTime.Today.ToString();
            txtFeatures.Text = "";
            txtSaleQty.Text = "";
            textBox1.Text = "";
            button4.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            textBox1.Visible = true;
            txtFeatures.Visible = true;
            label11.Visible = true;
            label8.Visible = true;
            txtSaleQty.Visible = true;
            textBox1.Visible = true;
            txtFeatures.Visible = true;
            button4.Enabled = false;


        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        private void auto()
        {
            textBox6.Text = "BID-" + GetUniqueKey(4);
        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars = "123456789".ToCharArray();
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
        

        private void frmpersonalUse_Load(object sender, EventArgs e)
        {
            GetData();
            FillCombo();
            txtSaleQty.Visible = true;
            textBox1.Visible = true;
            txtFeatures.Visible = true;
            button4.Enabled = false;
            button1.Enabled = false;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {


                

                if (cmbSupplierName.Text == "")
                {
                    MessageBox.Show("Please enter Expense Amount", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cmbSupplierName.Focus();
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
                Decimal .TryParse(textBox2.Text, out val2);
                if (val1 > val2)
                {
                    MessageBox.Show("Sorry ! No Balance", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                auto();

                con = new SqlConnection(cs.DBConn);
                con.Open();

                string cb = "insert Into personalExpense(ExpenseNo,ExpenseDate,ExpencseDetail,Amount,SuplierName,Total,Remaining) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7)";

                cmd = new SqlCommand(cb);

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1", textBox6.Text);
                cmd.Parameters.AddWithValue("@d2", dtpInvoiceDate.Text);
                cmd.Parameters.AddWithValue("@d3", txtFeatures.Text);
                cmd.Parameters.AddWithValue("@d4", txtSaleQty.Text);
                cmd.Parameters.AddWithValue("@d5", cmbSupplierName.Text);
                cmd.Parameters.AddWithValue("@d6", textBox3.Text);
                cmd.Parameters.AddWithValue("@d7", textBox4.Text);

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

                    string cb1 = "update Balance set BalanceAmount  = " + textBox1.Text + " where id='1'";
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                {
                    con = new SqlConnection(cs.DBConn);
                    con.Open();

                    string cb1 = "update SaplierRemaing set Remaining  = " + textBox4.Text + " where SuplierName='" + cmbSupplierName.Text + "'";
                    cmd = new SqlCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                button1.Enabled = true;


                MessageBox.Show("Successfully Placed", "Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
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

            Decimal val3 = 0;
            Decimal val4 = 0;
            int val5 = 0;
            Decimal.TryParse(textBox3.Text, out val3);
            Decimal.TryParse(txtSaleQty.Text, out val4);
            Decimal s = (val3 - val4);
            textBox4.Text = s.ToString();

            if (Convert.ToInt32(textBox4.Text) <= 0)
            {
                textBox4.Text = val5.ToString();
            }
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSaleQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 46))
            {
                e.Handled = true;
                return;
            }
        }

        private void cmbSupplierName_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData1();
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
            
        }
        public void GetData1()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();

                String sql = "SELECT Remaining from SaplierRemaing where SuplierName='" + cmbSupplierName.Text + "' ";
                cmd = new SqlCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                textBox2.Clear();
                if (rdr.Read() == true)
                {
                    textBox3.Text = rdr["Remaining"].ToString();

                }
                con.Close();
                GetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                Cursor = Cursors.WaitCursor;
                CrystalReport1 rpt = new CrystalReport1();
                //The report you created.
                cmd = new SqlCommand();
                SqlDataAdapter myDA = new SqlDataAdapter();
                POS_DBDataSet myDS = new POS_DBDataSet();
                //The DataSet you created.
                con = new SqlConnection(cs.DBConn);
                cmd.Connection = con;
                //cmd.CommandText = "SELECT * from Invoice_Info where companyname='" + cmbCustomerName.Text.Trim() + "'";
                cmd.CommandText = "SELECT * from personalExpense where ExpenseNo='"+ textBox6.Text.Trim() +"'";
                cmd.CommandType = CommandType.Text;
                myDA.SelectCommand = cmd;
                myDA.Fill(myDS, "personalExpense");

                // myDA.Fill(myDS, "Customer");
                rpt.SetDataSource(myDS);
                frmSalesReport frm = new frmSalesReport();
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
