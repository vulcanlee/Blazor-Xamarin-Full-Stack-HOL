namespace Backend.Helpers
{
    using AutoMapper;
    using Backend.AdapterModels;
    using DTOs.DataModels;
    using Domains.Models;

    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            #region Blazor AdapterModel
            CreateMap<MailQueue, MailQueueAdapterModel>();
            CreateMap<MailQueueAdapterModel, MailQueue>();
            CreateMap<WorkOrder, WorkOrderAdapterModel>();
            CreateMap<WorkOrderAdapterModel, WorkOrder>();
            CreateMap<MyUserPasswordHistory, MyUserPasswordHistoryAdapterModel>();
            CreateMap<MyUserPasswordHistoryAdapterModel, MyUserPasswordHistory>();
            CreateMap<SystemEnvironment, SystemEnvironmentAdapterModel>();
            CreateMap<SystemEnvironmentAdapterModel, SystemEnvironment>();
            CreateMap<FlowInbox, FlowInboxAdapterModel>();
            CreateMap<FlowInboxAdapterModel, FlowInbox>();
            CreateMap<FlowHistory, FlowHistoryAdapterModel>();
            CreateMap<FlowHistoryAdapterModel, FlowHistory>();
            CreateMap<FlowUser, FlowUserAdapterModel>();
            CreateMap<FlowUserAdapterModel, FlowUser>();
            CreateMap<FlowMaster, FlowMasterAdapterModel>();
            CreateMap<FlowMasterAdapterModel, FlowMaster>();
            CreateMap<PhaseMessage, PhaseMessageAdapterModel>();
            CreateMap<PhaseMessageAdapterModel, PhaseMessage>();
            CreateMap<PhaseCategory, PhaseCategoryAdapterModel>();
            CreateMap<PhaseCategoryAdapterModel, PhaseCategory>();
            CreateMap<PolicyDetail, PolicyDetailAdapterModel>();
            CreateMap<PolicyDetailAdapterModel, PolicyDetail>();
            CreateMap<PolicyHeader, PolicyHeaderAdapterModel>();
            CreateMap<PolicyHeaderAdapterModel, PolicyHeader>();

            CreateMap<SystemLog, SystemLogAdapterModel>();
            CreateMap<SystemLogAdapterModel, SystemLog>();
            CreateMap<MenuRole, MenuRoleAdapterModel>();
            CreateMap<MenuRoleAdapterModel, MenuRole>();
            CreateMap<MenuData, MenuDataAdapterModel>();
            CreateMap<MenuDataAdapterModel, MenuData>();
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
            CreateMap<MenuData, MenuDataDto>();
            CreateMap<MenuDataDto, MenuData>();
            CreateMap<MenuDataAdapterModel, MenuDataDto>();
            CreateMap<MenuDataDto, MenuDataAdapterModel>();

            CreateMap<MenuRole, MenuRoleDto>();
            CreateMap<MenuRoleDto, MenuRole>();
            CreateMap<MenuRoleAdapterModel, MenuRoleDto>();
            CreateMap<MenuRoleDto, MenuRoleAdapterModel>();

            CreateMap<MyUser, MyUserDto>();
            CreateMap<MyUserDto, MyUser>();
            CreateMap<MyUserAdapterModel, MyUserDto>();
            CreateMap<MyUserDto, MyUserAdapterModel>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<ProductAdapterModel, ProductDto>();
            CreateMap<ProductDto, ProductAdapterModel>();

            CreateMap<OrderMaster, OrderMasterDto>();
            CreateMap<OrderMasterDto, OrderMaster>();
            CreateMap<OrderMasterAdapterModel, OrderMasterDto>();
            CreateMap<OrderMasterDto, OrderMasterAdapterModel>();

            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItem>();
            CreateMap<OrderItemAdapterModel, OrderItemDto>();
            CreateMap<OrderItemDto, OrderItemAdapterModel>();
            #endregion
        }
    }
}
