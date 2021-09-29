using Blogger.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public interface IBlogPostService
    {
        BlogDbContext Context { get; }

        Task DeleteAsync(BlogPost item);
        Task<List<BlogPost>> GetAsync();
        Task<BlogPost> GetAsync(int id);
        Task PostAsync(BlogPost item);
        Task PutAsync(BlogPost item);
    }
}