using Confluent.Kafka;
using EventSystem.Dto.Bus.Base;
using EventSystem.Dto.Bus.Events;

namespace EventSystem.Consumer.Amqp.Consumers;

public sealed class EventMessageConsumer : AbstractConsumer<Event>, IEventMessageConsumer
{
    private readonly ILogger<EventMessageConsumer> _logger;

    public EventMessageConsumer(IConsumer<Null,Event> consumer, ILogger<EventMessageConsumer> logger) : base(consumer, logger)
    {
        _logger = logger;
    }

    public override async Task ProcessMessageAsync(Event message, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Обрабатываю ивент {message.Id} с сообщением {message.EventMessage}");

        await Task.Delay(2000, cancellationToken);
        
        Console.WriteLine($"Обрабатал ивент {message.Id}");
    }
}
