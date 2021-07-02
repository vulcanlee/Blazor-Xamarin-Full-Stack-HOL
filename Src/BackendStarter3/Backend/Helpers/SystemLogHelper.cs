using AutoMapper;
using Backend.AdapterModels;
using Backend.Services;
using Backend.SortModels;
using Domains.Models;
using BAL.Helpers;
using CommonDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class SystemLogHelper
    {
        public ISystemLogService SystemLogService { get; }
        public IMapper Mapper { get; }

        public SystemLogHelper(ISystemLogService systemLogService,
            IMapper mapper)
        {
            SystemLogService = systemLogService;
            Mapper = mapper;
        }

        public async Task LogAsync(SystemLogAdapterModel systemLogAdapterModel, Action action=null)
        {
            action?.Invoke();
            await SystemLogService.AddAsync(systemLogAdapterModel);
        }
    }
}
