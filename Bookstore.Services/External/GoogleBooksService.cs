using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services.External
{
    public class GoogleBooksService
    {
        private readonly string? _apiKey;
        private readonly HttpClient _httpClient;
        public GoogleBooksService(IHttpClientFactory clientFactory, IConfiguration
       configuration)
        {
            _httpClient = clientFactory.CreateClient("GoogleBooks");
            _apiKey = configuration["APIS:GoogleBooks:Key"];
        }
        public async Task<Item> GetBookDetails(string bookId)
        {
            var response = await
           _httpClient.GetAsync($"volumes/{bookId}?key={_apiKey}");
            if (!response.IsSuccessStatusCode)
            {
                return null!;
            }
            var content = await response.Content.ReadAsStringAsync();
            var book = JsonConvert.DeserializeObject<Item>(content);
            return book!;
        }
        public async Task<Item> SearchBookByISBN(string isbn)
        {
            var response = await
           _httpClient.GetAsync($"volumes?q=isbn:{isbn}&key={_apiKey}");
            if (!response.IsSuccessStatusCode)
            {
                return null!;
            }
            var content = await response.Content.ReadAsStringAsync();
            var searchResult =
           JsonConvert.DeserializeObject<GoogleBookSearchResultDto>(content);
            if (searchResult == null || searchResult.TotalItems == 0)
            {
                return null!;
            }
            var bookId = searchResult.Items?[0].Id;
            return await GetBookDetails(bookId!);
        }
    }

}
