namespace Gacoan.Backend.Common.Models;

public class ServiceResult<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }

    public static ServiceResult<T> Ok(T data, string? message = null, int statusCode = 200)
        => new()
        {
            Success = true,
            Data = data,
            Message = message,
            StatusCode = statusCode
        };

    public static ServiceResult<T> Fail(string message, int statusCode = 400)
        => new()
        {
            Success = false,
            Data = default,
            Message = message,
            StatusCode = statusCode
        };
}