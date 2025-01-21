
using System.Net;

namespace Server.common.Exceptions
{
    public class NotFoundException: Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;

        public NotFoundException(string message): base(message)
        {

        }
    }
}