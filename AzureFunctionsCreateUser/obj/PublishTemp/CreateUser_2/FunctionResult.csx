using System;

public class FunctionResult
{
    public int Id { get; }
    public bool Success { get; private set; }
    public string Message { get; }

    public FunctionResult(string message)
    {
        Id = new Random().Next(1, 100);

        Message = message;
        Success = true;
    }

    public FunctionResult SetError()
    {
        Success = false;
        return this;
    }
}