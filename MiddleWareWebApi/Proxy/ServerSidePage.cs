using MiddleWareWebApi.Models;

namespace MiddleWareWebApi.Proxy
{
    public class ServerSidePage
    {
        // ReSharper disable once InconsistentNaming
        public int draw { get; set; }

        // ReSharper disable once InconsistentNaming
        public int recordsTotal { get; set; }

        // ReSharper disable once InconsistentNaming
        public int recordsFiltered { get; set; }

        // ReSharper disable once InconsistentNaming
        public User[] data { get; set; }

        // ReSharper disable once InconsistentNaming
        public string error { get; set; }
    }
}
