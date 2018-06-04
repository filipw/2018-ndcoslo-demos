using ParallelPipelines;

namespace CustomerApi
{
    public class PublicService : IHiService
    {
        public string SayHi()
        {
            return "Hi from Public Service";
        }
    }
}
