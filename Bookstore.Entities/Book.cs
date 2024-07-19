using System;
using System.Collections.Generic;

namespace Bookstore.Entities
{
    public partial class Book
    {
        public Book()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Authors = new HashSet<Author>();
            Genres = new HashSet<Genre>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public byte[]? CoverImage { get; set; }
        public string Isbn { get; set; } = null!;
        public string? Publisher { get; set; }
        public int? PublishingYear { get; set; }
        public decimal? Price { get; set; }
        public int LanguageId { get; set; }

        public virtual Language Language { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
    }
}
