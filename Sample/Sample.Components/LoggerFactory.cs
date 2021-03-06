﻿using MassTransit.Context;
using Serilog;
using Serilog.Extensions.Logging;

namespace Sample.Components
{
    public static class LoggerFactory
    {
        public static void SetupMassTransitLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            LogContext.ConfigureCurrentLogContext(new SerilogLoggerFactory());
        }
    }
}
