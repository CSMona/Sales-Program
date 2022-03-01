using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesWithLinq.Forms
{
    public partial class frm_StoresList : XtraForm
    {
        public frm_StoresList()
        {
            InitializeComponent();
        }

        private void frm_StoresList_Load(object sender, EventArgs e)
        {
            RefreshDate();

            gridView1.OptionsBehavior.Editable = false;
            gridView1.Columns["ID"].Visible = false;
            gridView1.Columns["Name"].Caption = "الاسم";
            gridView1.DoubleClick += GridView1_DoubleClick;

        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            int id = 0;
            id = Convert.ToInt32(gridView1.GetFocusedRowCellValue("ID"));
            frm_Stores frm = new frm_Stores(id);


            frm.ShowDialog();
            RefreshDate();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            RefreshDate();
        }

        void RefreshDate()
        {
            var db = new DAL.dbDataContext();
           gridControl1.DataSource= db.Stores;
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm_Stores frm = new frm_Stores();
            frm.ShowDialog();
            RefreshDate();
        }
    }
    }

