﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace UserMVCApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
           var host = CreateWebHostBuilder(args)
               .UseUrls("http://localhost:11942")
               .Build();

           host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
