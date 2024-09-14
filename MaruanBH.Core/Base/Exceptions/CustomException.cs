using System;
using MaruanBH.Domain.Base.Error;

namespace MaruanBH.Core.Base.Exceptions
{
    public class CustomException : Exception
    {
        public Error Error { get; }

        public CustomException(string message) : base(message)
        {
        }
        public CustomException(Error error)
            : base(error.Messages.FirstOrDefault())
        {
            Error = error;
        }
    }
}
