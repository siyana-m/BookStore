using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services.DTO.Books
{
    public class BookSummaryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty!;
        public decimal Price { get; set; }

    }
}
