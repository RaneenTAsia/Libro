using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ReadingItem
    {
        public int ReadingItemId { get; set; }
        public Book Book { get; set; }
        public int BookId { get; set; }
        public ReadingList ReadingList { get; set; }
        public int ReadingListId { get; set; }
    }
}
