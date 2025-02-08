using EventSystem.Dto.Bus.Events;
using Microsoft.AspNetCore.Mvc;

namespace EventSystem.Engine.Controllers;

[ApiController]
[Route("events")]
public sealed class EventController : ControllerBase
{
    private readonly IEventMessageProducer _eventMessageProducer;
    private readonly ILogger<EventController> _logger;

    public EventController(IEventMessageProducer eventMessageProducer, ILogger<EventController> logger)
    {
        _eventMessageProducer = eventMessageProducer;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult> GetAsync(CancellationToken cancellationToken)
    {
        
        var eventToNotify = new Event
        {
            Id = Guid.NewGuid(),
            EventMessage = $"New Event At {DateTimeOffset.UtcNow}"
        };
        
        _logger.LogInformation("Получил запрос на отправку ивента {0} с сообщением {1}", 
            eventToNotify.Id, eventToNotify.EventMessage);

        var response = await _eventMessageProducer.ProduceAsync(eventToNotify, cancellationToken);

        if (!response.IsSuccess)
        {
            return BadRequest(response.ErrorDescription);
        }

        return Ok();
    }
}
