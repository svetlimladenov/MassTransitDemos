using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Reservation.Service
{
    public class ReservationService : IHostedService
    {
        readonly IBusControl bus;

        public ReservationService(IBusControl bus)
        {
            this.bus = bus;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await this.bus.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await this.bus.StopAsync();
        }
    }
}
