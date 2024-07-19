using AutoMapper;
using Bookstore.Entities;
using Bookstore.Services.DTO.Authors;
using Bookstore.Services.DTO.Books;
using Bookstore.Services.DTO.Genres;
using Bookstore.Services.DTO.Languages;
using Bookstore.Services.DTO.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, BookSummaryDto>();
            CreateMap<Author, AuthorDto>().ForMember(x => x.FullName, x =>
           x.MapFrom(y => $"{y.FirstName} {y.LastName}"));
            CreateMap<Genre, GenreDto>();
            CreateMap<Language, LanguageDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailsDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<CreateBookDto, Book>()
            .ForMember(dest => dest.Authors, opt => opt.Ignore())
            .ForMember(dest => dest.Genres, opt => opt.Ignore());
        }


    }
}
