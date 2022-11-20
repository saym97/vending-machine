using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreServices.DTOs.Product
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public int Cost { get; set; }

        public int AmountAvailable { get; set; }

        public Guid SellerId { get; set; }

    }
}
