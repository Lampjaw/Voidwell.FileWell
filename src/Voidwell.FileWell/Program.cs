﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Voidwell.Logging;

namespace Voidwell.FileWell
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseUrls("http://0.0.0.0:5000")
                .ConfigureLogging((context, builder) =>
                {
                    builder.ClearProviders();

                    var useGelf = context.Configuration.GetValue("UseGelfLogging", false);

                    builder.SetMinimumLevel(LogLevel.Information);

                    builder.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
                    builder.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Error);
                    builder.AddFilter("Microsoft.EntityFrameworkCore.Update", LogLevel.None);
                    builder.AddFilter("Microsoft.EntityframeworkCore.Database.Command", LogLevel.None);

                    if (useGelf && !context.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddGelf(options =>
                        {
                            options.LogSource = "Voidwell.FileWell";
                        });
                    }
                    else
                    {
                        builder.AddConsole();
                        builder.AddDebug();
                    }
                });
    }
}
