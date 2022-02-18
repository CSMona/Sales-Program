using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
    public enum productType
    {
        Inventory,
        Service
    }
}
