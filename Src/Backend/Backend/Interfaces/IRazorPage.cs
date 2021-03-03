using System;
using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IRazorPage
    {
        void NeedRefresh();
        Task NeedInvokeAsync(Action action);
    }
}
