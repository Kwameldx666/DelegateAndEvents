using Product_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount_
{
    // Class representing a discount on a product
    public class Discount
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public Action<Product> Apply { get; set; }

        public Discount(string name, DateTime date, Action<Product> apply)
        {
            Name = name;
            Date = date;
            Apply = apply;
        }
    }

}
