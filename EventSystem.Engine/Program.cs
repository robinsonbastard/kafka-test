using Confluent.Kafka;
using EventSystem.Dto.Bus.Events;
using EventSystem.Engine.Bus.Producers;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddGrpc();

builder.Services.AddScoped<IEventMessageProducer, EventProducer>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ProducerConfig>(c =>
{
    return new ProducerConfig()
    {
        BootstrapServers = "127.0.0.1:9092", // Kafka server your producer will connect to
        //Delivery error occurs when either the retry count or the message timeout are exceeded. An exception will be thrown
        MessageTimeoutMs = 1000, //Max time librdkafka may use to deliver produced. time 0 is infinity
        RetryBackoffMs = 100, //Time producer waits to retry message after failure.
        MessageSendMaxRetries = 3, //Number of times sending message is retired after failure

        DeliveryReportFields = "key, value, timestamp", //Only required fields. Reduces the size of delivery report. Use "none if you don't want any". Default is "all"
        EnableDeliveryReports = true, //Enables delivery report after successful message delivery
        
        EnableIdempotence = false, //Set true if message sequence in commit log is important. default is false.

        Acks = Acks.Leader, //Broker send ack to producer when the message in persisted based on the enum value assigned.
    };
});

builder.Services.AddSingleton<ISerializer<Event>, EventSerializer>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();