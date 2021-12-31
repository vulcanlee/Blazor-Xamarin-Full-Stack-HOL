using System.Threading.Tasks;

namespace Backend.Interfaces
{
    public interface IDataGrid
    {
        /// <summary>
        /// 強制更新 Grid 從 Adapter 讀取記錄
        /// </summary>
        void RefreshGrid();
        /// <summary>
        /// sfGrid 是否已經存在
        /// </summary>
        /// <returns></returns>
        bool GridIsExist();
        Task InvokeGridAsync(string actionName);
    }
}
