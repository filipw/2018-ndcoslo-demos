using ParallelPipelines;

namespace AdminApi
{
    public class AdminService : IHiService
    {
        public string SayHi()
        {
            return "Hi from Admin Service";
        }
    }
}
