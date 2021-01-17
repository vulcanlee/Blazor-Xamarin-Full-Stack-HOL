# 方便開發快速替換內容樣板

* WorkingLogDetail
  * 資料模型

* WorkingLogDetailSort
  * 宣告記錄排序方式

* WorkingLogDetailAdapterModel
  * Razor Component 使用的 Model
  * `加入Form Validation 屬性宣告` 
  * `註冊 AutoMapper (AutoMapping.cs)`

* WorkingLogDetailService
  * 取得資料庫的紀錄服務
  * 針對要排序與過濾的程式碼要特別設計 
    
    (#region 進行搜尋動作 / #region 進行排序動作)
  * 新增、修改、刪除前的邏輯要特別設計
  * 取得後的紀錄，是否要做關聯紀錄取得與處理 
    
    (#region 在這裡進行取得資料與與額外屬性初始化)
  * 需要先設計具體類別，再產生相關介面(擷取介面，無須 IMapper)
  * `需要進行註冊服務 Startup.cs`

* WorkingLogDetailAdapter
  * 取得紀錄的轉換器

* WorkingLogDetailRazorModel
  * Razor Component ViewModel
  * 若有開窗選取紀錄，需要特別修正程式碼
  * `註冊服務 Startup.cs`

* WorkingLogDetailView
  * Razor Component View
  * Grid 欄位調整
  * 修改與新增 紀錄的對話窗內容
  * 視情況加入對話窗引用

* WorkingLogDetailPage
  * 需要路由的頁面元件
  * `註冊功能表選項`

* WorkingLogDetailPicker
  * 可以開窗選取其他紀錄的元件
  * 不是每個資料表都會用到這樣的元件

* WorkingLogDetailByWorkingLogView
  * 具有一對多的多標籤元件
  * 命名規則為 : (一)By(多)View.razor

