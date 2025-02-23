using System.ComponentModel;

namespace FastX.AspNetCore;


public class ResponseResult<T>
{
    /// <summary>
    /// 状态结果
    /// </summary>
    public bool Success { get; set; } = true;

    private string? _msg;

    /// <summary>
    /// 消息描述
    /// </summary>
    public string? Message
    {
        get => !string.IsNullOrEmpty(_msg) ? _msg : "";
        set => _msg = value;
    }

    /// <summary>
    /// 消息详细错误
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public T Data { get; set; } = default!;

    /// <summary>
    /// 成功状态返回结果
    /// </summary>
    /// <returns></returns>
    public static ResponseResult<T> SuccessResult(T data)
    {
        return new ResponseResult<T> { Data = data };
    }


    /// <summary>
    /// 异常状态返回结果
    /// </summary>
    /// <param name="msg">异常信息</param>
    /// <returns></returns>
    public static ResponseResult<T> ErrorResult(string? msg = null)
    {
        return new ResponseResult<T> { Success = false, Message = msg };
    }

}