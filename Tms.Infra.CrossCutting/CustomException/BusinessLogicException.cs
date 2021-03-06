﻿using System;

namespace Tms.Infra.CrossCutting.CustomException
{
    /// <summary>
    /// The Custom Exception can lead to special tratatives such as loggin specific errors,
    /// starting a queue process or even showing some messages in a different way to the user
    /// than would be shown to the development team
    /// </summary>
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException() { }

        public BusinessLogicException(string message) : base(message) { }

        public BusinessLogicException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}