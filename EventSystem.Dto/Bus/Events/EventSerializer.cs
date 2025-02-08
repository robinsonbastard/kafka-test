using System.Text.Json;
using Confluent.Kafka;

namespace EventSystem.Dto.Bus.Events;

public class EventSerializer : ISerializer<Event>
{
    public byte[] Serialize(Event data, SerializationContext context)
    {
        return JsonSerializer.SerializeToUtf8Bytes(data);
    }
}
