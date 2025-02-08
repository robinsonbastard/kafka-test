using Confluent.Kafka;
using EventSystem.Consumer.Amqp.Consumers;
using EventSystem.Dto.Bus.Base;
using EventSystem.Dto.Bus.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ConsumerConfig>(x => new ConsumerConfig()
{
    BootstrapServers = "127.0.0.1:9092",
    GroupId = nameof(Event), //This id should be unique to every consumer. Kafka guarantees that a message is only ever read by a single consumer in the group.
    EnableAutoCommit = true, //Auto commits the offsets so that when consumer reconnects to the broker, broker has information of the last offset this consumer read the data from.
    AutoOffsetReset = AutoOffsetReset.Earliest, //If broker doesn't have consumer's last offset information it is auto set it.
    FetchWaitMaxMs = 500, //Max time consumer waits before filling the response with min bytes.
    EnablePartitionEof = true, //Triggers an event letting consumer know that there is no more data to consume.
    FetchErrorBackoffMs = 200, //If error occurs postpone the next fetch request for topic+partition.
});

builder.Services.AddSingleton(c =>
{
    return new ConsumerBuilder<Null, Event>(c.GetRequiredService<ConsumerConfig>())
        .SetValueDeserializer(new EventDesializer())
        .SetLogHandler((_, logMessage) =>
        {
            if (logMessage.Level < SyslogLevel.Notice)
            {
                var logger = c.GetRequiredService<ILogger<AbstractConsumer<Event>>>();
                
                logger.LogDebug(logMessage.Message);
            }
        })
        .Build();
});

builder.Services.AddHostedService<EventMessageConsumer>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();