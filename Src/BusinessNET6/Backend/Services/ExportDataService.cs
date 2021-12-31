using BAL.Helpers;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Backend.Services
{
    public class ExportDataService : IExportDataService
    {
        public ExportDataService(BackendDBContext context)
        {
            Context = context;
        }

        public BackendDBContext Context { get; }
        public InitDatas InitDatas { get; set; } = new InitDatas();

        public async Task WriteToDirectoryAsync(string path)
        {
            var output = await GetCollectionJson();
            await File.WriteAllTextAsync(path, output);
        }
        public async Task<string> GetCollectionJson()
        {
            await CollectionRecord();
            var result = JsonConvert.SerializeObject(InitDatas);
            return result;
        }

        public async Task CollectionRecord()
        {
            InitDatas.AccountPolicy = await GetAccountPolicyAsync();
            InitDatas.MenuData = await GetMenuDataAsync();
            InitDatas.MenuRole = await GetMenuRoleAsync();
            InitDatas.MyUser = await GetMyUserAsync();
            InitDatas.MyUserPasswordHistory = await GetMyUserPasswordHistoryAsync();
        }

        #region 取得資料庫內的紀錄
        private async Task<List<MyUserPasswordHistory>> GetMyUserPasswordHistoryAsync()
        {
            CleanTrackingHelper.Clean<MyUserPasswordHistory>(Context);
            var items = await Context.MyUserPasswordHistory
                .AsNoTracking()
                .ToListAsync();
            return items;
        }

        private async Task<List<MyUser>> GetMyUserAsync()
        {
            CleanTrackingHelper.Clean<MyUser>(Context);
            var items = await Context.MyUser
                .AsNoTracking()
                .ToListAsync();
            return items;
        }

        private async Task<List<MenuRole>> GetMenuRoleAsync()
        {
            CleanTrackingHelper.Clean<MenuRole>(Context);
            var items = await Context.MenuRole
                .AsNoTracking()
                .ToListAsync();
            return items;
        }

        private async Task<List<MenuData>> GetMenuDataAsync()
        {
            CleanTrackingHelper.Clean<MenuData>(Context);
            var items = await Context.MenuData
                .AsNoTracking()
                .ToListAsync();
            return items;
        }

        private async Task<List<AccountPolicy>> GetAccountPolicyAsync()
        {
            CleanTrackingHelper.Clean<AccountPolicy>(Context);
            var items = await Context.AccountPolicy
                .AsNoTracking()
                .ToListAsync();
            return items;
        }
        #endregion
    }
}
