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
    public partial class frm_ProductCategory : frm_Master
    {
        DAL.ProductCategory category;
        public frm_ProductCategory()
        {
            InitializeComponent();
            New();
        }

        public override void New()
        {
            category = new DAL.ProductCategory();
            base.New();
        }
        public override void GetDate()
        {
            textEdit1.Text = category.Name;
            lookUpEdit1.EditValue = category.ProductID;
            base.GetDate();
        }
        public override void SetDate()
        {
            category.Name = textEdit1.Text;
            category.ProductID = (lookUpEdit1.EditValue as int?) ?? 0;
            category.Number = "0";
            base.SetDate();
        }
        bool IsDataValid()
        {
            if (textEdit1.Text.Trim() == string.Empty)
            {
                textEdit1.ErrorText = "هذا الحقل مطلوب";
                return false;
            }
            var db = new DAL.dbDataContext();
            if (db.ProductCategories.Where(x => x.Name.Trim() == textEdit1.Text.Trim() && x.ID != category.ID).Count() > 0)
            {
                textEdit1.ErrorText = "هذا الحقل مسجل مسبقا";
                return false;
            }
            return true;
        }
        public override void Save()
        {
            if (IsDataValid() == false)
            {
                return;
            }
            var db = new DAL.dbDataContext();
           
            if (category.ID == 0)
            {
                db.ProductCategories.InsertOnSubmit(category);
            }
            else
            {
                db.ProductCategories.Attach(category);
            }
            SetDate();
            db.SubmitChanges();
            base.Save();
        }
        private void frm_ProductCategory_Load(object sender, EventArgs e)
        {
            RefreshDate();
            lookUpEdit1.Properties.DisplayMember = nameof(category.Name) ;
            lookUpEdit1.Properties.ValueMember = nameof(category.ID);
            
            treeList1.ParentFieldName = nameof(category.ProductID) ;
            treeList1.KeyFieldName = nameof(category.ID);
            treeList1.OptionsBehavior.Editable = false;
            treeList1.Columns[nameof(category.Number)].Visible = false;
            treeList1.Columns[nameof(category.Name)].Caption = "الاسم";
            treeList1.FocusedNodeChanged += TreeList1_FocusedNodeChanged;

        }

        private void TreeList1_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            int id = 0;
            if (int.TryParse(e.Node.GetValue("ID").ToString(),out id))
            {
                var db = new DAL.dbDataContext();
                category = db.ProductCategories.Single(x => x.ID == id);
                GetDate();
            }
         
        }

        public override void RefreshDate()
        {
            var db = new DAL.dbDataContext();
            var groups = db.ProductCategories;
            lookUpEdit1.Properties.DataSource = groups;
            treeList1.DataSource = groups;
            treeList1.ExpandAll();
            base.RefreshDate();
        }
    }
}
