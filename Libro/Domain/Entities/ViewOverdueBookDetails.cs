using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ViewOverdueBookDetails
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
    }
}
