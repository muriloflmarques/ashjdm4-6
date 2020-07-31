using System;

namespace Tms.Infra.CrossCutting.CustomException
{
    public class DomainRulesException : Exception
    {
        public DomainRulesException() { }

        public DomainRulesException(string message) : base(message) { }

        public DomainRulesException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}