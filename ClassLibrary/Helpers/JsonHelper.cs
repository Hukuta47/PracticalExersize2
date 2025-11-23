using Newtonsoft.Json;


namespace ClassLibrary.Helpers
{
    public struct JsonHelper
    {
        private static readonly HttpClient _client = new HttpClient();
        public static async Task<T> GetAsync<T>(string url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // бросит исключение, если не 2xx

            string json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
