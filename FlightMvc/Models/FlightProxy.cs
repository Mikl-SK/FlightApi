﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMvc.Models
{
    public class FlightProxy
    {
        //[Required] skal være på alle props, hvis det er krævet at der skal kunne laves et nyt fly i selve opgaven
        public int FlightId { get; set; }
        public string AircraftType { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public IEnumerable<FlightProxy> Flights { get; set; }
    }
}
