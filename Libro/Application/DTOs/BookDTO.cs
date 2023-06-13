using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int PageAmount { get; set; }
        public DateTime PublishDate { private get; set; }
        public string DatePublished
        {
            get
            {
                return PublishDate.ToLongDateString();
            }
        }
    }
}
