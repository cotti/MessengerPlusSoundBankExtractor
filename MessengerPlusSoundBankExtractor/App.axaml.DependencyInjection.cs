using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.EventLog;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;
using Splat.Microsoft.Extensions.Logging;
using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MessengerPlusSoundBankExtractor.ViewModels;
using MessengerPlusSoundBankExtractor.Views;
using Avalonia.Threading;

namespace MessengerPlusSoundBankExtractor
{
    public partial class App : Application
    {
        public IServiceProvider Container { get; private set; }

        public App()
        {
            Init();
        }

        void Init()
        {
            var host = Host
              .CreateDefaultBuilder()
              .ConfigureServices(services =>
              {
                  services.UseMicrosoftDependencyResolver();
                  var resolver = Locator.CurrentMutable;
                  resolver.InitializeSplat();
                  resolver.InitializeReactiveUI();

                  // Configure our local services and access the host configuration
                  ConfigureServices(services);
                  RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
              })
              .ConfigureLogging(loggingBuilder =>
              {
                  loggingBuilder.AddSplat();
              })
              .UseEnvironment(Environments.Development)
              .Build();

            // Since MS DI container is a different type,
            // we need to re-register the built container with Splat again
            Container = host.Services;
            Container.UseMicrosoftDependencyResolver();
        }

        void ConfigureServices(IServiceCollection services)
        {
            // register your personal services here, for example
            services.AddSingleton<MainWindowViewModel>();
        }
    }
}
