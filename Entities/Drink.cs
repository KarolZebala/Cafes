using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafeApi.Entities
{
    public class Drink
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CafeId { get; set; }
        public virtual Cafe Cafe { get; set; }
    }
}
