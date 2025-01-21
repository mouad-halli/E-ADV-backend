using Newtonsoft.Json.Linq;

namespace Server.Interfaces.Services
{
    public interface IGraphApiService
    {
        Task<JObject> GetUserFromGraphApiAsync(string accessToken);
    }
}