# 方便開發快速替換內容樣板

*Product
  * 資料模型

*ProductSort
  * 宣告記錄排序方式

*ProductAdapterModel
  * Razor Component 使用的 Model
  * `加入Form Validation 屬性宣告` 
  * `註冊 AutoMapper (AutoMapping.cs)`

*ProductService
  * 取得資料庫的紀錄服務
  * 需要先設計具體類別，再產生相關介面
  * `需要進行註冊服務 Startup.cs`

*ProductAdapter
  * 取得紀錄的轉換器

*ProductRazorModel
  * Razor Component ViewModel
  * `註冊服務 Startup.cs`

*ProductView
  * Razor Component View

*ProductPage
  * 需要路由的頁面元件
  * `註冊功能表選項`

*ProductPicker
  * 可以開窗選取其他紀錄的元件
  * 不是每個資料表都會用到這樣的元件

*ProductByOrderView
  * 具有一對多的多標籤元件
  * 命名規則為 : (一)By(多)View.razor

