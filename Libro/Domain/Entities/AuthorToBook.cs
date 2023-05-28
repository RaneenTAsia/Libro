using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AuthorToBook
    {
        public Author Author { get; set; }
        public int AuthorId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
    }
}
