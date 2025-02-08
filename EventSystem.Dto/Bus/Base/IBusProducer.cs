namespace EventSystem.Dto.Bus.Base;

public interface IBusProducer <TIn> where TIn: class
{
    Task<BusResponse> ProduceAsync(TIn message, CancellationToken cancellationToken);
}