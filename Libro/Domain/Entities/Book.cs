using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Book
    {
        public Book()
        {
            Authors = new List<Author>();
            BookGenres = new List<BookGenre>();
        }

        [Key]
        public int BookId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public int PageAmount { get; set; }
        public DateTime PublishDate { get; set; }
        public int BookStatus { get; set; }
        public List<Author> Authors { get; set; }
        public List<BookGenre> BookGenres { get; set; }
    }
}
