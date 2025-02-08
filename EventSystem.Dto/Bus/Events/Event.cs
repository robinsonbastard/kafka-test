using System.Text.Json;

namespace EventSystem.Dto.Bus.Events;

public sealed record Event
{
    public Guid Id { get; init; }

    public string EventMessage { get; init; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}