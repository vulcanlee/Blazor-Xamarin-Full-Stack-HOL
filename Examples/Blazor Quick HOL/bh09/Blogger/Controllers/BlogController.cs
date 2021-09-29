using Blogger.Models;
using Blogger.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blogger.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public IBlogPostService BlogPostService { get; }
        #region 建構式
        public BlogController(IBlogPostService blogPostService)
        {
            BlogPostService = blogPostService;
        }
        #endregion

        #region C 新增
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BlogPost data)
        {
            await BlogPostService.PostAsync(data);
            return Ok();
        }
        #endregion

        #region R 查詢
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var records = await BlogPostService.GetAsync();
            return Ok(records);
        }
        #endregion

        #region U 更新
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] BlogPost data)
        {
            await BlogPostService.PutAsync(data);
            return Ok();
        }
        #endregion

        #region D 刪除
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var item = await BlogPostService.GetAsync(id);
            await BlogPostService.DeleteAsync(item);
            return Ok();
        }
        #endregion

    }
}
