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
    
    public partial class frm_CustomerVendor : frm_Master
    {
        bool IsCustomer;
        DAL.CustomersAndVendor CusVen;
        public frm_CustomerVendor(bool isCustomer)
        {
            InitializeComponent();
            IsCustomer = isCustomer;
            New();
        }

        private void frm_CustomerVendor_Load(object sender, EventArgs e)
        {
            this.Text = (IsCustomer) ? "عميل" : "مورد";
        }
        public override void New()
        {
            CusVen = new DAL.CustomersAndVendor();
            base.New();
        }
        public override void GetDate()
        {
            textEdit1.Text = CusVen.Name;
            textEdit2.Text = CusVen.Phone;
            textEdit3.Text = CusVen.Mobile;
          
            textEdit4.Text = CusVen.Address;
           textEdit5.Text = CusVen.AccountID.ToString();

            base.GetDate();
        }

        public override void SetDate()
        {
            CusVen.Name =  textEdit1.Text;
           
            CusVen.Phone  = textEdit2.Text;
            CusVen.Mobile = textEdit3.Text;
            CusVen.Address  = textEdit4.Text;
            CusVen.IsCustomer  = IsCustomer;   

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
            if (db.ProductCategories.Where(x=>x.Name.Trim()== textEdit1.Text.Trim()).Count()>0)
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
            DAL.Account account;
            if (CusVen.ID == 0)
            {
                db.CustomersAndVendors.InsertOnSubmit(CusVen);
                account = new DAL.Account();
                db.Accounts.InsertOnSubmit(account);

            }
               
            else
            {
                db.CustomersAndVendors.Attach(CusVen);
                account = db.Accounts.Single(s => s.ID == CusVen.AccountID);
            }
            SetDate();
            account.Name = CusVen.Name;
            db.SubmitChanges();
            CusVen.AccountID = account.ID;
            db.SubmitChanges();
            base.Save();
        }
    }
}
