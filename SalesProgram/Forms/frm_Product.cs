using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraEditors.Repository;
namespace SalesWithLinq.Forms
{
    public partial class frm_Product : frm_Master
    {
        DAL.Product product;

        DAL.dbDataContext sdb = new DAL.dbDataContext();
        RepositoryItemLookUpEdit reoUOM = new RepositoryItemLookUpEdit();
        public frm_Product()
        {
            InitializeComponent();
            RefreshDate();
            New();
        }

        public override void New()
        {
            product = new DAL.Product();
            base.New();
        }
        public override void GetDate()
        {
            txt_Code.Text = product.Code;
            txt_Name.Text = product.Name;
            lkp_Category.EditValue = product.CategoryID;
            lkp_Type.EditValue = product.Type;
            memoEdit1.Text = product.Description;
            checkEdit1.Checked = product.IsActive;

            gridControl1.DataSource = sdb.ProductUntits.Where(x => x.ProductID == product.ID);
            base.GetDate();
        }
        public override void SetDate()
        {
            product.CategoryID = Convert.ToInt32(lkp_Category.EditValue);
            product.Code = txt_Code.Text;
            product.Name = txt_Name.Text;
            product.Type = Convert.ToByte(lkp_Type.EditValue);
            product.IsActive = checkEdit1.Checked;
            product.Image = GetByteFromImage(pictureEdit1.Image);
            base.SetDate();
        }
        bool ValidiateDate()
        {
            if (lkp_Category.EditValue is int == false)
            {
                lkp_Category.ErrorText = ErrorText;
                return false;
            }
            if (lkp_Type.EditValue is byte == false)
            {
                lkp_Type.ErrorText = ErrorText;
                return false;
            }
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = ErrorText;
                return false;

            }
            if (txt_Code.Text.Trim() == string.Empty)
            {
                txt_Code.ErrorText = ErrorText;
                return false;
            }
            var db = new DAL.dbDataContext();
            if (db.Products.Where(X => X.ID != product.ID && X.Name.Trim() == txt_Name.Text.Trim()).Count() > 0)
            {
                txt_Name.ErrorText = "هذا الاسم مسجل مسبقا";
                return false;
            }

            if (db.Products.Where(X => X.ID != product.ID && X.Code.Trim() == txt_Code.Text.Trim()).Count() > 0)
            {
                txt_Code.ErrorText = "هذا الكود مسجل مسبقا";
                return false;
            }

            return true;
        }
        public override void Save()
        {
            if (ValidiateDate() == false)
                return;
            var db = new DAL.dbDataContext();
            if (product.ID == 0)
            {
                db.Products.InsertOnSubmit(product);
            }
            else
            {
                db.Products.Attach(product);
            }
            SetDate();
            db.SubmitChanges();
            var data = gridView1.DataSource as BindingList<DAL.ProductUntit>;
            foreach (var item in data)
            {
                item.ProductID = product.ID;
                if (string.IsNullOrEmpty(item.Barcode))
                    item.Barcode = "";
            }
            sdb.SubmitChanges();
            base.Save();
        }

        Byte[] GetByteFromImage(Image image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    image.Save(stream, ImageFormat.Jpeg);
                    return stream.ToArray();
                }
              catch
                {
                    return stream.ToArray();
                }
            }
        }
        public override void RefreshDate()
        {
            using (var db = new DAL.dbDataContext())
            {
                lkp_Category.Properties.DataSource = db.ProductCategories
                    .Where(x => db.ProductCategories.Where(w => w.ProductID == x.ID).Count() == 0).ToList();
                reoUOM.DataSource = db.UnitNames.ToList();
            }
            base.RefreshDate();
        }
        private void frm_Product_Load(object sender, EventArgs e)
        {
            lkp_Category.Properties.DisplayMember = "Name";
            lkp_Category.Properties.ValueMember = "ID";
            lkp_Category.ProcessNewValue += Lkp_Category_ProcessNewValue;
            lkp_Category.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            lkp_Type.Properties.DataSource = new List<ValueAndID>() { new ValueAndID() { ID = 0, Name = "مخزني" },
             new ValueAndID() { ID=1,Name="خدمي"}
            };
            lkp_Type.Properties.DisplayMember = "Name";
            lkp_Type.Properties.ValueMember = "ID";
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            var ins = new DAL.ProductUntit();
            gridView1.Columns[nameof(ins.ID)].Visible=false;
            gridView1.Columns[nameof(ins.ProductID)].Visible = false;
            RepositoryItemCalcEdit calcEdit=new RepositoryItemCalcEdit();
          

            gridControl1.RepositoryItems.Add(calcEdit);
            gridControl1.RepositoryItems.Add(reoUOM);

            gridView1.Columns[nameof(ins.SellPrice)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(ins.BuyPrice)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(ins.SellDiscount)].ColumnEdit = calcEdit;
            gridView1.Columns[nameof(ins.Flactor)].ColumnEdit = calcEdit;

            gridView1.Columns[nameof(ins.UnitID)].ColumnEdit = reoUOM;

            reoUOM.ValueMember = "ID";
            reoUOM.DisplayMember = "Name";
            reoUOM.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            reoUOM.ProcessNewValue += ReoUOM_ProcessNewValue;
        }

        private void ReoUOM_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
           if(e.DisplayValue is string value &&value.Trim()!=string.Empty)
            {
                var newObject = new DAL.UnitName() { Name = value.Trim() };
                using (DAL.dbDataContext db =new DAL.dbDataContext())
                {
                    db.UnitNames.InsertOnSubmit(newObject);
                    db.SubmitChanges();
                }
                ((List<DAL.UnitName>)reoUOM.DataSource).Add(newObject);
                e.Handled = true;
            }
        }

        public enum productType
        {
            Inventory,
            Service
        }

        class ValueAndID
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        private void Lkp_Category_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if (e.DisplayValue is string st &&st.Trim()!=string.Empty)
            {
                var newObject = new DAL.ProductCategory() { Name=st,ProductID=0,Number="0"};
                using(var db=new DAL.dbDataContext())
                {
                    db.ProductCategories.InsertOnSubmit(newObject);
                    db.SubmitChanges();
                }
                ((List<DAL.ProductCategory>)lkp_Category.Properties.DataSource).Add(newObject);
                e.Handled = true;
                
            }
        }
    }
}
