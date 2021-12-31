using Backend.AdapterModels;
using Backend.Helpers;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BAL.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IMyUserService myUserService;
        private readonly ILogger<LoginModel> logger;
        private readonly IAccountPolicyService AccountPolicyService;

        public LoginModel(IMyUserService myUserService, ILogger<LoginModel> logger,
            SystemLogHelper systemLogHelper, IHttpContextAccessor httpContextAccessor,
            IAccountPolicyService AccountPolicyService)
        {
#if DEBUG
            Username = "god";
            Password = "123";
            PasswordType = "";
#endif
            this.myUserService = myUserService;
            this.logger = logger;
            SystemLogHelper = systemLogHelper;
            HttpContextAccessor = httpContextAccessor;
            this.AccountPolicyService = AccountPolicyService;
        }
        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";
        [BindProperty]
        public string Version { get; set; } = "";
        [BindProperty]
        public string CaptchaOrigin { get; set; }
        [BindProperty]
        public string Captcha { get; set; }
        public string PasswordType { get; set; } = "password";
        public string CaptchaImage { get; set; }
        public string Msg { get; set; }
        public string ReturnUrl { get; set; }
        public SystemLogHelper SystemLogHelper { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public async Task OnGetAsync()
        {
            Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            try
            {
                // �M���w�g�s�b���n�J Cookie ���e
                await HttpContext
                    .SignOutAsync(
                    MagicHelper.CookieAuthenticationScheme);
            }
            catch { }

            GetCaptchaImage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            AccountPolicyAdapterModel AccountPolicyAdapterModel = await AccountPolicyService.GetAsync();
            bool checkPreLoginData = true;
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(Username.Trim()) || string.IsNullOrEmpty(Password.Trim()))
            {
                Msg = "�b���Ϊ̱K�X���i���ť�";
                checkPreLoginData = false;
            }
            else if (string.IsNullOrEmpty(Captcha))
            {
                Msg = "�п�J���ҽX";
                checkPreLoginData = false;
            }
            else if (GetCaptchaSHA(Captcha) != CaptchaOrigin)
            {
                Msg = "���ҽX��J���~";
                checkPreLoginData = false;
            }

            if (checkPreLoginData)
            {
                #region �ˬd�ӨϥΪ̬O�_�w�g�Q���ΤF
                if (Username.ToLower() != MagicHelper.�}�o�̱b��)
                {
                    var accUser = await myUserService.UserByAccount(Username);
                    if (accUser == null)
                    {
                        #region �������ҥ��ѡA�ϥΪ̤��s�b
                        Msg = $"�������ҥ��ѡA�ϥΪ̱b�� ({Username}) �Ϊ̱K�X�����T";
                        await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                        {
                            Message = Msg,
                            Category = LogCategories.User,
                            Content = "",
                            LogLevel = LogLevels.Information,
                            Updatetime = DateTime.Now,
                            IP = HttpContextAccessor.GetConnectionIP(),
                        });
                        logger.LogInformation($"{Msg}");
                        GetCaptchaImage();
                        return Page();
                        #endregion
                    }

                    if (accUser.Status == false)
                    {
                        #region �ϥΪ̤w�g�Q���ΡA�L�k�n�J
                        Msg = $"�ϥΪ� {accUser.Account} �w�g�Q���ΡA�L�k�n�J";
                        await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                        {
                            Message = Msg,
                            Category = LogCategories.User,
                            Content = "",
                            LogLevel = LogLevels.Information,
                            Updatetime = DateTime.Now,
                            IP = HttpContextAccessor.GetConnectionIP(),
                        });
                        logger.LogInformation($"{Msg}");
                        GetCaptchaImage();
                        return Page();
                        #endregion
                    }

                    if (AccountPolicyAdapterModel.EnableLoginFailDetection)
                    {
                        if (accUser.LoginFailUnlockDatetime > DateTime.Now)
                        {
                            #region �ϥΪ̤w�g�Q���A�L�k�n�J
                            Msg = $"�ϥΪ� {accUser.Account} �]����J�L�h���~�K�X�A�w�g�Q���A�L�k�n�J";
                            await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                            {
                                Message = Msg,
                                Category = LogCategories.User,
                                Content = "",
                                LogLevel = LogLevels.Information,
                                Updatetime = DateTime.Now,
                                IP = HttpContextAccessor.GetConnectionIP(),
                            });
                            logger.LogInformation($"{Msg}");
                            GetCaptchaImage();
                            return Page();
                            #endregion
                        }
                    }
                }
                #endregion

                (MyUserAdapterModel user, string message) =
                await myUserService.CheckUser(Username, Password);
                if (user == null)
                {
                    #region �������ҥ��ѡA�ϥΪ̤��s�b
                    Msg = $"�������ҥ��ѡA�ϥΪ�({Username}���s�b : {message})";
                    await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                    {
                        Message = Msg,
                        Category = LogCategories.User,
                        Content = "",
                        LogLevel = LogLevels.Information,
                        Updatetime = DateTime.Now,
                        IP = HttpContextAccessor.GetConnectionIP(),
                    });
                    logger.LogInformation($"{Msg}");
                    GetCaptchaImage();
                    return Page();
                    #endregion
                }

                string returnUrl = Url.Content("~/");

                #region �[�J�o�ӨϥΪ̻ݭn�Ψ쪺 �ŧi���� Claim Type
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.NameIdentifier, user.Account),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(MagicHelper.MenuRoleClaim, user.MenuRoleId.ToString()),
                    new Claim(MagicHelper.MenuRoleNameClaim, user.MenuRole?.Name),
                    new Claim(MagicHelper.LastLoginTimeClaim, DateTime.Now.Ticks.ToString())
                };

                #region �Y�� �}�o�H���A�[�J �}�o�H�� �M�ݪ�����
                if (MagicHelper.�}�o�̱b��.ToString() == Username.ToLower())
                {
                    claims.Add(new Claim(ClaimTypes.Role, MagicHelper.�}�o�̪������n��));
                }
                else
                {
                    if (user.ForceChangePassword == true)
                    {
                        returnUrl = Url.Content("~/NeedChangePassword");
                    }
                }
                #endregion

                #region �[�J�ӨϥΪ̻ݭn�[�J����������
                var menuDatas = user.MenuRole.MenuData
                     .Where(x => x.Enable == true).ToList();

                foreach (var item in menuDatas)
                {
                    if (item.IsGroup == false)
                    {
                        if (item.CodeName.ToLower() != MagicHelper.�}�o�̪������n��)
                        {
                            // �קK�ϥΪ̦ۤv�[�J�@�� �}�o�H���M�� ������
                            if (!((item.CodeName.Contains("/") == true ||
                                item.CodeName.ToLower().Contains("http") == true)))
                            {
                                claims.Add(new Claim(ClaimTypes.Role, item.CodeName));
                            }
                        }
                    }
                }
                #endregion
                #endregion

                #region �إ� �ŧi�������ѧO
                // ClaimsIdentity���O�O�ŧi�������ѧO���������, �]�N�O�ŧi���X�Ҵy�z�������ѧO
                var claimsIdentity = new ClaimsIdentity(
                    claims, MagicHelper.CookieAuthenticationScheme);
                #endregion

                #region �إ�����{�Ҷ��q�ݭn�x�s�����A
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    RedirectUri = this.Request.Host.Value
                };
                #endregion

                #region �i��ϥεn�J
                try
                {
                    await HttpContext.SignInAsync(
                    MagicHelper.CookieAuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }
                #endregion

                Msg = $"�ϥΪ� ({Username}) �n�J���\";
                await SystemLogHelper.LogAsync(new SystemLogAdapterModel()
                {
                    Message = Msg,
                    Category = LogCategories.User,
                    Content = "",
                    LogLevel = LogLevels.Information,
                    Updatetime = DateTime.Now,
                    IP = HttpContextAccessor.GetConnectionIP(),
                });
                logger.LogInformation($"{Msg}");

                #region ��s�ϥΪ̳̫�n�J�ɶ�
                user.LastLoginDatetime = DateTime.Now;
                await myUserService.UpdateAsync(user);
                #endregion
                return LocalRedirect(returnUrl);
            }
            GetCaptchaImage();
            return Page();
        }

        public IActionResult OnPostRefreshCode()
        {
            Version = Assembly.GetEntryAssembly().GetName().Version.ToString();
            GetCaptchaImage();
            return Page();
        }

        #region �������ҽX
        public void GetCaptchaImage()
        {
            int length = 4;
            string captchaCode = CreateRandomCode(length);
#if DEBUG
            Captcha = captchaCode;
#endif
            CaptchaOrigin = GetCaptchaSHA(captchaCode);
            CaptchaImage = CreateCaptchaImage(captchaCode);
        }

        string CreateRandomCode(int length)
        {
            string randomCode = "";
            for (int i = 0; i < length; i++)
            {
                randomCode += new Random().Next(9);
            }
            return randomCode;
        }

        string CreateCaptchaImage(string captcha)
        {
            Bitmap bitmap = new Bitmap(120, 50);
            Graphics graphics = Graphics.FromImage(bitmap);
            Random random = new Random();
            string result = "";
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    graphics.Clear(Color.DarkGray);
                    for (int i = 0; i < 10; i++)
                    {
                        Pen whitePen = new Pen(Brushes.White,
                            random.Next(1, 4));
                        int x1 = random.Next(bitmap.Width);
                        int x2 = random.Next(bitmap.Width);
                        int y1 = random.Next(bitmap.Height);
                        int y2 = random.Next(bitmap.Height);
                        graphics.DrawLine(whitePen, x1, y1, x2, y2);
                    }

                    Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                    int startX = random.Next(0, 25);
                    int startY = random.Next(0, 8);
                    graphics.DrawString(captcha, new Font("Tahoma", 30), Brushes.White, startX, startY);

                    bitmap.Save(stream, ImageFormat.Jpeg);
                    byte[] imageBytes = stream.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    result = "data:image/png;base64," + base64String;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        string GetCaptchaSHA(string captcha)
        {
            SHA256 sha = SHA256.Create();
            byte[] bytes = sha.ComputeHash(Encoding.UTF8
                .GetBytes(MagicHelper.DefaultConnectionString + captcha + "VulcaN"));
            StringBuilder builder = new StringBuilder();
            string result = "";
            for (int j = 0; j < bytes.Length; j++)
            {
                builder.Append(bytes[j].ToString("x2"));
            }
            result = builder.ToString();

            return result;
        }
        #endregion
    }
}
