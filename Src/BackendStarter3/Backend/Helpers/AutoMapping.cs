namespace Backend.Helpers
{
    using AutoMapper;
    using Backend.AdapterModels;
    using DataTransferObject.DTOs;
    using Entities.Models;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            #region Blazor AdapterModel
            CreateMap<MyUser, MyUserAdapterModel>();
            CreateMap<MyUserAdapterModel, MyUser>();

            CreateMap<Product, ProductAdapterModel>();
            CreateMap<ProductAdapterModel, Product>();
            CreateMap<OrderMaster, OrderMasterAdapterModel>();
            CreateMap<OrderMasterAdapterModel, OrderMaster>();
            CreateMap<OrderItem, OrderItemAdapterModel>();
            CreateMap<OrderItemAdapterModel, OrderItem>();
            #endregion

            #region DTO
            CreateMap<MyUser, MyUserDto>();
            CreateMap<MyUserDto, MyUser>();
            CreateMap<MyUserAdapterModel, MyUserDto>();
            CreateMap<MyUserDto, MyUserAdapterModel>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<ProductAdapterModel, ProductDto>();
            CreateMap<ProductDto, ProductAdapterModel>();

            CreateMap<OrderMaster, OrderDto>();
            CreateMap<OrderDto, OrderMaster>();
            CreateMap<OrderMasterAdapterModel, OrderDto>();
            CreateMap<OrderDto, OrderMasterAdapterModel>();

            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderItemAdapterModel, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItemAdapterModel>();
            #endregion
        }
    }
}
