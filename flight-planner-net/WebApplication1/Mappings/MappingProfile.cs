using AutoMapper;
using FlightPlanner.Core.Models;
using WebApplication1.Models;

namespace WebApplication1.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AddFlightRequest, Flight>();
            CreateMap<AirportRequest, Airport>()
                .ForMember(airport => airport.AirportCode,
                options => options.MapFrom(request => request.Airport))
                .ForMember(airport => airport.Id,
                options => options.Ignore());
            CreateMap<Flight, FlightResponse>();
            CreateMap<Airport, AirportResponse>()
                .ForMember(response => response.Airport,
                options => options.MapFrom(airport => airport.AirportCode));
        }
    }
}
