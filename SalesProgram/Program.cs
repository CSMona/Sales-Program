using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SalesWithLinq.Forms;
using System.Reflection;

namespace SalesWithLinq
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frm_CompanyInfo());
            //Application.Run(new frm_Stores());
            //Application.Run(new frm_StoresList());
            //Application.Run(new frm_Drawer());

            //OpenFormByName("frm_Stores");
            //OpenFormByName("frm_StoresList");
            //OpenFormByName("frm_Drawer");
            //Application.Run(new frm_Main());
            //Application.Run(new frm_ProductCategory());
            //Application.Run(new frm_CustomerVendor(true));
            //Application.Run(new frm_Invoice());

            Application.Run(new frm_ProductList());

        }



      
    }
}
