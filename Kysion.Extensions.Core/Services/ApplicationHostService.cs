using Kysion.Extensions.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace Kysion.Extensions.Core.Services
{
    public class ApplicationHostService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public ApplicationHostService(IServiceProvider serviceProvider)
        {
            // If you want, you can do something with these services at the beginning of loading the application.
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return HandleActivationAsync();
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates main window during activation.
        /// </summary>
        private Task HandleActivationAsync()
        {
            if (Application.Current.Windows.OfType<IWindow>().Any())
            {
                return Task.CompletedTask;
            }

            var mainWindow = _serviceProvider.GetRequiredService<IWindow>();
            mainWindow.Loaded += OnMainWindowLoaded;
            mainWindow?.Show();

            return Task.CompletedTask;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not IWindow)
            {
                return;
            }

            //_ = mainWindow.NavigationView.Navigate(typeof(DashboardPage));
        }
    }

}
