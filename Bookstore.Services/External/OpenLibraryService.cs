using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Http;
using Newtonsoft.Json;

namespace Bookstore.Services.External
{
    public class OpenLibraryService
    {
        private readonly HttpClient _httpClient;
        public OpenLibraryService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("OpenLibrary");
        }
        public async Task<OpenLibraryBookDto> GetBookAsync(string isbn)
        {
            var response = await _httpClient.GetAsync($"books?bibkeys=ISBN:{isbn}&format=json&jscmd=data");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<Dictionary<string, OpenLibraryBookDto>>(json);
            var bookResponse = data?.FirstOrDefault().Value;
            return bookResponse!;
        }
    }

}
