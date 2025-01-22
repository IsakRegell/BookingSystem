using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem
{
    public class Bookings
    {
        public int BookingId { get; set; }
        public int StudentId { get; set; }
        public int SceduelId { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public string Status {  get; set; }
    }
}
