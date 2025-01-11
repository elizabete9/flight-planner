using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class AirportRequest
    {
        public string Country { get; set; }

        public string City { get; set; }

        public string Airport { get; set; }
    }
}
