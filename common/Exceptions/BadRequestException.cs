using System.Net;

namespace Server.common.Exceptions
{
    public class BadRequestException: Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.BadRequest;

        public BadRequestException(string message): base(message)
        {
        }
    }
}