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
using DevExpress.XtraGrid.Views.Grid;
using static SalesWithLinq.Class.Master;

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

        public frm_Product(int id)
        {
            InitializeComponent();
            RefreshDate();
            LoadProduct(id);
        }
        void LoadProduct(int id)
        {
            using (var db=new DAL.dbDataContext())
            {
                product = db.Products.Single(x => x.ID == id);
            }
            GetDate();
        }
        public override void New()
        {
            product = new DAL.Product()
            {
                Code = GetNewProductCode()
            };
            
            base.New();
            var data = gridView1.DataSource as BindingList<DAL.ProductUntit>;
            var db = new DAL.dbDataContext();
            if (db.UnitNames.Count()==0)
            {
                db.UnitNames.InsertOnSubmit(new DAL.UnitName() { Name = "قطعه" });
                db.SubmitChanges();
                RefreshDate();
            }
            data.Add(new DAL.ProductUntit() { Flactor = 1, UnitID = db.UnitNames.First().ID ,Barcode=GetNewBarCode()});
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
        DAL.ProductUntit ins = new DAL.ProductUntit();
        private void frm_Product_Load(object sender, EventArgs e)
        {
            lkp_Category.Properties.DisplayMember = "Name";
            lkp_Category.Properties.ValueMember = "ID";
            lkp_Category.ProcessNewValue += Lkp_Category_ProcessNewValue;
            lkp_Category.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            lkp_Type.Properties.DataSource = ProductTypesList;
            
            lkp_Type.Properties.DisplayMember = "Name";
            lkp_Type.Properties.ValueMember = "ID";
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Top;
            
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
            
            
            gridView1.Columns[nameof(ins.Barcode)].Caption="الباركود";
            gridView1.Columns[nameof(ins.BuyPrice)].Caption = "سعر الشراء";
            gridView1.Columns[nameof(ins.Flactor)].Caption = "معامل التحويل";
            gridView1.Columns[nameof(ins.SellDiscount)].Caption = "خصم البيع";
            gridView1.Columns[nameof(ins.SellPrice)].Caption = "سعر البيع";
            gridView1.Columns[nameof(ins.UnitID)].Caption = "اسم الوحده";


            reoUOM.ValueMember = "ID";
            reoUOM.DisplayMember = "Name";
            reoUOM.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            reoUOM.ProcessNewValue += ReoUOM_ProcessNewValue;

            gridView1.ValidateRow += GridView1_ValidateRow;
            gridView1.InvalidRowException += GridView1_InvalidRowException;
            gridView1.FocusedRowChanged += GridView1_FocusedRowChanged;

        }

        private void GridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            
                gridView1.Columns[nameof(ins.Flactor)].OptionsColumn.AllowEdit = !(e.FocusedRowHandle==0);
            
        }

        private void GridView1_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
        }

        private void GridView1_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            var row = e.Row as DAL.ProductUntit;
            var view = sender as GridView;
            if (row == null)
            {
                return;
            }
            if (row.Flactor <= 1 && e.RowHandle>0)
            {
                e.Valid = false;
                gridView1.SetColumnError(view.Columns[nameof(row.Flactor)], "يجب ان تكون القيمه اكبر من 1");
            }
            if (row.UnitID <= 0 )
            {
                e.Valid = false;
                gridView1.SetColumnError(view.Columns[nameof(row.Flactor)], ErrorText);
            }
            if (CheckIfBarCodeExist(row.Barcode, proID: product.ID))
            {
                e.Valid = false;
                gridView1.SetColumnError(view.Columns[nameof(row.Barcode)], "هذا الكود موجود بالفعل");

            }
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

        string GetNewProductCode()
        {
            string maxCode;
            using (var db = new DAL.dbDataContext())
            {
                maxCode = db.Products.Select(x => x.Code).Max();
            }

            return GetNextNumberInString(maxCode);
        }

        string GetNewBarCode()
        {
            string maxCode;
            using (var db = new DAL.dbDataContext())
            {
                maxCode = db.ProductUntits.Select(x => x.Barcode).Max();
            }

            return GetNextNumberInString(maxCode);
        }

        string GetNextNumberInString(string Number)
        {
            if (Number == string.Empty || Number == null)
            {
                return "1";
            }
            string str1 = "";
            foreach (Char c in Number)
            {
                str1 = char.IsDigit(c) ? str1 + c.ToString():"";
            }
            if (str1 == string.Empty)
            {
                return Number + "1";
            }
            string str2 = str1.Insert(0, "1");
            str2 = (Convert.ToInt32(str2) + 1).ToString();
            string str3 = str2[0] == '1' ? str2.Remove(0, 1) : str2.Remove(0, 1).Insert(0, "1");

            int index = Number.LastIndexOf(str1);
            Number= Number.Remove(index);
            Number = Number.Insert(index, str3);
            return Number;
        }
        Boolean CheckIfBarCodeExist(string barcode, int proID)
        {
            using (var db=new DAL.dbDataContext())
            {
                return  db.ProductUntits.Where(x => x.Barcode == barcode&&x.ProductID!=proID).Count()>0;
            }
        }

    }
}
