using System;
using System.Collections.Generic;
using System.Text;

namespace CommonDomain.Enums
{
    public enum ErrorMessageEnum
    {
        None = 0,
        SecurityTokenExpiredException,
        SecurityTokenReplayDetectedException,
        SecurityTokenNotYetValidException,
        SecurityTokenValidationException,
        AuthenticationFailed,
        客製化文字錯誤訊息,
        // Web API 使用到的錯誤訊息
        帳號或密碼不正確 = 1000,
        權杖中沒有發現指定使用者ID,
        沒有發現指定的該使用者資料,
        傳送過來的資料有問題,
        沒有任何符合資料存在,
        沒有發現指定的請假單,
        權杖Token上標示的使用者與傳送過來的使用者不一致,
        沒有發現指定的請假單類別,
        要更新的紀錄_發生同時存取衝突_已經不存在資料庫上,
        紀錄更新時_發生同時存取衝突,
        紀錄更新所指定ID不一致,
        無法新增紀錄,
        無法修改紀錄,
        無法刪除紀錄,
        無法刪除紀錄_要刪除的紀錄已經不存在資料庫上,
        使用者需要強制登出並重新登入以便進行身分驗證,
        原有密碼不正確,
        新密碼不能為空白,
        密碼不能為空白,
        尚未輸入功能表的角色,
        該功能項目已經存在該功能表的角色,
        沒有指定功能表角色項目,
        沒有發現指定的發票,
        沒有發現指定的發票明細項目,
        // Blazor 專案 使用到的錯誤訊息
        資料有問題無法新增或者修改 = 20000,
        要新增的紀錄已經存在無法新增,
        要修改的紀錄已經存在無法修改,
        該紀錄無法刪除因為有其他資料表在使用中,
        開發者帳號不存在資料庫上,
        使用者帳號不存在,
        密碼不正確,
        訊息,
        警告,
        請再次輸入新密碼,
        新密碼2次輸入須相同,
        舊密碼不正確,
        新密碼須和舊密碼不同,
        密碼已經變更成功,
        開發者帳號不可以被新增,
        開發者帳號不可以被修改,
        開發者帳號不可以被刪除,
        使用者不存在,
        尚未輸入該訂單要用到的產品,
        該訂單已經存在該產品_不能重複同樣的商品在一訂單內,
        Exception = 2147483647,
    }
}
