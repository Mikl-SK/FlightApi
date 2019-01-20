using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightApi.Models
{
    public class Flight
    {
        [Key] // [Key] og andre notations skal ikke med over i proxy delen
        public int FlightId { get; set; }
        public string AircraftType { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        //[MinLength(100)] 
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }


        //til concurrency           SKAL IKKE MED OVER I PROXY DELEN
        //[Timestamp]
        //public byte[] RowVersion { get; set; }



    }
}
