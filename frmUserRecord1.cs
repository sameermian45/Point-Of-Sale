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

    public partial class frmUserRecord1 : Form
    {
        SqlDataReader rdr = null;
        SqlConnection con = null;
        SqlCommand cmd = null;
        ConnectionString cs = new ConnectionString();
        public frmUserRecord1()
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
                frmExpences frm = new frmExpences();
                frm.Show();
                frm.txtInvoiceNo.Text = dr.Cells[0].Value.ToString();
                frm.dtpInvoiceDate.Text = dr.Cells[1].Value.ToString();
                frm.txtCustomerName.Text = dr.Cells[2].Value.ToString();
                frm.txtSaleQty.Text = dr.Cells[4].Value.ToString();
                frm.button5.Enabled = false;
                frm.button6.Enabled = true;
                frm.txtSaleQty.Visible = true;
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

                // GroupBox3.Visible = true;
                con = new SqlConnection(cs.DBConn);
                con.Open();

                cmd = new SqlCommand("SELECT RTRIM(ExpenseNo) as [Expense ID],RTRIM(ExpenseDate) as [Expense Date],RTRIM(UserName) as [User Name],RTRIM(ExpencseDetail) as [Expense Details],RTRIM(Amount) as [Expense Amount] from Expense where ExpenseDate between @d1 and @d2 order by ExpenseDate desc", con);
                cmd.Parameters.Add("@d1", SqlDbType.DateTime, 30, "ExpenseDate").Value = dtpInvoiceDateFrom.Value.Date;
                cmd.Parameters.Add("@d2", SqlDbType.DateTime, 30, "ExpenseDate").Value = dtpInvoiceDateTo.Value.Date;
                SqlDataAdapter myDA = new SqlDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Expense");
                dataGridView1.DataSource = myDataSet.Tables["Expense"].DefaultView;


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
            frmpersonalUse frm = new frmpersonalUse();
            frm.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
        }
    }
}
