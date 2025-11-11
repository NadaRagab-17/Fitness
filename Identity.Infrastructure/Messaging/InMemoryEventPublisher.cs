using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cqrs.Events;
using Identity.Domain.Events;
using Identity.Application.Common;
using Microsoft.Extensions.Logging;


namespace Identity.Infrastructure.Messaging
{
    public class InMemoryEventPublisher : IEventPublisher
    {
        private readonly ILogger<InMemoryEventPublisher> _logger;

        public InMemoryEventPublisher(ILogger<InMemoryEventPublisher> logger)
        {
            _logger = logger;
        }
        public Task PublishAsync<T>(T @event) where T : class
        {
            _logger.LogInformation("📢 Event published: {@Event}", @event);
            return Task.CompletedTask;
        }
    }
}
