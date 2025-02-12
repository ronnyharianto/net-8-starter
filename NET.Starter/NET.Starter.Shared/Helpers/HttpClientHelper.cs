using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text;

namespace NET.Starter.Shared.Helpers
{
    /// <summary>
    /// Provides helper methods for making HTTP requests with logging and optional authorization.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="HttpClientHelper"/> class.
    /// Sets the default timeout to 30 minutes.
    /// </remarks>
    /// <param name="client">An instance of <see cref="HttpClient"/>.</param>
    /// <param name="logger">An instance of <see cref="ILogger{TCategoryName}"/> for logging.</param>
    public class HttpClientHelper(HttpClient client, ILogger<HttpClientHelper> logger)
    {
        private readonly HttpClient _client = client;
        private readonly ILogger<HttpClientHelper> _logger = logger;

        /// <summary>
        /// Sends a POST request with form URL-encoded data.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="url">The endpoint URL to send the request to.</param>
        /// <param name="formUrlEncodedData">The form data as key-value pairs.</param>
        /// <returns>A deserialized response object, or null if the response cannot be deserialized.</returns>
        public async Task<T?> Post<T>(string url, Dictionary<string, string> formUrlEncodedData)
        {
            var urlEncodeData = string.Join(Environment.NewLine, formUrlEncodedData);
            _logger.LogInformation("Post Url => {url}, urlEncode data => {urlEncodeData}", url, urlEncodeData);

            var formUrlEncodedContent = new FormUrlEncodedContent(formUrlEncodedData);

            var response = await _client.PostAsync(url, formUrlEncodedContent);
            var responseText = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Response Text => {responseText}", responseText);

            return JsonHelper.DeserializeObject<T>(responseText); // Handle possible deserialization exceptions.
        }

        /// <summary>
        /// Sends a POST request with a JSON body and optional Basic Authentication.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request body.</typeparam>
        /// <typeparam name="TResponse">The type of the response object.</typeparam>
        /// <param name="url">The endpoint URL to send the request to.</param>
        /// <param name="requestBody">The object to be serialized into the JSON body.</param>
        /// <param name="basicAuth">Optional Basic Authentication header value in Base64 format.</param>
        /// <returns>A deserialized response object, or null if the response cannot be deserialized.</returns>
        public async Task<TResponse?> Post<TRequest, TResponse>(string url, TRequest requestBody, string? basicAuth)
        {
            if (!string.IsNullOrWhiteSpace(basicAuth))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

            var requestBodyJson = JsonHelper.SerializeObject(requestBody);
            _logger.LogInformation("Post Url => {url}, basic Auth {basicAuth}, Request body json => {requestBodyJson}",
                url, basicAuth, requestBodyJson);

            var content = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Response Text => {responseText}", responseText);

            return JsonHelper.DeserializeObject<TResponse>(responseText); // Handle potential JSON parsing errors.
        }

        /// <summary>
        /// Sends a GET request with an optional Bearer token.
        /// </summary>
        /// <typeparam name="T">The type of the response object.</typeparam>
        /// <param name="url">The endpoint URL to send the request to.</param>
        /// <param name="token">Optional Bearer token for authorization.</param>
        /// <returns>A deserialized response object, or null if the response cannot be deserialized.</returns>
        public async Task<T?> Get<T>(string url, string? token)
        {
            if (!string.IsNullOrWhiteSpace(token))
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _logger.LogInformation("Get Url => {url}, token data => {token}", url, token);

            var response = await _client.GetAsync(url);
            var responseText = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("Response Text => {responseText}", responseText);

            return JsonHelper.DeserializeObject<T>(responseText); // Handle possible deserialization exceptions.
        }
    }
}
