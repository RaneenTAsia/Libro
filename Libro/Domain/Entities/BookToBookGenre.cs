using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookToBookGenre
    {
        public Book Book { get; set; }
        public int BookId { get; set; }
        public BookGenre BookGenre { get; set; }
        public Genre BookGenreId { get; set; }
    }
}
