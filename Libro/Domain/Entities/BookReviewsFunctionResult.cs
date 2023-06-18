using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookReviewsFunctionResult
    {
        public string Username { get; set; }
        public Rating Rating { get; set; }
        public string? ReviewContent { get; set; }
    }
}
