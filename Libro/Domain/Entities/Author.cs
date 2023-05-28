using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Author
    {
        public Author()
        {
            WrittenBooks = new List<Book>(); 
        }

        [Key]
        public int AuthorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Book> WrittenBooks { get; set; }
    }
}
