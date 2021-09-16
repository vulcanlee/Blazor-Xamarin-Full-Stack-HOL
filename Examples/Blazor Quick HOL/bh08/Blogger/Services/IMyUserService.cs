using Blogger.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blogger.Services
{
    public interface IMyUserService
    {
        Task DeleteAsync(MyUser item);
        Task<List<MyUser>> GetAsync();
        Task PostAsync(MyUser item);
        Task PutAsync(MyUser item);
    }
}