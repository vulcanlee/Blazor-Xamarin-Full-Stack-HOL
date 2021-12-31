using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi.CommonApiServices
{
    /// <summary>
    /// Dictionary<String, String>的資料型態，主要用於 Http的 Get & Post 的傳遞資料儲存地方
    /// </summary>
    public class WebQueryDictionary : Dictionary<string, string>
    {

        /// <summary>
        /// 傳遞屬性的Lambda表示式，自動取出該屬性的名稱與值出來，並且加入到Dictionary內
        /// </summary>
        /// <typeparam name="T">傳遞屬性變數的資料類型</typeparam>
        /// <param name="expr">Lambda表示式，該屬性名稱即為要傳遞的參數名稱</param>
        public void AddItem<T>(Expression<Func<T>> expr)
        {
            string propertyName = ((MemberExpression)expr.Body).Member.Name;
            T propertyValue = expr.Compile()();
            this.Add(propertyName, propertyValue.ToString());
        }

        /// <summary>
        /// 傳遞屬性的Lambda表示式(無須指定屬性的資料型別)，自動取出該屬性的名稱與值出來，並且加入到Dictionary內
        /// </summary>
        /// <param name="expr">Lambda表示式，該屬性名稱即為要傳遞的參數名稱</param>
        public void AddStringItem(Expression<Func<object>> expr)
        {
            string propertyName = ((MemberExpression)expr.Body).Member.Name;
            object propertyValue = expr.Compile()();
            this.Add(propertyName, propertyValue.ToString());
        }
        public void AddStringItem(string propertyName, string propertyValue)
        {
            this.Add(propertyName, propertyValue.ToString());
        }
    }
}
