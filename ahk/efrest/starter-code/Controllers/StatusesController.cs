using api.DAL;
using api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace api.Controllers
{
    [Route("api/statuses")] // DO NO CHANGE THE URL - NE VALTOZTASD MEG AZ URLT
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusesRepository repository;

        // DO NOT CHANGE THE CONSTRUCTOR - NE VALTOZTSD MEG A KONSTRUKTORT
        public StatusesController(IStatusesRepository repository)
        {
            this.repository = repository;
        }
    }
}
