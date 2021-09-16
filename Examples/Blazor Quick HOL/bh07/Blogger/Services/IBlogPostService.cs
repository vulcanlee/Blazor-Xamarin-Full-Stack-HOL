using Blogger.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public interface IBlogPostService
    {
        Task DeleteAsync(BlogPost item);
        Task<List<BlogPost>> GetAsync();
        Task PostAsync(BlogPost item);
        Task PutAsync(BlogPost item);
    }
}