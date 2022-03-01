using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
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
    public partial class frm_CustomerVendorList : frm_Master
    {
        bool IsCustomer;
        public frm_CustomerVendorList(bool isCustomer)
        {
            InitializeComponent();
            IsCustomer = isCustomer;
            RefreshDate();
            this.Text = IsCustomer ? "قائمه العملاء":"قائمه الموردين";

        }
        public override void RefreshDate()
        {
            using (var db = new DAL.dbDataContext())
            {
                gridControl1.DataSource = db.CustomersAndVendors.Where(x=>x.IsCustomer==IsCustomer).ToList();
            }
            base.RefreshDate();
        }

        private void frm_CustomerVendorList_Load(object sender, EventArgs e)
        {
            var ins = new DAL.CustomersAndVendor();
            gridView1.Columns[nameof(ins.ID)].Visible = false;
            gridView1.Columns[nameof(ins.AccountID)].Visible = false;
            gridView1.Columns[nameof(ins.IsCustomer)].Visible = false;
            gridView1.Columns[nameof(ins.Name)].Caption = "الاسم";
            gridView1.Columns[nameof(ins.Phone)].Caption = "الهاتف";
            gridView1.Columns[nameof(ins.Mobile)].Caption = "الموبيل";
            gridView1.Columns[nameof(ins.Address)].Caption = "العنوان";
            gridView1.OptionsBehavior.Editable = false;
            gridView1.DoubleClick += GridView1_DoubleClick;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                var frm = new frm_CustomerVendor(Convert.ToInt32(view.GetFocusedRowCellValue("ID")));
                frm.ShowDialog();
                RefreshDate();
            }
        }
    }
}
