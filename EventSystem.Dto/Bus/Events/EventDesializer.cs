using System.Runtime.Serialization;
using System.Text.Json;
using Confluent.Kafka;

namespace EventSystem.Dto.Bus.Events;

public class EventDesializer : IDeserializer<Event>
{
    public Event Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            return new Event();
        }

        return JsonSerializer.Deserialize<Event>(data) ?? throw new SerializationException($"Не могу десериализовать ивент");
    }
}
