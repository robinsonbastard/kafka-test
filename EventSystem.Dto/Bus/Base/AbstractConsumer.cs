using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventSystem.Dto.Bus.Base;

public abstract class AbstractConsumer<TIn> : BackgroundService, IBusConsumer<TIn>
    where TIn : class
{
    private readonly IConsumer<Null, TIn> _consumer;
    private readonly ILogger<AbstractConsumer<TIn>> _logger;

    protected AbstractConsumer(IConsumer<Null, TIn> consumer, ILogger<AbstractConsumer<TIn>> logger)
    {
        _consumer = consumer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(typeof(TIn).Name);
        _logger.LogInformation("Подписался на топик {0}", typeof(TIn).Name);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var message = _consumer.Consume(cancellationToken);
                if (message.IsPartitionEOF)
                {
                    _logger.LogDebug("Закончились сообщения в партишене");
                    continue;
                }

                await ProcessMessageAsync(message.Message.Value, cancellationToken);
            }
            catch (NullReferenceException)
            {
                _logger.LogWarning("нет новых сообщений");
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Приложение завершается");
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Ошибка при обработке сообщения");
            }
        }
    }

    public abstract Task ProcessMessageAsync(TIn message, CancellationToken cancellationToken);
}
