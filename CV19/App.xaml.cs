using System;
using System.Linq;
using System.Windows;
using CV19.Services;
using CV19.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CV19
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool IsDesingMode
        {
            get;
            private set;
        } = true;

        private static IHost __Host;

        public static IHost Host => __Host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        protected override async void OnStartup(StartupEventArgs e)
        {
            IsDesingMode = false;
            var host = Host;
            base.OnStartup(e);

            await host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var host = Host;
            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
            __Host = null;
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<DataService>();
            services.AddSingleton<CountriesStatisticViewModel>();
        }
    }
}
