using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Backend.AdapterModels;
    using Backend.SortModels;
    using Domains.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using BAL.Factories;
    using BAL.Helpers;
    using CommonDomain.DataModels;
    using CommonDomain.Enums;
    using System;

    public class PasswordPolicyService : IPasswordPolicyService
    {
        #region 欄位與屬性
        private readonly BackendDBContext context;
        public IMapper Mapper { get; }
        public ILogger<PasswordPolicyService> Logger { get; }
        #endregion

        #region 建構式
        public PasswordPolicyService(BackendDBContext context, IMapper mapper,
            ILogger<PasswordPolicyService> logger)
        {
            this.context = context;
            Mapper = mapper;
            Logger = logger;
        }
        #endregion

        #region 檢查密碼政策
        public async Task CheckPasswordAge()
        {

        }
        #endregion
    }
}
