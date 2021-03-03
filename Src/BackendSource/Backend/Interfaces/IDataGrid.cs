using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IDataGrid
    {
        void RefreshGrid();
        bool GridIsExist();
        Task InvokeGridAsync(string actionName);
    }
}
