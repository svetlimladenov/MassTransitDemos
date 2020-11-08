using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample.Service
{
    public class MassTransitConsoleHostedService : IHostedService
    {
        private readonly IBusControl bus;

        public MassTransitConsoleHostedService(IBusControl bus)
        {
            this.bus = bus;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            return this.bus.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return this.bus.StopAsync(cancellationToken);
        }
    }
}
