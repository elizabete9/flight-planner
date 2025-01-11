using FlightPlanner.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AddFlightRequest
    {
        public AirportRequest From { get; set; }

        public AirportRequest To { get; set; }

        public string? Carrier { get; set; }

        public string? DepartureTime { get; set; }

        public string? ArrivalTime { get; set; }
       
        
    }
}
