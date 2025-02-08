using Confluent.Kafka;
using EventSystem.Dto.Bus.Base;

namespace EventSystem.Dto.Bus.Events;

public interface IEventMessageConsumer : IBusConsumer<Event> { }
