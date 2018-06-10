using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ActionConstraint.Controllers
{
    [SwitzerlandOnly]
    [Route("api/movies")]
    public class SwissMoviesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Godfather 1", "Heidi", "Tatort" };
        }
    }

    [Route("api/movies")]
    public class MoviesController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Godfather 1", "Godfather 2", "Scarface", "Casino" };
        }
    }
}