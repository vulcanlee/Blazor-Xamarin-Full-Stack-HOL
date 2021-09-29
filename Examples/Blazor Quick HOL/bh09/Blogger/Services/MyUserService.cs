using Blogger.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public class MyUserService : IMyUserService
    {
        public MyUserService(BlogDbContext context)
        {
            Context = context;
        }

        public BlogDbContext Context { get; }

        public async Task<List<MyUser>> GetAsync()
        {
            var items = await Context.MyUser
                .ToListAsync();
            return items;
        }
        public async Task PostAsync(MyUser item)
        {
            await Context.MyUser
                .AddAsync(item);
            await Context.SaveChangesAsync();
        }
        public async Task PutAsync(MyUser item)
        {
            Context.ChangeTracker.Clear();
            Context.MyUser.Update(item);
            await Context.SaveChangesAsync();
        }
        public async Task DeleteAsync(MyUser item)
        {
            Context.ChangeTracker.Clear();
            Context.MyUser.Remove(item);
            await Context.SaveChangesAsync();
        }
    }
}
