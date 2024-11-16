namespace WEB_253503_Kalabin.Domain.Models;

public class ResponseData<T>
{
    // запрашиваемые данные
    public T? Data { get; set; }
    // признак успешного завершения запроса
    public bool Successfull { get; set; } = true;
    // сообщение в случае неуспешного завершения
    public string? ErrorMessage { get; set; }
    public static ResponseData<T> Success(T data)
    {
        return new ResponseData<T> { Data = data };
    }
    public static ResponseData<T> Error(string message, T? data = default)
    {
        return new ResponseData<T>
        {
            ErrorMessage = message,
            Successfull = false,
            Data = data
        };
    }
}