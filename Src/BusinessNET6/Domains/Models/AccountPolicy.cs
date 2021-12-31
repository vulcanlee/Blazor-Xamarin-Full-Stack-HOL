using System;
using System.Collections.Generic;
using System.Text;

namespace Domains.Models
{
    public class AccountPolicy
    {
        public int Id { get; set; }
        /// <summary>
        /// 是否啟用登入失敗偵測
        /// </summary>
        public bool EnableLoginFailDetection { get; set; }
        /// <summary>
        /// 輸入錯誤密碼的最大次數
        /// </summary>
        public int LoginFailMaxTimes { get; set; }
        /// <summary>
        /// 觸發過多密碼錯誤條件，帳號被鎖定期限
        /// </summary>
        public int LoginFailTimesLockMinutes { get; set; }
        /// <summary>
        /// 需要變更密碼的週期
        /// </summary>
        public int PasswordAge { get; set; }
        /// <summary>
        /// 需要檢查密碼是否要定期更新
        /// </summary>
        public bool EnableCheckPasswordAge { get; set; }
        /// <summary>
        /// 密碼最小字數
        /// </summary>
        public int MinimumPasswordLength { get; set; }
        /// <summary>
        /// 紀錄變更密碼數量，以便免重覆變更之前密碼
        /// </summary>
        public int PasswordHistory { get; set; }
        public bool EnablePasswordHistory { get; set; }
        /// <summary>
        /// 輸入密碼的複雜度
        /// </summary>
        public int PasswordComplexity { get; set; }
    }
}
