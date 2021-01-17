# 方便開發快速替換內容樣板

* OnCallPhone
  * 資料模型

* OnCallPhoneSort
  * 宣告記錄排序方式

* OnCallPhoneAdapterModel
  * Razor Component 使用的 Model
  * `加入Form Validation 屬性宣告` 
  * `註冊 AutoMapper (AutoMapping.cs)`

* OnCallPhoneService
  * 取得資料庫的紀錄服務
  * 針對要排序與過濾的程式碼要特別設計
  * 新增、修改、刪除前的邏輯要特別設計
  * 取得後的紀錄，是否要做關聯紀錄取得與處理
  * 需要先設計具體類別，再產生相關介面(擷取介面，無須 IMapper)
  * `需要進行註冊服務 Startup.cs`

* OnCallPhoneAdapter
  * 取得紀錄的轉換器

* OnCallPhoneRazorModel
  * Razor Component ViewModel
  * 若有開窗選取紀錄，需要特別修正程式碼
  * `註冊服務 Startup.cs`

* OnCallPhoneView
  * Razor Component View
  * Grid 欄位調整
  * 修改與新增 紀錄的對話窗內容
  * 視情況加入對話窗引用

* OnCallPhonePage
  * 需要路由的頁面元件
  * `註冊功能表選項`

* OnCallPhonePicker
  * 可以開窗選取其他紀錄的元件
  * 不是每個資料表都會用到這樣的元件

* OnCallPhoneByOrderView
  * 具有一對多的多標籤元件
  * 命名規則為 : (一)By(多)View.razor

