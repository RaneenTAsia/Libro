using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookGenre
    {
        public Genre BookGenreId { get; set; }
        public string Genre { get; set; }
        public List<Book> Books { get; set; }

    }
}
