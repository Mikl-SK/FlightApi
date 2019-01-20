using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FlightMvc.Models;
using System.Net.Http;

namespace FlightMvc.Controllers
{
    public class HomeController : Controller
    {
        public const string API = "https://localhost:44329";
        public static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(API);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public async Task<IActionResult> Index()
        {
            var client = GetClient();
            HttpResponseMessage response = await client.GetAsync($@"api/Flights/AllFlights"); 

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = await response.Content.ReadAsAsync<List<FlightProxy>>();

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult SearchFlights()
        {
            return View();
        }

        //[ValidateAntiForgeryToken] for at sikre mod XSS
        public async Task<IActionResult> EditFlights(int id)
        {
            var client = GetClient();
            HttpResponseMessage response = await client.GetAsync($@"api/Flights/{id}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = await response.Content.ReadAsAsync<FlightProxy>();

                return View(model);
            }
            else
            {
                return BadRequest("An error occured.");
            }
        }

        public async Task<IActionResult> EditFlightsPut(FlightProxy flight)
        {
            var client = GetClient();
            HttpResponseMessage response = await client.PutAsJsonAsync($@"api/airport/{flight.FlightId}", flight);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = await response.Content.ReadAsAsync<FlightProxy>();

                return RedirectToAction("Index");
                //return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> SearchFlightsAction(string FromLocation, string ToLocation)
        {
            var client = GetClient();
            HttpResponseMessage response = await client.GetAsync($@"api/Flights/SpecificFlights/{FromLocation}/{ToLocation}");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = await response.Content.ReadAsAsync<List<FlightProxy>>();

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> SeeAllLocations()
        {
            var client = GetClient();
            HttpResponseMessage response = await client.GetAsync($@"api/Flights/AllDeparturesAndArrivals");

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var model = await response.Content.ReadAsAsync<List<FlightProxy>>();

                return View(model);
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> DeleteFlight(int id)
        {
            var client = GetClient();
            string request = $@"api/Flights/deleteflight/{id}";
            HttpResponseMessage response = await client.DeleteAsync(request);

            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
