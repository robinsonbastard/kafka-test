namespace EventSystem.Dto.Bus.Base;

public sealed record BusResponse
{
    public bool IsSuccess { get; init; }

    public string? ErrorDescription { get; init; }

    public BusResponse()
    {
        IsSuccess = true;
    }

    public BusResponse(string errorDescription)
    {
        IsSuccess = false;
        ErrorDescription = errorDescription;
    }
}