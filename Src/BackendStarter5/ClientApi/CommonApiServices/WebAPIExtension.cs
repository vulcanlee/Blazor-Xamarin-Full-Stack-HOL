using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi.CommonApiServices
{
    public static class WebAPIExtension
    {
        /// <summary>
        /// 取得變數名稱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string getName(Expression<Func<string>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        }

        public static string getStringValue(Expression<Func<string>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        }

        /// <summary>
        /// 取得變數名稱
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string getName(Expression<Func<int>> expr)
        {
            return ((MemberExpression)expr.Body).Member.Name;
        }

        /// <summary>
        /// 取得呼叫的方法名稱
        /// </summary>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static string getCallerName([CallerMemberName] string caller = "")
        {
            return caller;
        }

        /// <summary>
        /// Get使用(將字典轉換成QueryString)
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string ToQueryString(this Dictionary<string, string> dic)
        {
            string queryString = "?";

            foreach (string key in dic.Keys)
            {
                queryString += key + "=" + dic[key] + "&";
            }

            queryString = queryString.Remove(queryString.Length - 1, 1);

            return queryString;
        }

        /// <summary>
        /// Post使用(將字典轉換成Multipart)
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static MultipartFormDataContent ToMultipartFormDataContent(this Dictionary<string, string> dic)
        {
            MultipartFormDataContent form = new MultipartFormDataContent();

            foreach (string key in dic.Keys)
            {
                form.Add(new StringContent(dic[key]), String.Format("\"{0}\"", key));
            }

            return form;
        }

        /// <summary>
        /// Post使用(將字典轉換成 FormUrlEncoded)
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static FormUrlEncodedContent ToFormUrlEncodedContent(this Dictionary<string, string> dic)
        {
            FormUrlEncodedContent form = new FormUrlEncodedContent(dic);

            return form;
        }
    }
}
