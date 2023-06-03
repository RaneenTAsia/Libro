using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BrowsingBookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Genres { get; set; }
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
