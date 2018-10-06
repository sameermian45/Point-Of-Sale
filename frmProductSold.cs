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
namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmProductSold : Form
    {
        DataTable dTable;
        SqlConnection con = null;
        SqlDataAdapter adp;
        DataSet ds;
        SqlCommand cmd = null;
        DataTable dt = new DataTable();
        SqlDataReader rdr;
        ConnectionString cs = new ConnectionString();
        public frmProductSold()
        {
            InitializeComponent();
        }

        private void frmProductSold_Load(object sender, EventArgs e)
        {

        }
    }
}
