using System;
using System.Collections.Generic;
using System.Linq;

namespace MaruanBH.Domain.Base.Error
{
    public readonly struct Error
    {
        private Error(ErrorType errorType, IEnumerable<string> messages)
            : this(errorType, messages.ToArray())
        {
        }

        private Error(ErrorType errorType, params string[] messages)
        {
            Type = errorType;
            Date = DateTime.Now;
            Messages = messages;
        }

        public IReadOnlyList<string> Messages { get; }

        public DateTime Date { get; }

        public ErrorType Type { get; }

        public static Error NotFound(string error) =>
            new Error(ErrorType.NotFound, error);

        public static Error NotFound(IEnumerable<string> errors) =>
            new Error(ErrorType.NotFound, errors);


        public static Error BadRequest(string error) =>
            new Error(ErrorType.BadRequest, error);

        public static Error BadRequest(IEnumerable<string> errors) =>
            new Error(ErrorType.BadRequest, errors);

        public ErrorResponse ToErrorResponse() =>
        new ErrorResponse
        {
            Type = Type.ToString(),
            Date = Date,
            Messages = Messages
        };
    }
}
