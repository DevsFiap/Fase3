using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fase03.Infra.IoC.Logging
{
    public class CustomLogger : ILogger
    {
        //public static bool Arquivo { get; set; } = false;
        private readonly string _loggerName;
        private readonly CustomLoggerProviderConfiguration _loggerConfig;

        public CustomLogger(string loggerName, CustomLoggerProviderConfiguration loggerConfig)
        {
            _loggerName = loggerName;
            _loggerConfig = loggerConfig;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string mensagem = string.Format($"{logLevel}: {eventId.Id} - {formatter(state, exception)}");
            Console.WriteLine( mensagem );
           
        }

        //private void SalvarLogNoBancoDeDados(string mensagem)
        //{
          
        //}
    }
}
