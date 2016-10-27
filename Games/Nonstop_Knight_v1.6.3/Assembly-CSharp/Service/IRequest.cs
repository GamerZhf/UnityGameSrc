namespace Service
{
    using System;

    public interface IRequest
    {
        string ErrorMsg { get; }

        Service.ExceptionResponse ExceptionResponse { get; }

        bool Success { get; }
    }
}

