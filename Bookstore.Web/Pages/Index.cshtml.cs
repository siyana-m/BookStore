using Bookstore.Services;
using Bookstore.Services.DTO.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bookstore.Web.Pages
{
    public class IndexModel : PageModel
    {

        private readonly BookService _booksService;
        public IndexModel(BookService booksService)
        {
            _booksService = booksService;
        }

        public string SearchTerm { get; set; }
        public IList<BookDto> Books { get; set; }

        public async Task<IActionResult> OnGetAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                Books = await _booksService.GetAll();
            }
            else
            {
                Books = await _booksService.Search(searchTerm);
            }
            return Page();
        }

    }

}