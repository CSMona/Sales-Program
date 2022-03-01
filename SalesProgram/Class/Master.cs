using DevExpress.XtraEditors;
using SalesWithLinq.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesWithLinq.Class
{
  public static  class Master
    {
       public class ValueAndID
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
        public static List<ValueAndID> ProductTypesList = new List<ValueAndID>() {
                new ValueAndID() { ID = 0, Name = "مخزني" },
             new ValueAndID() { ID = 1, Name = "خدمي" }
        };
        public enum productType
        {
            Inventory,
            Service
        }
        public static List<ValueAndID> PartTypesList = new List<ValueAndID>() {
                new ValueAndID() { ID = 0, Name = "مورد" },
                new ValueAndID() { ID = 1, Name = "عميل" }
        };
        public enum PartType
        {
            Vendor,
            Customer
        }
        public static bool IsTextValide(this TextEdit txt)
        {
            if (txt.Text.Trim() == string.Empty)
            {
                txt.ErrorText = frm_Master.ErrorText;
                return false;
            }
            return true;
        }

        public static bool IsEditValueValide(this LookUpEditBase lkp)
        {
            if (lkp.Text.Trim() == string.Empty)
            {
                lkp.ErrorText = frm_Master.ErrorText;
                return false;
            }
            return true;
        }
        public static bool IsEditValueValideAndNotZero(this LookUpEditBase lkp)
        {
            if (lkp.IsEditValueIntOfTypeInt()==false||Convert.ToInt32(lkp.EditValue)==0)
            {
                lkp.ErrorText = frm_Master.ErrorText;
                return false;
            }
            return true;
        }
        public static bool IsDateValide(this DateEdit dt)
        {
            if (dt.DateTime.Year<1950)
            {
                dt.ErrorText = frm_Master.ErrorText;
                return false;
            }
            return true;
        }

        public static bool IsEditValueIntOfTypeInt(this LookUpEditBase edit)
        {
            var val = edit.EditValue;
            return (val is int ||val is byte);
        }
      public  static void IntializeDate(this LookUpEdit lkp, object dataSource)
        {

            lkp.IntializeDate( dataSource, "Name", "ID");
        }
       public static void IntializeDate(this LookUpEdit lkp, object dataSource, string displayMember, string valueMember)
        {
            lkp.Properties.DataSource = dataSource;
            lkp.Properties.DisplayMember = displayMember;
            lkp.Properties.ValueMember = valueMember;
            lkp.Properties.PopulateColumns();
            lkp.Properties.Columns[valueMember].Visible = false;
        }

    }
  
}
