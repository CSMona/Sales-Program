using SalesWithLinq.Class;
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
    public partial class frm_ProductList : frm_Master
    {
        public frm_ProductList()
        {
            InitializeComponent();
        }

        private void frm_ProductList_Load(object sender, EventArgs e)
        {
            gridView1.OptionsBehavior.Editable = false;
            btn_Save.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            btn_Delete.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            gridView1.DoubleClick += GridView1_DoubleClick;
            RefreshDate();
            gridView1.CustomColumnDisplayText += GridView1_CustomColumnDisplayText;
        }

        private void GridView1_DoubleClick(object sender, EventArgs e)
        {
            int id = 0;
            if (int.TryParse(gridView1.GetFocusedRowCellValue("ID").ToString(),out id)&&id>0)
            {
                var frm = new frm_Product(id);
                frm.ShowDialog();
                RefreshDate();
            }
        }

        private void GridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "Type")
            {
                e.DisplayText = Master.ProductTypesList.Single(x => x.ID == Convert.ToInt32(e.Value)).Name;
            }
        }

        public override void RefreshDate()
        {
            base.RefreshDate();
            using (var db = new DAL.dbDataContext())
            {
                var data = from pr in db.Products
                           join cg in db.ProductCategories on pr.CategoryID equals cg.ID
                           select new
                           {
                               pr.ID,
                               pr.Code,
                               pr.Name,
                               CategoryName = cg.Name,
                               pr.Description,
                               pr.IsActive,
                               pr.Type,
                               UOM = (from u in db.ProductUntits where u.ProductID==pr.ID
                                      join un in db.UnitNames on u.UnitID equals un.ID
                                      select new
                                      {
                                          unitName=un.Name,
                                          u.Flactor,
                                          u.SellPrice,
                                          u.BuyPrice,
                                          u.Barcode,
                                      }).ToList(),


                               //db.ProductUntits.Where(x => x.ProductID == pr.ID).Select(u=>new { 
                               //unitName=db.UnitNames.Single(un=>un.ID==u.UnitID).Name,
                               //u.Flactor,
                               //u.SellPrice,
                               //u.BuyPrice,
                               //u.Barcode,
                               //}).ToList(),
                           };
                gridControl1.DataSource = data;
            }

        }
    }
}