using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SalesWithLinq.Forms
{
    public partial class frm_Main : DevExpress.XtraBars.FluentDesignSystem.FluentDesignForm
    {
        public frm_Main()
        {
            InitializeComponent();
            accordionControl1.ElementClick += AccordionControl1_ElementClick;
           
        }

        private void AccordionControl1_ElementClick(object sender, DevExpress.XtraBars.Navigation.ElementClickEventArgs e)
        {
            var tag = e.Element.Tag as string;
            if(tag != string.Empty)
            {
                OpenFormByName(tag);
            }
        }

      

        public static void OpenFormByName(string name)
        {
            Form frm=null;
            if (name == "frm_Vendor")
            {
                frm = new frm_CustomerVendor(false);
                
            }
            if (name == "frm_Customer")
            {
                frm = new frm_CustomerVendor(true);
                
            }
            if (frm != null)
            {
                frm.Show();
                return;
            }
            else
            {
                var ins = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.Name == name);
                if (ins != null)
                {
                    frm = Activator.CreateInstance(ins) as Form;
                    if (Application.OpenForms[frm.Name] != null)
                    {
                        frm = Application.OpenForms[frm.Name];
                    }
                    else
                    {
                        frm.Show();
                    }
                    frm.BringToFront();
                }

            }

        }

       
    }
}
