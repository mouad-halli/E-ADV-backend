
using System.Net;

namespace Server.common.Exceptions
{
    public class UnauthorizedException: Exception
    {
        public HttpStatusCode statusCode { get; } = HttpStatusCode.Unauthorized;

        public UnauthorizedException(string message): base(message)
        {}
    }
}