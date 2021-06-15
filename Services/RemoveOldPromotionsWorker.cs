using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZooMag.Services.Interfaces;

namespace ZooMag.Services
{
    public class RemoveOldPromotionsWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RemoveOldPromotionsWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var promotionService = scope.ServiceProvider.GetRequiredService<IPromotionService>();
                await promotionService.DeleteOldPromotionsAsync();
                await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
            }
        }
    }
}