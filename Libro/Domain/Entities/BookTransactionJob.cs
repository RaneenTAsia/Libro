using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookTransactionJob
    {
        public int BookTransactionJobId { get; set; }
        public string JobId { get; set; }
        public JobType BookTransactionJobType { get; set; }
        public BookTransaction BookTransaction { get; set; }
        public int BookTransactionId { get; set; }
    }
}
