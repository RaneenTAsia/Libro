using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ReadingList
    {
        public ReadingList()
        {
            ReadingItems = new List<ReadingItem>();
        }
        public int ReadingListId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public String Title { get; set; }
        public List<ReadingItem> ReadingItems { get; set; }
    }
}
