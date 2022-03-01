using DevExpress.XtraEditors;
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
    public partial class frm_Invoice : frm_Master
    {
        DAL.InvoiceHeader invoice;
        public frm_Invoice()
        {
            InitializeComponent();
        }

        private void frm_Invoice_Load(object sender, EventArgs e)
        {
            spn_Total.EditValue = 800;
           lkp_PartType.IntializeDate( Class.Master.PartTypesList);
            glkp_PartID.ButtonClick += Lkp_PartType_ButtonClick;
            spn_DiscountValue.Enter += new System.EventHandler(this.spn_DiscountValue_Enter);
            spn_DiscountValue.Leave += Spn_DiscountValue_Leave;
            spn_DiscountValue.EditValueChanged += Spn_DiscountValue_EditValueChanged;
            spn_DiscountRation.EditValueChanged += Spn_DiscountValue_EditValueChanged;

            spn_TaxValue.Enter += Spn_TaxValue_Enter;
            spn_TaxValue.Leave += Spn_TaxValue_Leave;
            spn_TaxValue.EditValueChanged += Spn_TaxValue_EditValueChanged;
            spn_Tax.EditValueChanged += Spn_TaxValue_EditValueChanged;

            spn_TaxValue.EditValueChanged += Spn_EditValueChanged;
            spn_DiscountValue.EditValueChanged += Spn_EditValueChanged;
            spn_Expences.EditValueChanged += Spn_EditValueChanged;

            spn_Paid.EditValueChanged += Spn_Paid_EditValueChanged;
            spn_Net.EditValueChanged += Spn_Paid_EditValueChanged;
        }
        public override void RefreshDate()
        {
            base.RefreshDate();
        }
        public override bool IsDataValide()
        {
            int NumberOfErrors = 0;
            NumberOfErrors += txt_Code.IsTextValide() ? 0 : 1;
            NumberOfErrors += lkp_PartType.IsEditValueValide()? 0 : 1;
            NumberOfErrors += lkp_Drawer.IsEditValueValide() ? 0 : 1;
            NumberOfErrors += lkp_Branch.IsEditValueValide() ? 0 : 1;
            NumberOfErrors += glkp_PartID.IsEditValueValideAndNotZero() ? 0 : 1;
            NumberOfErrors += (dt_Date.IsDateValide()) ? 0 : 1;

            if (chk_PostToStore.Checked)
            {
                NumberOfErrors += dt_PostDate.IsDateValide() ? 0 : 1;
                layoutControlGroup2.Expanded = true;

            }
            return (NumberOfErrors==0);
        }
        #region spenEditCalculation
        private void Spn_Paid_EditValueChanged(object sender, EventArgs e)
        {
            var net = Convert.ToDouble(spn_Net.EditValue);
            var paid = Convert.ToDouble(spn_Paid.EditValue);
            spn_Remaing.EditValue = net - paid;
        }

        private void Spn_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spn_Total.EditValue);
            var tax = Convert.ToDouble(spn_TaxValue.EditValue);
            var discount = Convert.ToDouble(spn_DiscountValue.EditValue);
            var expences = Convert.ToDouble(spn_Expences.EditValue);
            spn_Net.EditValue = (total + tax - discount + expences);
        }

            Boolean IsTaxtValueFoucused;
        private void Spn_TaxValue_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spn_Total.EditValue);
            var val = Convert.ToDouble(spn_TaxValue.EditValue);
            var ratio = Convert.ToDouble(spn_Tax.EditValue);
            if (IsTaxtValueFoucused)
            {
                spn_Tax.EditValue = (val / total);
            }
            else
            {
                spn_TaxValue.EditValue = total * ratio;
            }
        }

        private void Spn_TaxValue_Leave(object sender, EventArgs e)
        {
            IsTaxtValueFoucused = false;
        }

        private void Spn_TaxValue_Enter(object sender, EventArgs e)
        {
            IsTaxtValueFoucused = true;
        }

        private void Spn_DiscountValue_EditValueChanged(object sender, EventArgs e)
        {
            var total = Convert.ToDouble(spn_Total.EditValue);
            var discountVal = Convert.ToDouble(spn_DiscountValue.EditValue);
            var discountRation = Convert.ToDouble(spn_DiscountRation.EditValue);
            if (IsDiscountValueFoucused )
            {
                spn_DiscountRation.EditValue = (discountVal / total);
            }
            else
            {
                spn_DiscountValue.EditValue = total * discountRation;
            }
        }

        private void Lkp_PartType_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if(e.Button.Kind==DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            {
                using (var frm = new frm_CustomerVendor(Convert.ToInt32(lkp_PartType.EditValue) == (int)Master.PartType.Customer))
                {
                    frm.ShowDialog();
                    RefreshDate();
                }
            }
           
        }
        Boolean IsDiscountValueFoucused;
        private void spn_DiscountValue_Enter(object sender, EventArgs e)
        {
            IsDiscountValueFoucused = true;
        }
        private void Spn_DiscountValue_Leave(object sender, EventArgs e)
        {
            IsDiscountValueFoucused = false;
        }
        #endregion
    }
}