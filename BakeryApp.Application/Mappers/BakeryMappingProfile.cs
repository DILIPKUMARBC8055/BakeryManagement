using AutoMapper;
using BakeryApp.Application.Commands.ItemCommand;
using BakeryApp.Application.Commands.OrderCommand;
using BakeryApp.Application.Commands.UserCommand;
using BakeryApp.Application.Response;
using BakeryApp.Core.Entities;

namespace BakeryApp.Application.Mappers
{
    public class BakeryMappingProfile : Profile
    {
        public BakeryMappingProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<BakeryItem, BakeryItemDto>().ReverseMap();

            CreateMap<CreateOrderCommand, Order>().ReverseMap();
            CreateMap<CreateItemCommand, BakeryItem>().ReverseMap();
            CreateMap<CreateUserCommand, User>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
