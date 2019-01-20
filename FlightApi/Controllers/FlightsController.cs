using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlightApi.FlightDataContext;
using FlightApi.Models;

namespace FlightApi.Controllers
{
    [Route("api/Flights")] //Husk at tjekke properties LaunchSettings
    [ApiController]
    //[Produces("aplication/xml")] tvinger alt til at blive vist som XML
    public class FlightsController : ControllerBase
    {
        private readonly DataContext _context;

        public FlightsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet]
        public IEnumerable<Flight> GetFlights()
        {
            return _context.Flights;
        }

        // GET: api/Flights/5
        [HttpGet("AllFlights")]
        public ActionResult<IEnumerable<Flight>> AllFlights()
        {
            List<Flight> flights = _context.Flights.ToList();
            if (flights == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid) //serverside validation. Hvis du i dit model lag har tilføjet fx annotaion [MinLenght(100)] så checker ModelState op om længden bliver overholdt
            {
                return BadRequest();
            }

            return flights;
        }

        [HttpGet("{id}")]
        public ActionResult<Flight> Get(int id)
        {
            var flight = _context.Flights.First(x => x.FlightId == id);
            return flight;
        }

        [HttpGet("SpecificFlights/{FromLocation}/{ToLocation}")]
        public List<Flight> GetSpecificFlights(string FromLocation, string ToLocation) 
        {
            List<Flight> specificFlights = new List<Flight>();
            foreach (var item in _context.Flights)
            {
                if (item.FromLocation == FromLocation && item.ToLocation == ToLocation)
                {
                    specificFlights.Add(item);
                }
            }
            return specificFlights;
        }

        [HttpGet("alldeparturesandarrivals")]
        public List<Flight> AllDeparturesAndArrivals()
        {
            List<Flight> flights = _context.Flights.ToList();
            return flights;
        }

        [HttpPut("{id}")]
        public void Put([FromBody] Flight flight)
        {
            //flight.FlightId = id;
            var saved = false;
            while (!saved)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        BadRequest(ModelState);
                    }
                    _context.Flights.Update(flight);
                    _context.SaveChanges();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex) //checker for cuncurrency
                {
                    ex.Entries.Single().Reload(); //Database værdier bliver IKKE overskrevet
                    _context.SaveChanges();
                    saved = true;
                }
            }
        }

        // POST: api/Flights
        [HttpPost]
        public void PostFlight([FromBody] Flight flight)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            _context.Flights.Add(flight);
            _context.SaveChanges();
            
        }

        // DELETE: api/Flights/5
        [HttpDelete("deleteflight/{id}")]
        public async Task<IActionResult> DeleteFlight([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            return Ok(flight);
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightId == id);
        }
    }
}