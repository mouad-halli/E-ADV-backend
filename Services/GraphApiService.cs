using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Server.Interfaces.Services;

namespace Server.Services
{
    public class GraphApiService: IGraphApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<GraphApiService> _logger;

        public GraphApiService(IHttpClientFactory httpClientFactory, ILogger<GraphApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // call Microsoft Graph API
        public async Task<JObject> GetUserFromGraphApiAsync(string accessToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                // set the authorization header
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // prepare Graph API URI and make the request
                // var url = "https://graph.microsoft.com/v1.0/me?$select=givenName,surname,mail,userPrincipalName,jobTitle,department,employeeId,userType,employeeHireDate";
                var url = "https://graph.microsoft.com/v1.0/me?$select=givenName,surname,mail,userPrincipalName,employeeId";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Fetching user data from Microsoft Graph API Failed: {StatusCode}", response.StatusCode);
                    return null;
                }
                
                // Serialize Graph Api Response content to a string then parse it to a JObject
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JObject.Parse(jsonResponse);
            }
            catch (HttpRequestException  ex)
            {
                _logger.LogError(ex, "Error from GraphApiService->GetUserFromGraphApiAsync");
                throw new ApplicationException("Error communicating with Microsoft Graph API", ex);
            }

        }
    }
}