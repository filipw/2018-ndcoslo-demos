using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace OptionsDynamicLambda.Controllers
{
    [Route("api/[controller]")]
    public class AlbumsController : Controller
    {
        private IOptionsSnapshot<AlbumsOptions> _albumsOptions;

        public AlbumsController(IOptionsSnapshot<AlbumsOptions> albumsOptions)
        {
            _albumsOptions = albumsOptions;
        }

        private readonly List<Album> _albums = new List<Album>
        {
            new Album { Quantity = 10, Artist = "Betontod", Title = "Revolution", Genre = "punk rock" },
            new Album { Quantity = 50, Artist = "The Dangerous Summer", Title = "The Dangerous Summer", Genre = "alt rock" },
            new Album { Quantity = 200, Artist = "CHVRCHES", Title = "The Bones of What you Believe", Genre = "synth pop" },
            new Album { Quantity = 200, Artist = "Within Temptation", Title = "Let Us Burn", Genre = "gothic rock" }
        };

        [HttpGet]
        public IEnumerable<Album> Get()
        {
            var filteredAlbums = _albums.Where(_albumsOptions.Value.VisibleAlbumsFilterLambda);
            return filteredAlbums;
        }
    }
}
