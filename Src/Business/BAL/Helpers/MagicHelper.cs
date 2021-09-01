using System;
using System.Collections.Generic;
using System.Text;

namespace BAL.Helpers
{
    public class MagicHelper
    {
        #region 定義神奇字串或者神奇數值
        #region 系統預設參數
        public static readonly string AppName = "Blazor + Xamarin 超全端程式設計 動手練習系列";
        public static readonly string MenuMainTitle = "Blazor 超全端程式設計";
        public static readonly string DefaultConnectionString = "BackendDefaultConnection";
        public const string CookieAuthenticationScheme = "BackendCookieAuthenticationScheme"; // CookieAuthenticationDefaults.AuthenticationScheme
        public const string JwtBearerAuthenticationScheme = "BackendJwtBearerAuthenticationScheme"; // JwtBearerDefaults.AuthenticationScheme
        public static readonly string 未經授權無法存取此頁面 = "抱歉，未經授權無法存取此頁面。";
        public static readonly string 未能找到您要的網頁 = "對不起，未能找到您要的網頁。";
        public static readonly string 可能該網頁已被移除或被移到其他的網址 = "可能該網頁已被移除或被移到其他的網址。";
        public static readonly string 有錯誤發生要重新載入頁面 = "有錯誤發生，該應用程式將不無法提供服務，請重新載入 Reload 頁面。";
        public static readonly string 有例外異常錯誤發生 = "有例外異常錯誤發生，請使用網頁開發工具 F12 查看更多詳細訊息。";
        public static readonly string 確認有表頭預設文字 = "確認有表頭預設文字";
        public static readonly string MyUserUserMode = "MyUser";
        public static readonly string AgentUserMode = "Agent";
        public static readonly int GridPageSize = 12;
        public static readonly string MenuRoleClaim = "MenuRole";
        public static readonly string LastLoginTimeClaim = "LastLoginTime";
        public static readonly string SystemAdminClaim = "Administrator";
        #endregion

        #region 個別作業會用到的作業名稱宣告
        public static readonly string 首頁功能名稱 = "首頁";
        public static readonly string 登入 = "登入";
        public static readonly string 登出 = "登出";
        public static readonly string 變更密碼 = "變更密碼";
        public static readonly string 功能表角色功能名稱 = "功能表角色";
        public static readonly string 系統日誌功能名稱 = "系統日誌";
        public static readonly string Excel匯入功能名稱 = "Excel 匯入";
        public static readonly string 帳號管理功能名稱 = "帳號管理";
        public static readonly string 訂單管理功能名稱 = "訂單管理";
        public static readonly string 訂單明細管理功能名稱 = "訂單明細管理";
        public static readonly string 商品管理功能名稱 = "商品管理";
        public static readonly string 管理者專用功能名稱 = "管理者專用";
        public static readonly string 一般使用者使用功能名稱 = "一般使用者使用";

        public static readonly string 簽核流程政策 = "簽核流程政策";
        public static readonly string 簽核流程政策明細 = "簽核流程政策明細";
        public static readonly string 片語分類 = "片語分類";
        public static readonly string 片語文字 = "片語文字";
        public static readonly string 簽核文件 = "簽核文件";
        public static readonly string 簽核使用者明細 = "簽核使用者明細";
        public static readonly string 簽核歷史紀錄 = "簽核歷史紀錄";
        public static readonly string 簽核收件匣 = "簽核收件匣";
        public static readonly string 系統運作條件 = "系統運作條件";
        public static readonly string 派工單 = "派工單";
        public static readonly string 郵件寄送紀錄 = "郵件寄送紀錄";
        public static readonly string 系統摘要資訊 = "系統摘要資訊";
        public static readonly string 系統訊息廣播 = "系統訊息廣播";
        #endregion

        #region 測試與系統帳號
        public static readonly string 開發者功能表角色 = "開發者角色";
        public static readonly string 系統管理員功能表角色 = "系統管理員角色";
        public static readonly string 使用者功能表角色 = "使用者角色";
        public static readonly string MenuRoleNameClaim = "MenuRoleNameClaim";
        public const string 開發者的角色聲明 = "developer";
        public static string 開發者帳號 = "god";
        public static string 系統管理員帳號 = "admin";
        public static string[] 使用者帳號 = { "user1", "user2", "user3", "user4", "user5",
        "user6", "user7", "user8", "user9", "user10",
        "user11", "user12", "user13", "user14", "user15",
        "user16", "user17", "user18", "user19", "user20",};
        #endregion

        #region 作業會用到的名稱
        public static readonly string EnableTrue = "啟用";
        public static readonly string EnableFalse = "停用";

        #endregion
        #endregion

        #region 支援方法
        public static readonly int MaxEmailResend = 3;
        public static readonly int MailStatus等待 = 0;
        public static readonly int MailStatus失敗 = 1;
        public static readonly int MailStatus成功 = 2;
        #endregion
    }
}
