﻿namespace Application.Interfaces.Responses
{
    public interface IResult<T>
    {
        T Data { get; set; }
        List<string> Messages { get; set; }
        Exception Exception { get; set; }
        bool Succeeded { get; set; }
        int Code { get; set; }
    }
}
