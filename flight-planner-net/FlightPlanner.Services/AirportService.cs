using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using FlightPlanner.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Services
{
    public class AirportService : EntityService<Airport>, IAirportService
    {
        public AirportService(FlightPlannerDbContext context) : base(context)
        {

        }
        public IEnumerable<Airport> SearchAirports(string search)
        {
            if (string.IsNullOrEmpty(search))
                return new List<Airport>();

            search = search.Trim().ToLower();

            return _context.Airports.Where(airport =>
                (airport.AirportCode ?? string.Empty).ToLower().Contains(search) ||
                (airport.City ?? string.Empty).ToLower().Contains(search) ||
                (airport.Country ?? string.Empty).ToLower().Contains(search)
            ).ToList();
        }
    }
}
