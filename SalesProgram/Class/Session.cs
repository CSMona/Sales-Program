using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesWithLinq.Class
{
   public static class Session
    {
        public static int DafaultDrawerID { get => 1; }
        private static BindingList<DAL.Product> _products;
        public static BindingList<DAL.Product> products { get {
                if (_products==null)
                {
                    using (var db=new DAL.dbDataContext())
                    {
                        _products = new BindingList<DAL.Product>(db.Products.ToList());
                    }
                }
                return _products;
            } }
    }
}
