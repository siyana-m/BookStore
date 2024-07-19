using Bookstore.Entities;
using Bookstore.Services.DTO.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services.DTO.Orders
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual BookDto Book { get; set; } = null!;
        //public virtual OrderDto Order { get; set; } = null!;
    }
}
