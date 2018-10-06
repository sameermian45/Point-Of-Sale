using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sales_and_Inventory_System__Gadgets_Shop_
{

    public partial class frmDemandRecord : Form
    {
        SqlDataReader rdr = null;
        SqlConnection con = null;
        SqlCommand cmd = null;
        ConnectionString cs = new ConnectionString();
        public frmDemandRecord()
        {
            InitializeComponent();
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
                frmDemand frm = new frmDemand();
                frm.Show();
                frm.txtProductID.Text = dr.Cells[0].Value.ToString();
                frm.productName.Text = dr.Cells[1].Value.ToString();
                frm.customerName.Text = dr.Cells[2].Value.ToString();
                frm.Address.Text = dr.Cells[3].Value.ToString();
                frm.mobileNumber.Text = dr.Cells[4].Value.ToString(); 
                frm.advance.Text = dr.Cells[5].Value.ToString();
                frm.quantity.Text = dr.Cells[6].Value.ToString();
                frm.date.Text = dr.Cells[7].Value.ToString();

                frm.btnUpdate.Enabled = true;
                frm.btnDelete.Enabled = true;
                frm.btnSave.Enabled = false;
                frm.customerName.Focus();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void GetData()
        {
            try
            {
                con = new SqlConnection(cs.DBConn);
                con.Open();
                cmd = new SqlCommand("SELECT RTRIM(ID)as [ID],RTRIM(Demanditem) as [Demand Item],RTRIM(customername) as [Customer Name],RTRIM(address) as [Address],RTRIM(mobilenumber) as [Mobile Number],RTRIM(advance) as [Advance],RTRIM(quantity) as [Quantity],RTRIM(date) as [Date]  from demand_item order by Demanditem", con);
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "demand_item");
                dataGridView1.DataSource = myDataSet.Tables["demand_item"].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmDemandRecord_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void frmDemandRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmDemand frm = new frmDemand();
            frm.Show();
        }
    }
}
