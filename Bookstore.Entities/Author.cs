using System;
using System.Collections.Generic;

namespace Bookstore.Entities
{
    public partial class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Nationality { get; set; }
        public byte[]? Photo { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
