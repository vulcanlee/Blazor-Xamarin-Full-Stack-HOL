using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Services
{
    using AutoMapper;
    using Entities.Models;
    using Microsoft.EntityFrameworkCore;
    using ShareBusiness.Helpers;
    using System;

    public class DatabaseInitService
    {
        private readonly BackendDBContext context;

        public IMapper Mapper { get; }

        public DatabaseInitService(BackendDBContext context, IMapper mapper)
        {
            this.context = context;
            Mapper = mapper;
        }

        public async Task InitDataAsync()
        {
            Random random = new Random();

            #region 適用於 Code First ，刪除資料庫與移除資料庫
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
            #endregion

            #region 建立使用者紀錄 
            var items = Get姓名();
            CleanTrackingHelper.Clean<MyUser>(context);
            int idx = 1;
            foreach (var item in items)
            {
                var itemMyUser = await context.MyUser
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Name == item);
                if (itemMyUser == null)
                {
                    itemMyUser = new MyUser()
                    {
                        Account = $"user{idx}",
                        Name = $"{item}",
                        Password = "pw",
                    };
                    if (idx == 9) itemMyUser.IsManager = true;
                    context.Add(itemMyUser);
                    await context.SaveChangesAsync();
                    idx++;
                }
            }
            CleanTrackingHelper.Clean<MyUser>(context);
            #endregion

            #region 建立產品紀錄
            CleanTrackingHelper.Clean<Product>(context);
            CleanTrackingHelper.Clean<Order>(context);
            CleanTrackingHelper.Clean<OrderItem>(context);
            List<Product> products = new List<Product>();
            for (int i = 0; i < 10; i++)
            {
                Product product = new Product()
                {
                    Name = $"Product{i}"
                };
                products.Add(product);
                context.Add(product);
            }
            await context.SaveChangesAsync();
            #endregion

            #region 建立產品紀錄
            for (int i = 0; i < 3; i++)
            {
                Order order = new Order()
                {
                    Name = $"Order{i}",
                    OrderDate = DateTime.Now.AddDays(random.Next(30)),
                    RequiredDate = DateTime.Now.AddDays(random.Next(30)),
                    ShippedDate = DateTime.Now.AddDays(random.Next(30)),
                };
                context.Add(order);
                await context.SaveChangesAsync();
                var total = random.Next(1, 6);
                for (int j = 0; j < total; j++)
                {
                    OrderItem orderItem = new OrderItem()
                    {
                        Name = $"OrderItem{j}",
                        OrderId = order.Id,
                        ProductId = products[j].Id,
                        Quantity = 3,
                        ListPrice = 168,
                    };
                    context.Add(orderItem);
                }
                await context.SaveChangesAsync();
            }
            CleanTrackingHelper.Clean<Product>(context);
            CleanTrackingHelper.Clean<Order>(context);
            CleanTrackingHelper.Clean<OrderItem>(context);
            #endregion
        }
        public List<string> Get姓名()
        {
            string nameString = "邱明儒、連寧甫、張瑞信、黃素貞、林淑惠、林國瑋、楊逸群、黃思翰、施詩婷、許建勳、蔡明虹、車振宇、李政心、王秀美、陳彥廷、袁幸萍、程怡君、林育齊、梁禎火、游登冰、袁彥璋、謝其易、楊宗翰、林欣年、王博新、林家希、陳奕翔、王萱寧、馮哲維、張瑤任、杜子方、曹松隆、張瑞發、陳俐瑜、侯宇嘉、翁俊毅、陳奕海、吳千慧、何茂平、郭又蕙、蘇志忠、藍邦琬、黃慧珠、夏姿瑩、劉柏鈞、張剛伸、顏彥文、張嘉玫、楊仕榮、馬育萱、王威任、林佳文、張淑敏、王俊剛、陳仕妍、黃宗吟、黃俊音、王志成、林郁人、王偉傑、張麗芬、郭欣怡、謝明淑、彭靜芳、吳淑華、周志星、黃美堅、盧哲瑋、吳朝以、張惠文、林秀玲、韓國偉、許靜宜、郭進瑤、符佩琪、張庭瑋、周聖凱";
            List<string> all姓名 = new List<string>();
            var allNames = nameString.Split("、");
            foreach (var item in allNames)
            {
                all姓名.Add(item);
            }
            return all姓名;
        }
    }
}
