using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Discount_;
namespace Manufacturer_
{
    // Class representing a manufacturer with a list of discounts
    public class Manufacturer
    {
        public string Name { get; set; }
        public List<Discount> Discounts { get; set; }

        public Manufacturer(string name, List<Discount> discounts)
        {
            Name = name;
            Discounts = discounts ?? new List<Discount>(); // Initialize the list of discounts
        }
    }

}
