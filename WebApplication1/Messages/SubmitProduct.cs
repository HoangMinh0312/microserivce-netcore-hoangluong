using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Messages.Contract
{
    public class SubmitProduct
    {
        public string ProductName { get; set; }

        public int SupplierId { get; set; }

        public decimal? UnitPrice { get; set; }

        public string Package { get; set; }

        public bool IsDiscontinued { get; set; }
    }
}
