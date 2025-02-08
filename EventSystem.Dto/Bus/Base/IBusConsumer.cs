namespace EventSystem.Dto.Bus.Base;

public interface IBusConsumer<TIn> where TIn: class
{
    Task ProcessMessageAsync(TIn message, CancellationToken cancellationToken);
}
