using Confluent.Kafka;
using EventSystem.Dto.Bus.Base;
using EventSystem.Dto.Bus.Events;

namespace EventSystem.Engine.Bus.Producers;

public sealed class EventProducer : AbstractProducer<Event>, IEventMessageProducer
{
    public EventProducer(ProducerConfig config, ISerializer<Event> serializer) : base(config, serializer) { }
}
