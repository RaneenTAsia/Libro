using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BookDetailsDTO
    {
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Genres { get; set; }
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
        public Status BookStatus { set; private get; }
        public string Status
        {
            get
            {
                return BookStatus.ToString();
            }
        }
    }
}
