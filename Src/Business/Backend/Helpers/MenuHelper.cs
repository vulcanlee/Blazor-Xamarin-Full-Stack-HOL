using Backend.AdapterModels;
using Backend.Services;
using Backend.SortModels;
using Domains.Models;
using BAL.Helpers;
using CommonDomain.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Helpers
{
    public class MenuHelper
    {
        public MenuHelper(IMenuDataService menuDataService, int roleId)
        {
            MenuDataService = menuDataService;
            RoleId = roleId;
        }

        public IMenuDataService MenuDataService { get; }
        public int RoleId { get; }

        public async Task<List<MainMenu>> MakeMenuObjectAsync(bool isDeveloper)
        {
            #region 讀取該使用者角色的所有功能表清單
            var dataRequest = new DataRequest()
            {
                Sorted = new SortCondition()
                {
                    Id = (int)MenuDataSortEnum.Default,
                    Title = "預設"
                },
                Skip = 0,
                Take = int.MaxValue
            };
            var allMenuData = await MenuDataService.GetByHeaderIDAsync(RoleId, dataRequest);
            var menuDatas = allMenuData.Result;
            menuDatas = menuDatas
                .Where(x => x.Enable == true)
                .OrderBy(x => x.Sequence).ToList();
            #endregion

            List<MainMenu> mainMenus = new();
            MainMenu mainMenu = new();
            SubMenu subMenu = new();

            #region 依據資料庫內的紀錄，產生要顯示的功能表物件
            foreach (var item in menuDatas)
            {
                if(item.CodeName.ToLower().Contains("http:") ||
                    item.CodeName.ToLower().Contains("https:"))
                {
                    item.NewTab = true;
                }
                if (item.Level == 0)
                {
                    #region 第一層功能清單
                    mainMenu = new MainMenu()
                    {
                        IsSubMenu = item.IsGroup,
                        IsExpand = false,
                        MenuData = item
                    };
                    if (mainMenu.IsSubMenu == true)
                    {
                        mainMenu.ExpandIcon = "mdi mdi-18px mdi-chevron-right";
                    }
                    else
                    {
                        mainMenu.ExpandIcon = "";
                    }
                    mainMenus.Add(mainMenu);
                    #endregion
                }
                else
                {
                    #region 第二層功能清單
                    subMenu = new SubMenu()
                    {
                        MenuData = item
                    };
                    mainMenu.SubMenus.Add(subMenu);
                    #endregion
                }
            }
            #endregion

            #region 加入開發者可以使用的功能表清單
            if (isDeveloper == true)
            {
                mainMenu = new MainMenu()
                {
                    IsSubMenu = true,
                    IsExpand = false,
                    MenuData = new()
                    {
                        Level = 0,
                        Name = "開發者專用",
                        Enable = true,
                        ForceLoad = false,
                        Icon = "mdi-apple-icloud",
                        CodeName = "mdi-apple-icloud",
                        IsGroup = true,
                    }
                };
                mainMenu.ExpandIcon = "mdi mdi-18px mdi-chevron-right";
                mainMenus.Add(mainMenu);

                #region 第二層功能清單
                #region 功能表角色功能名稱
                subMenu = new SubMenu()
                {
                    MenuData = new MenuDataAdapterModel()
                    {
                        Name = BAL.Helpers.MagicHelper.功能表角色功能名稱,
                        CodeName = "MenuRole",
                        Enable = true,
                        Icon = "mdi-menu",
                        IsGroup = false,
                        Level = 1,
                    }
                };
                mainMenu.SubMenus.Add(subMenu);
                #endregion
                #region 系統日誌功能名稱
                subMenu = new SubMenu()
                {
                    MenuData = new MenuDataAdapterModel()
                    {
                        Name = BAL.Helpers.MagicHelper.系統日誌功能名稱,
                        CodeName = "SystemLog",
                        Enable = true,
                        Icon = "mdi-message-processing",
                        IsGroup = false,
                        Level = 1,
                    }
                };
                mainMenu.SubMenus.Add(subMenu);
                #endregion
                #region Icons 圖示
                subMenu = new SubMenu()
                {
                    MenuData = new MenuDataAdapterModel()
                    {
                        Name = "Icons 圖示",
                        CodeName = "https://pictogrammers.github.io/@mdi/font/5.8.55/",
                        Enable = true,
                        Icon = "mdi-family-tree",
                        IsGroup = false,
                        Level = 1, ForceLoad = true, NewTab = true,
                    }
                };
                mainMenu.SubMenus.Add(subMenu);
                #endregion
                #region Excel匯入功能名稱
                subMenu = new SubMenu()
                {
                    MenuData = new MenuDataAdapterModel()
                    {
                        Name = BAL.Helpers.MagicHelper.Excel匯入功能名稱,
                        CodeName = "Import",
                        Enable = true,
                        Icon = "mdi-database-import",
                        IsGroup = false,
                        Level = 1,
                    }
                };
                mainMenu.SubMenus.Add(subMenu);
                #endregion
                #region SystemInfo
                subMenu = new SubMenu()
                {
                    MenuData = new MenuDataAdapterModel()
                    {
                        Name = BAL.Helpers.MagicHelper.系統摘要資訊,
                        CodeName = "SystemInfo",
                        Enable = true,
                        Icon = "mdi-wifi-alert",
                        IsGroup = false,
                        Level = 1,
                    }
                };
                mainMenu.SubMenus.Add(subMenu);
                #endregion
                #endregion
            }
            #endregion  

            #region 加入登出按鈕
            if (mainMenus.Count > 0)
            {
                mainMenu = new MainMenu()
                {
                    IsSubMenu = false,
                    IsExpand = false,
                    MenuData = new()
                    {
                        Level = 0,
                        Name = "登出",
                        Enable = true,
                        ForceLoad = true,
                        NewTab = false,
                        Icon = "mdi-logout",
                        CodeName = "/Logout",
                        IsGroup = false,
                    }
                };
                mainMenus.Add(mainMenu);
            }
            #endregion

            return mainMenus;
        }

    }
    public class MainMenu
    {
        public bool IsSubMenu { get; set; }
        public bool IsExpand { get; set; } = false;
        public bool IsClicked { get; set; } = false;
        public string MenuClass { get; set; } = "";
        public string ExpandIcon { get; set; } = "";
        public MenuDataAdapterModel MenuData { get; set; }
        public List<SubMenu> SubMenus { get; set; } = new List<SubMenu>();
    }
    public class SubMenu
    {
        public bool IsClicked { get; set; } = false;
        public string MenuClass { get; set; } = "";
        public MenuDataAdapterModel MenuData { get; set; }
    }
}
