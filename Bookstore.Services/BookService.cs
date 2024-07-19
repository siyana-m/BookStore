using AutoMapper;
using Bookstore.Data;
using Bookstore.Entities;
using Bookstore.Services.DTO.Authors;
using Bookstore.Services.DTO.Books;
using Bookstore.Services.DTO.Genres;
using Bookstore.Services.DTO.Languages;
using Bookstore.Services.DTO.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services
{
    public class BookService
    {
        private readonly Bookstore_v2023Context _dbContext;
        
        private readonly IMapper _mapper;
        public BookService(Bookstore_v2023Context dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<BookDto>> GetAll()
        {
            var books = await _dbContext.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Language)
            .ToListAsync();

            //return books.Select(b => new BookDto
            //{
            //    Id = b.Id,
            //    Title = b.Title,
            //    Description = b.Description ?? "",
            //    Price = b.Price,
            //    CoverImage = b.CoverImage,
            //    Publisher = b.Publisher ?? "",
            //    ISBN = b.Isbn,
            //    PublishingYear = b.PublishingYear,
            //    Language = new LanguageDto()
            //    {
            //        Id = b.Language.Id,
            //        Name = b.Language.Name
            //    },
            //    Authors = b.Authors.Select(ba => new AuthorDto
            //    {
            //        Id = ba.Id,
            //        FullName = $"{ba.FirstName} {ba.LastName}"
            //    }).ToList(),
            //    Genres = b.Genres.Select(bg => new GenreDto
            //    {
            //        Id = bg.Id,
            //        Name = bg.Name
            //    }).ToList()
            //}).ToList();

            return _mapper.Map<List<BookDto>>(books);
        }

        public async Task<List<BookDto>> Search(string searchTerm)
        {
            var books = await _dbContext.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Language)
            .Where(b => b.Title.Contains(searchTerm))
            .ToListAsync();
            //return books.Select(b => new BookDto
            //{
            //    Id = b.Id,
            //    Title = b.Title,
            //    Description = b.Description ?? "",
            //    Price = b.Price,
            //    CoverImage = b.CoverImage,
            //    Publisher = b.Publisher ?? "",
            //    ISBN = b.Isbn,
            //    PublishingYear = b.PublishingYear,
            //    Language = new LanguageDto()
            //    {
            //        Id = b.Language.Id,
            //        Name = b.Language.Name
            //    },
            //    Authors = b.Authors.Select(ba => new AuthorDto
            //    {
            //        Id = ba.Id,
            //        FullName = $"{ba.FirstName} {ba.LastName}"
            //    }).ToList(),
            //    Genres = b.Genres.Select(bg => new GenreDto
            //    {
            //        Id = bg.Id,
            //        Name = bg.Name
            //    }).ToList()
            //}).ToList();

            return _mapper.Map<List<BookDto>>(books);

        }
        public async Task<BookDto?> GetById(int bookId)
        {
            var book = await _dbContext.Books
            .Include(b => b.Authors)
            .Include(b => b.Genres)
            .Include(b => b.Language)
            .FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                return null;
            }
            //return new BookDto
            //{
            //    Id = book.Id,
            //    Title = book.Title,
            //    Description = book.Description??"",
            //    Price = book.Price,
            //    CoverImage = book.CoverImage,
            //    Publisher = book.Publisher??"",
            //    ISBN = book.Isbn,
            //    PublishingYear = book.PublishingYear,
            //    Language = new LanguageDto()
            //    {
            //        Id = book.Language.Id,
            //        Name = book.Language.Name
            //    },
            //    Authors = book.Authors.Select(ba => new AuthorDto
            //    {
            //        Id = ba.Id,
            //        FullName = $"{ba.FirstName} {ba.LastName}"
            //    }).ToList(),
            //    Genres = book.Genres.Select(bg => new GenreDto
            //    {
            //        Id = bg.Id,
            //        Name = bg.Name
            //    }).ToList()
            //};

            return _mapper.Map<BookDto>(book);

        }

        public async Task<BookDto> Create(CreateBookDto bookDto)
        {
            var book = _mapper.Map<Book>(bookDto);
            book.Authors = _dbContext.Authors.Where(a =>
            bookDto!.Authors!.Contains(a.Id)).ToList();
            book.Genres = _dbContext.Genres.Where(g =>
            bookDto!.Genres!.Contains(g.Id)).ToList();
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> Update(int id, BookDto bookDto)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return null!;
            }
            _mapper.Map(bookDto, book);
            _dbContext.Entry(book).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> UpdateCover(int id, byte[] coverImageData)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return null!;
            }
            book.CoverImage = coverImageData;
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }
        public async Task<bool> Delete(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }
            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<OrderDto>> GetBookPurchases(int bookId)
        {
            var book = await _dbContext.Books
            .Include(b => b.OrderDetails)
            .ThenInclude(od => od.Order)
            .FirstOrDefaultAsync(b => b.Id == bookId);
            if (book == null)
            {
                return null!;
            }
            var orders = book.OrderDetails.Select(od => od.Order).ToList();
            return _mapper.Map<List<OrderDto>>(orders);
        }
        public async Task<List<RevenueSummaryDto>> GetRevenueSummary()
        {
            var books = await _dbContext.Books
            .Include(b => b.OrderDetails)
            .ThenInclude(od => od.Order)
            .ToListAsync();
            var revenueSummary = books.Select(book => new RevenueSummaryDto
            {
                BookId = book.Id,
                Revenues = book.OrderDetails
            .GroupBy(od => new {
                od.Order.OrderDateTime.Year,
                od.Order.OrderDateTime.Month
            })
            .Select(g => new RevenueDto
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                TotalRevenue = g.Sum(od => od.UnitPrice * od.Quantity)
            })
            .OrderByDescending(r => r.Year)
            .ThenBy(r => r.Month)
            .ToList()
            })
            .ToList();
            return revenueSummary;
        }


    }
}
