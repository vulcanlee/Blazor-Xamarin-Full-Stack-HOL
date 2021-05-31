using Backend.AdapterModels;
using Backend.Services;
using Backend.SortModels;
using Entities.Models;
using ShareBusiness.Helpers;
using ShareDomain.DataModels;
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

        public async Task<List<MainMenu>> MakeMenuObjectAsync(bool isHttc)
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
                if (item.Level == 0)
                {
                    #region 第一層功能清單
                    mainMenu = new MainMenu()
                    {
                        IsSubMenu = item.IsGroup,
                        IsExpand = false,
                        MenuData = item
                    };
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

            #region 加入鴻才管理者可以使用的功能表清單
            if (isHttc == true)
            {
                mainMenu = new MainMenu()
                {
                    IsSubMenu = true,
                    IsExpand = false,
                    MenuData = new()
                    {
                        Level = 0,
                        Name = "系統管理者",
                        Enable = true,
                        ForceLoad = false,
                        Icon = "mdi-apple-icloud",
                        CodeName = "mdi-apple-icloud",
                        IsGroup = true,
                    }
                };
                mainMenus.Add(mainMenu);

                #region 第二層功能清單
                subMenu = new SubMenu()
                {
                    MenuData = new MenuDataAdapterModel()
                    {
                        Name = ShareBusiness.Helpers.MagicHelper.功能表角色功能名稱,
                        CodeName = "MenuRole",
                        Enable = true,
                        Guid = Guid.NewGuid(),
                        Icon = "mdi-menu",
                        IsGroup = false,
                        Level = 1,
                    }
                };
                mainMenu.SubMenus.Add(subMenu);
                subMenu = new SubMenu()
                {
                    MenuData = new MenuDataAdapterModel()
                    {
                        Name = ShareBusiness.Helpers.MagicHelper.系統日誌功能名稱,
                        CodeName = "SystemLog",
                        Enable = true,
                        Guid = Guid.NewGuid(),
                        Icon = "mdi-message-processing",
                        IsGroup = false,
                        Level = 1,
                    }
                };
                mainMenu.SubMenus.Add(subMenu);
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

        public static string MakeMenuDataHash(MenuData menuData)
        {
            string result = $"{menuData.CodeName} {MagicHelper.功能表項目數位簽名添加內容} {menuData.Guid}";
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(result));

                // Convert byte array to a string   
                StringBuilder builder = new();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                result = builder.ToString();
            }
            return result;
        }
    }
    public class MainMenu
    {
        public bool IsSubMenu { get; set; }
        public bool IsExpand { get; set; } = false;
        public bool IsClicked { get; set; } = false;
        public string MenuClass { get; set; } = "";
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
