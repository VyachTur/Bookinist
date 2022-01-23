using Bookinist.Services;
using Bookinist.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bookinist.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

#nullable disable

namespace Bookinist
{
    public partial class App : Application
    {
        private static IHost s_host;

        public static IHost Host => s_host 
                        ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
            .AddServices()
            .AddViewModels()
            .AddDatabase(host.Configuration.GetSection("Database"))
            ;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;

            using(var scope = Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DbInitializer>().InitializeAsync().Wait();
            }

            base.OnStartup(e);
            await host.StartAsync();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            using var host = Host;
            base.OnExit(e);
            host.StopAsync();
        }
    }
}
