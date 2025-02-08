using EventSystem.Dto.Bus.Base;

namespace EventSystem.Dto.Bus.Events;

public interface IEventMessageProducer : IBusProducer<Event> { }