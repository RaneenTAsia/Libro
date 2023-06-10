using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserBorrowingHistoryFunctionResult
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Decimal? Fine { get; set; } = 0M;
    }
}
