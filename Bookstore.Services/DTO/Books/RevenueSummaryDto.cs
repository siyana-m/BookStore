using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services.DTO.Books
{
    public class RevenueSummaryDto
    {
        public int BookId { get; set; }
        public List<RevenueDto>? Revenues { get; set; }
    }
}
