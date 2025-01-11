﻿using FlightPlanner.Core.Models;
using FlightPlanner.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("testing-api")]
    [ApiController]
    public class TestingController(IDbClearingService dbClearingService) : ControllerBase
    {
        private readonly IDbClearingService _dbClearingService = dbClearingService;

        [HttpPost]
        [Route("clear")]
        public IActionResult Clear() 
        {
            _dbClearingService.Clear<Airport>();
            _dbClearingService.Clear<Flight>();

            return Ok();
        }
    }
}
