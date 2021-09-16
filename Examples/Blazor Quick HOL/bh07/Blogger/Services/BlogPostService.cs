using Blogger.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public class BlogPostService : IBlogPostService
    {
        public BlogPostService(BlogDbContext context)
        {
            Context = context;
        }

        public BlogDbContext Context { get; }

        public async Task<List<BlogPost>> GetAsync()
        {
            var items = await Context.BlogPost
                .ToListAsync();
            return items;
        }
        public async Task PostAsync(BlogPost item)
        {
            await Context.BlogPost
                .AddAsync(item);
            await Context.SaveChangesAsync();
        }
        public async Task PutAsync(BlogPost item)
        {
            Context.ChangeTracker.Clear();
            Context.BlogPost.Update(item);
            await Context.SaveChangesAsync();
        }
        public async Task DeleteAsync(BlogPost item)
        {
            Context.ChangeTracker.Clear();
            Context.BlogPost.Remove(item);
            await Context.SaveChangesAsync();
        }
    }
}
