using System;

namespace OptionsDynamicLambda
{
    public class AlbumsOptions
    {
        public string VisibleAlbumsFilter { get; set; }

        public Func<Album, bool> VisibleAlbumsFilterLambda { get; set; }
    }
}
