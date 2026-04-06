using System;
using System.Collections.Generic;
namespace AdminSystem.Core.DTOs;

public class ApiResponse<T>
{
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> Success(T data, string message = "操作成功")
    {
        return new ApiResponse<T> { Code = 200, Message = message, Data = data };
    }

    public static ApiResponse<T> Success(string message = "操作成功")
    {
        return new ApiResponse<T> { Code = 200, Message = message };
    }

    public static ApiResponse<T> Fail(string message, int code = 500)
    {
        return new ApiResponse<T> { Code = code, Message = message };
    }
}

public class PageResult<T>
{
    public List<T> Items { get; set; } = new();
    public long Total { get; set; }
    public int PageNum { get; set; }
    public int PageSize { get; set; }
}

public class ApiResponse : ApiResponse<object>
{
    public new static ApiResponse Success(string message = "操作成功")
    {
        return new ApiResponse { Code = 200, Message = message };
    }

    public new static ApiResponse Fail(string message, int code = 500)
    {
        return new ApiResponse { Code = code, Message = message };
    }
}
