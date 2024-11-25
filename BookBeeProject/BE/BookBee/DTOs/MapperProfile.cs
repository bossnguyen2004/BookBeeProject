using AutoMapper;
using BookBee.DTOs.Address;
using BookBee.DTOs.Author;
using BookBee.DTOs.Book;
using BookBee.DTOs.Cart;
using BookBee.DTOs.CartDetail;
using BookBee.DTOs.Order;
using BookBee.DTOs.OrderDetail;
using BookBee.DTOs.Publisher;
using BookBee.DTOs.Role;
using BookBee.DTOs.Tag;
using BookBee.DTOs.User;
using BookBee.Model;


namespace BookBee.DTOs
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserDTO, Model.User>().ReverseMap();

            CreateMap<RegisterUserDTO, Model.User>().ReverseMap();

            CreateMap<CreateUserDTO, Model.User>().ReverseMap();

            CreateMap<UpdateUserDTO, Model.User>().ReverseMap();

            CreateMap<AuthorDTO, Model.Author>().ReverseMap();

            CreateMap<PublisherDTO, Model.Publisher>().ReverseMap();

            CreateMap<AddressDTO, Model.Address>().ReverseMap();

            CreateMap<CreateAddressDTO, Model.Address>().ReverseMap();

            CreateMap<SelfCreateAddressDTO, Model.Address>().ReverseMap();

            CreateMap<UpdateAddressDTO, Model.Address>().ReverseMap();

            CreateMap<TagDTO, Model.Tag>().ReverseMap();



            CreateMap<BookDTO, Model.Book>()
                .ReverseMap()
                .ForMember(dest => dest.Sold, opt => opt.MapFrom(src => src.OrderDetails.Where(o => o.Order.Status == "DON").ToList().Sum(o => o.Quantity)));

            CreateMap<CreateBookDTO, Model.Book>().ReverseMap();

            CreateMap<UpdateBookDTO, Model.Book>().ReverseMap();

            CreateMap<QuantityDTO, Quantity>().ReverseMap();

            CreateMap<OrderDTO, Model.Order>().ReverseMap();

            CreateMap<CreateOrderDTO, Model.Order>().ReverseMap();

            CreateMap<SelfCreateOrderDTO, Model.Order>().ReverseMap();

            CreateMap<UpdateOrderDTO, Model.Order>().ReverseMap();

            CreateMap<CartDTO, Model.Cart>().ReverseMap();

            CreateMap<RoleDTO, Model.Role>().ReverseMap();


            CreateMap<CartDetailDTO, Model.CartDetail>().ReverseMap();

            CreateMap<Model.OrderDetail, OrderDetailDTO>().ReverseMap();


        }
    }
}
