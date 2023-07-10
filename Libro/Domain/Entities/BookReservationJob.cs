using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BookReservationJob
    {
        public int BookReservationJobId { get; set; }
        public string JobId { get; set; }
        public JobType BookReservationJobType { get; set; }
        public BookReservation BookReservation { get; set; }
        public int BookReservationId { get; set; }
    }
}
