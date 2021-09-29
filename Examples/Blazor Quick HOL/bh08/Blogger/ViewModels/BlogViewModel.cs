using Blogger.Models;
using Blogger.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.ViewModels
{
    public class BlogViewModel
    {
        public BlogViewModel(IBlogPostService blogPostService)
        {
            BlogPostService = blogPostService;
        }

        #region Property
        /// <summary>
        /// 要顯示在畫面上的集合紀錄
        /// </summary>
        public List<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
        /// <summary>
        /// 現在正在新增或者修改和刪除的當前紀錄
        /// </summary>
        public BlogPost CurrentBlogPost { get; set; } = new BlogPost();
        /// <summary>
        /// 是否顯示編輯當前紀錄的 UI
        /// </summary>
        public bool ShowEditRecord { get; set; } = false;
        /// <summary>
        /// 是否顯示確認是否要刪除的對話窗
        /// </summary>
        public bool ShowConfirmDeleteRecord { get; set; } = false;
        /// <summary>
        /// 現在操作屬於 新增 還是 修改
        /// </summary>
        public bool IsAddRecord { get; set; } = false;
        /// <summary>
        /// 得到現在是新增或者修改模式的說明文字
        /// </summary>
        public string IsAddRecordName
        {
            get
            {
                if (IsAddRecord) return "新增";
                else return "修改";
            }
        }

        public IBlogPostService BlogPostService { get; }
        #endregion

        #region 讀取資料庫內的紀錄
        public async Task ReloadAsync()
        {
            BlogPosts = await BlogPostService.GetAsync();
        }
        #endregion

        #region UI 綁定事件委派方法
        public void OnAdd()
        {
            CurrentBlogPost = new BlogPost();
            IsAddRecord = true;
            ShowEditRecord = true;
        }

        public void OnEdit(BlogPost postItem)
        {
            #region 複製要修該物件的值
            CurrentBlogPost = new BlogPost();
            CurrentBlogPost.Id = postItem.Id;
            CurrentBlogPost.Title = postItem.Title;
            CurrentBlogPost.Text = postItem.Text;

            CurrentBlogPost.PublishDate = postItem.PublishDate;
            #endregion

            IsAddRecord = false;
            ShowEditRecord = true;
        }

        public async Task OnRecordEditAsync(bool recordChanged)
        {
            if (recordChanged == true)
            {
                if (IsAddRecord == true)
                {
                    await BlogPostService.PostAsync(CurrentBlogPost);
                    await ReloadAsync();
                }
                else
                {
                    await BlogPostService.PutAsync(CurrentBlogPost);
                    await ReloadAsync();
                }
            }
            ShowEditRecord = false;
        }

        public void OnShowDeleteConfirm(BlogPost postItem)
        {
            CurrentBlogPost = postItem;
            ShowConfirmDeleteRecord = true;
            return;
        }
        #endregion

        #region 刪除確認對話窗的方法
        public async Task OnConfirmDialogAsync(bool confirm)
        {
            ShowConfirmDeleteRecord = false;
            if (confirm == true)
            {
                await BlogPostService.DeleteAsync(CurrentBlogPost);
                await ReloadAsync();
            }
        }
        #endregion
    }
}
