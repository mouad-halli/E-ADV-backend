using System.Net;

namespace Server.common.Exceptions
{
    public class ForbiddenException: Exception
    {
        public HttpStatusCode statusCode { get; } = HttpStatusCode.Forbidden;

        public ForbiddenException(string message): base(message)
        {
        }
    }
}