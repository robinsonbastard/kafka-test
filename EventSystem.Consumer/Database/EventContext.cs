using EventSystem.Dto.Bus.Events;
using Microsoft.EntityFrameworkCore;

namespace EventSystem.Consumer.Database;

public sealed class EventContext : DbContext
{
    public DbSet<Event> Events { get; set; }
}
