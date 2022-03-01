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
    public partial class frm_Drawer :frm_Master
    {
        DAL.Drawer drawer;
        public frm_Drawer()
        {
            InitializeComponent();
            New();
        }
        public frm_Drawer(int id)
        {
            InitializeComponent();
            LoadDrawer(id);
          
        }

        void LoadDrawer(int id)
        {
            using (var db=new DAL.dbDataContext())
            {
                drawer = db.Drawers.Single(x => x.ID == id);
                GetDate();
            }
        }
        public override void New()
        {
            drawer = new DAL.Drawer();
            base.New();
        }
        public override void GetDate()
        {
            txt_Name.Text = drawer.Name;
            base.GetDate();
           
        }

        public override void Save()
        {
            if (txt_Name.Text.Trim() == string.Empty)
            {
                txt_Name.ErrorText = "الرجاء ادخال اسم الخزنه";
                return;
            }
            var db = new DAL.dbDataContext();//new store
            DAL.Account account;
            if (drawer.ID ==0)
            {
                account = new DAL.Account();
                db.Drawers.InsertOnSubmit(drawer);
                db.Accounts.InsertOnSubmit(account);
               
            }
           
            else//store aready exist in database
            {
                db.Drawers.Attach(drawer);

                account = db.Accounts.Single(s => s.ID == drawer.AccountID);
            }

            SetDate();
            account.Name = drawer.Name;
            db.SubmitChanges();

            drawer.AccountID = account.ID;
            base.Save();
          
        }

        public override void SetDate()
        {
            drawer.Name = txt_Name.Text;
            base.SetDate();
        }
    }
}
