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
    public partial class frm_Master : DevExpress.XtraEditors.XtraForm
    {
        
        public static string ErrorText { get {
                return "هذا الحقل مطلوب"; 
            } }
        public frm_Master()
        {
            InitializeComponent();
        }
        public virtual void Save()
        {
            XtraMessageBox.Show("تم الحفظ بنجاح");
            RefreshDate();
        }
        public virtual void New()
        {
            GetDate();
        }

        public virtual void Delete()
        {

        }

        public virtual void GetDate()
        {
            
        }
        public virtual void SetDate()
        {

        }
        public virtual void RefreshDate()
        {

        }
        public virtual bool IsDataValide()
        {
            return true;
        }

        private void btn_Save_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(IsDataValide())
                Save();
           
        }

        private void btn_New_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            New();
        }

        private void btn_Delete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Delete();
        }

        private void frm_Master_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btn_Save.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
               
                New();
            }
            if (e.KeyCode == Keys.F3)
            {

                Delete();
            }
        }
    }
}
