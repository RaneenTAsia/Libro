using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookUpdateDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int PageAmount { get; set; }
        public DateTime PublishDate { get; set; }
        public List<BookGenre> BookGenres { get; set; } = new List<BookGenre>();
        public List<Author> Authors { get; set; } = new List<Author>();
    }
}
