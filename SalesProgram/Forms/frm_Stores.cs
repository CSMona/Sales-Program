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
    public partial class frm_Stores : DevExpress.XtraEditors.XtraForm
    {
        DAL.Store store;
        public frm_Stores()
        {
            InitializeComponent();
            New();
        }
        public frm_Stores(int id)
        {
            InitializeComponent();
            var db = new DAL.dbDataContext();
            store = db.Stores.Where(s=>s.ID==id).First();
        }
        void Save()
        {
            if (txt_NameStore.Text.Trim() == string.Empty)
            {
                txt_NameStore.ErrorText = "الرجاء ادخال اسم الفرع";
                return;
            }
            var db = new DAL.dbDataContext();//new store
            if (store.ID == 0)
                db.Stores.InsertOnSubmit(store);
            else//store aready exist in database
                db.Stores.Attach(store);

            SetDate();
            db.SubmitChanges();
            XtraMessageBox.Show("تم الحفظ بنجاح");
        }
        void GetDate()
        {
            txt_NameStore.Text = store.Name;
        }
        void SetDate()
        {
            store.Name  = txt_NameStore.Text;
        }
        void New()
        {
            store = new DAL.Store();
            GetDate();
        }

        private void btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Save();
        }

        private void btn_New_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            New();
        }

        private void btn_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            var db = new DAL.dbDataContext();
            if(XtraMessageBox.Show(text:"هل تريد حذف المخزن",caption:"تاكيد الحذف",buttons:MessageBoxButtons.YesNo,icon:MessageBoxIcon.Question)==DialogResult.Yes)
            {
                db.Stores.Attach(store);
                db.Stores.DeleteOnSubmit(store);
                db.SubmitChanges();
                XtraMessageBox.Show("تم الحذف بنجاح");
                New();
            }
          
        }
    }
}
