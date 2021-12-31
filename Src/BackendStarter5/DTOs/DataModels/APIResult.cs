using System;
using System.ComponentModel;

namespace DTOs.DataModels
{
    /// <summary>
    /// 呼叫 API 回傳的制式格式
    /// </summary>
    public class APIResult : ICloneable, INotifyPropertyChanged
    {
        /// <summary>
        /// 此次呼叫 API 是否成功
        /// </summary>
        public bool Status { get; set; } = true;
        public int HTTPStatus { get; set; } = 200;
        public int ErrorCode { get; set; }
        /// <summary>
        /// 呼叫 API 失敗的錯誤訊息
        /// </summary>
        public string Message { get; set; } = "";
        /// <summary>
        /// 呼叫此API所得到的其他內容
        /// </summary>
        public object Payload { get; set; }

        #region 介面實作
        public event PropertyChangedEventHandler PropertyChanged;

        public APIResult Clone()
        {
            return ((ICloneable)this).Clone() as APIResult;
        }
        object ICloneable.Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion
    }
}
