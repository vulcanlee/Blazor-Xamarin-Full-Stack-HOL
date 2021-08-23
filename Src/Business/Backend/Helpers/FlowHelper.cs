using AutoMapper;
using Backend.AdapterModels;
using Backend.Services;
using Domains.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class FlowHelper
    {
        public FlowHelper(IMapper mapper, IMyUserService myUserService)
        {
            Mapper = mapper;
            MyUserService = myUserService;
        }

        public IMapper Mapper { get; }
        public IMyUserService MyUserService { get; }

        public async Task CreateFlowHistory()
        {
            #region  取得現在登入使用者資訊
            #endregion

        }
    }
}
