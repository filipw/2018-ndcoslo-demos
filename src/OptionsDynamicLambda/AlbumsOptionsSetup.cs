using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OptionsDynamicLambda
{
    public class AlbumsOptionsSetup : IPostConfigureOptions<AlbumsOptions>
    {
        private static SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private static Dictionary<string, Func<Album, bool>> _visibleAlbumFilters = new Dictionary<string, Func<Album, bool>>();

        private async Task<Func<Album, bool>> GetVisibleAlbumsFilter(string filter)
        {
            if (_visibleAlbumFilters.ContainsKey(filter))
            {
                return _visibleAlbumFilters[filter];
            }

            await _lock.WaitAsync();
            try
            {
                var options = ScriptOptions.Default.AddReferences(typeof(Album).Assembly);
                var compiledFilter = await CSharpScript.EvaluateAsync<Func<Album, bool>>(filter, options);
                _visibleAlbumFilters[filter] = compiledFilter;
                return compiledFilter;
            }
            finally
            {
                _lock.Release();
            }
        }

        public void PostConfigure(string name, AlbumsOptions options)
        {
            options.VisibleAlbumsFilterLambda = GetVisibleAlbumsFilter(options.VisibleAlbumsFilter).GetAwaiter().GetResult();
        }
    }
}
