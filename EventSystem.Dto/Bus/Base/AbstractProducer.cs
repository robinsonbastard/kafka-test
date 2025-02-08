using Confluent.Kafka;
using Confluent.Kafka.Admin;

namespace EventSystem.Dto.Bus.Base;

public abstract class AbstractProducer <TIn> : IBusProducer<TIn>
    where TIn: class
{
    private readonly IProducer<Null, TIn> _producer;
    private readonly ProducerConfig _producerConfig;
    private readonly ISerializer<TIn> _serializer;

    protected AbstractProducer(ProducerConfig producerConfig, ISerializer<TIn> serializer)
    {
        _producerConfig = producerConfig;
        _serializer = serializer;
        _producer = new ProducerBuilder<Null, TIn>(producerConfig).SetValueSerializer(_serializer).Build();
    }

    public async Task<BusResponse> ProduceAsync(TIn message, CancellationToken cancellationToken)
    {
        var topic = typeof(TIn).Name;

        using var adminClient = new AdminClientBuilder(new AdminClientConfig()
        {
            BootstrapServers = _producerConfig.BootstrapServers
        }).Build();

        var tsd = adminClient.GetMetadata(topic, TimeSpan.FromSeconds(1)).Topics;
        
        if (!adminClient.GetMetadata(topic, TimeSpan.FromSeconds(1)).Topics.Any())
        {
            await adminClient.CreateTopicsAsync(new List<TopicSpecification>()
            {
                new TopicSpecification()
                {
                    Name = topic,
                }
            });
        }
        var msg = new Message<Null, TIn>()
        {
            Value = message
        };

        try
        {
            await _producer.ProduceAsync(topic, msg, cancellationToken);

            return new BusResponse();
        }
        catch (Exception e)
        {
            return new BusResponse(e.Message);
        }
    }
}
