using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmDemand : Form
    {
        SqlDataReader rdr = null;
        DataTable dtable = new DataTable();
        SqlConnection con = null;
        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        ConnectionString cs = new ConnectionString();
        public frmDemand()
        {
            InitializeComponent();
        }

        private void frmDemand_Load(object sender, EventArgs e)
        {
            Autocomplete();
        }
        private void Reset()
        {
            productName.Text = "";
            customerName.Text = "";
            Address.Text = "";
            mobileNumber.Text = "";
            quantity.Text = "";
            advance.Text = "";
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
           // txtCategoryName.Focus();
        }

        private void auto()
        {
            txtProductID.Text =  GetUniqueKey(6);
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
        private void btnNew_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
        }

        private void Autocomplete()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT distinct Demanditem FROM Demand", con);
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Demand");
                AutoCompleteStringCollection col = new AutoCompleteStringCollection();
                int i = 0;
                for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    col.Add(ds.Tables[0].Rows[i]["Demanditem"].ToString());

                }
               // txtCategoryName.AutoCompleteSource = AutoCompleteSource.CustomSource;
              //  txtCategoryName.AutoCompleteCustomSource = col;
               // txtCategoryName.AutoCompleteMode = AutoCompleteMode.Suggest;

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           
        }

      

        

        private void btnGetData_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDemandRecord frm = new frmDemandRecord();
            frm.Show();
            frm.GetData();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void frmDemand_Load_1(object sender, EventArgs e)
        {

        }

        private void Label11_Click(object sender, EventArgs e)
        {

        }

        private void txtContactNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void GroupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Label19_Click(object sender, EventArgs e)
        {

        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Label5_Click_1(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void txtSupplierName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSupplierID_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click_1(object sender, EventArgs e)
        {
        
        }

        private void label7_Click(object sender, EventArgs e)
        {
        
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (productName.Text == "")
            {
                MessageBox.Show("Please enter Product Name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                productName.Focus();
                return;
            }

            if (quantity.Text == "")
            {
                MessageBox.Show("Please enter Quanatity", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                productName.Focus();
                return;
            }

            

            try
            {
               auto();

                con = new SqlConnection(cs.DBConn);
                con.Open();

                string cb = "insert into demand_item(ID,Demanditem,mobilenumber,advance,customername,quantity,date,address) VALUES (@d1,@d2,@d3,@d4,@d5,@d6,@d7,@d8)";
                cmd = new SqlCommand(cb);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1", txtProductID.Text);
                cmd.Parameters.AddWithValue("@d2", productName.Text);
                cmd.Parameters.AddWithValue("@d3", mobileNumber.Text);
                cmd.Parameters.AddWithValue("@d4", advance.Text);
                cmd.Parameters.AddWithValue("@d5", customerName.Text);
                cmd.Parameters.AddWithValue("@d6", quantity.Text);
                cmd.Parameters.AddWithValue("@d7", date.Text);
                cmd.Parameters.AddWithValue("@d8", Address.Text);
               // cmd.Parameters.AddWithValue("@d8", date.Text);

                cmd.ExecuteReader();
                MessageBox.Show("Successfully saved Product Demand detail", "Product Demand order Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGetData_Click_1(object sender, EventArgs e)
        {

            this.Hide();
            frmDemandRecord frm = new frmDemandRecord();
            frm.Show();
            frm.GetData();
        }

        private void delete_records()
        {

            try
            {

                int RowsAffected = 0;
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cq = "delete from demand_item where ID='" + txtProductID.Text + "'";
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
                    MessageBox.Show("No record found", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            try
            {


                if (MessageBox.Show("Do you really want to delete the record?", "Demand Items Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    delete_records();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click_1(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (productName.Text == "")
                {
                    MessageBox.Show("Please enter product name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    productName.Focus();
                    return;
                }

                if (quantity.Text == "")
                {
                    MessageBox.Show("Please enter address", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    quantity.Focus();
                    return;
                }
               
                con = new SqlConnection(cs.DBConn);
                con.Open();
                string cb = "update demand_item set Demanditem=@d2, advance=@d3, customername=@d4,quantity=@d5,date=@d6, address=@d7, mobilenumber=@d8  where ID=@d1";

                cmd = new SqlCommand(cb);

                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@d1", txtProductID.Text);
                cmd.Parameters.AddWithValue("@d2", productName.Text);
                cmd.Parameters.AddWithValue("@d3", advance.Text);
                cmd.Parameters.AddWithValue("@d4", customerName.Text);
                cmd.Parameters.AddWithValue("@d5", quantity.Text);
                cmd.Parameters.AddWithValue("@d6", date.Text);
                cmd.Parameters.AddWithValue("@d7", Address.Text);
                cmd.Parameters.AddWithValue("@d8", mobileNumber.Text);
                cmd.ExecuteReader();
                MessageBox.Show("Successfully updated", "Supplier Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnUpdate.Enabled = false;
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // Update Function 
    }
}
