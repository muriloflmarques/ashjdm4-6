using System;

namespace Tms.Infra.CrossCutting.CustomException
{
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException() { }

        public BusinessLogicException(string message) : base(message) { }

        public BusinessLogicException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}