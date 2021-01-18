using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    using AutoMapper;
    using Entities.Models;
    using Backend.AdapterModels;
    using DataTransferObject.DTOs;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            #region Blazor AdapterModel
            CreateMap<TravelExpenseDetail, TravelExpenseDetailAdapterModel>();
            CreateMap<TravelExpenseDetailAdapterModel, TravelExpenseDetail>();
            CreateMap<TravelExpense, TravelExpenseAdapterModel>();
            CreateMap<TravelExpenseAdapterModel, TravelExpense>();
            CreateMap<LeaveForm, LeaveFormAdapterModel>();
            CreateMap<LeaveFormAdapterModel, LeaveForm>();
            CreateMap<WorkingLogDetail, WorkingLogDetailAdapterModel>();
            CreateMap<WorkingLogDetailAdapterModel, WorkingLogDetail>();
            CreateMap<WorkingLog, WorkingLogAdapterModel>();
            CreateMap<WorkingLogAdapterModel, WorkingLog>();
            CreateMap<Project, ProjectAdapterModel>();
            CreateMap<ProjectAdapterModel, Project>();
            CreateMap<LeaveCategory, LeaveCategoryAdapterModel>();
            CreateMap<LeaveCategoryAdapterModel, LeaveCategory>();
            CreateMap<OnCallPhone, OnCallPhoneAdapterModel>();
            CreateMap<OnCallPhoneAdapterModel, OnCallPhone>();
            CreateMap<MyUser, MyUserAdapterModel>();
            CreateMap<MyUserAdapterModel, MyUser>();

            CreateMap<Product, ProductAdapterModel>();
            CreateMap<ProductAdapterModel, Product>();
            CreateMap<Order, OrderAdapterModel>();
            CreateMap<OrderAdapterModel, Order>();
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
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<OrderAdapterModel, OrderDto>();
            CreateMap<OrderDto, OrderAdapterModel>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderItemAdapterModel, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItemAdapterModel>();
            #endregion
        }
    }
}
