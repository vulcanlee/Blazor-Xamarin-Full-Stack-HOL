using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApi.CommonApiServices
{
    /// <summary>
    /// Storage 相關的 API
    /// http://www.nudoq.org/#!/Packages/PCLStorage/PCLStorage/FileSystem
    /// </summary>
    public class StorageUtility
    {
        /// <summary>
        /// 將所指定的字串寫入到指定目錄的檔案內
        /// </summary>
        /// <param name="folderName">目錄名稱</param>
        /// <param name="filename">檔案名稱</param>
        /// <param name="content">所要寫入的文字內容</param> 
        /// <returns></returns>
        public static async Task WriteToDataFileAsync(string folderName, string filename, string content)
        {
            string rootPath = Directory.GetCurrentDirectory();

            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            try
            {
                #region 建立與取得指定路徑內的資料夾
                string fooPath = Path.Combine(rootPath, folderName);
                if (Directory.Exists(fooPath) == false)
                {
                    Directory.CreateDirectory(fooPath);
                }
                fooPath = Path.Combine(fooPath, filename);
                #endregion

                byte[] encodedText = Encoding.UTF8.GetBytes(content);

                using (FileStream sourceStream = new FileStream(fooPath,
                    FileMode.Create, FileAccess.Write, FileShare.None,
                    bufferSize: 4096, useAsync: true))
                {
                    await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
            }
        }

        /// <summary>
        /// 從指定目錄的檔案內將文字內容讀出
        /// </summary>
        /// <param name="folderName">目錄名稱</param>
        /// <param name="filename">檔案名稱</param>
        /// <returns>文字內容</returns>
        public static async Task<string> ReadFromDataFileAsync(string folderName, string filename)
        {
            string content = "";
            string rootPath = Directory.GetCurrentDirectory();

            if (string.IsNullOrEmpty(folderName))
            {
                throw new ArgumentNullException(nameof(folderName));
            }

            if (string.IsNullOrEmpty(filename))
            {
                throw new ArgumentNullException(nameof(filename));
            }

            try
            {
                #region 建立與取得指定路徑內的資料夾
                string fooPath = Path.Combine(rootPath, folderName);
                if (Directory.Exists(fooPath) == false)
                {
                    Directory.CreateDirectory(fooPath);
                }
                fooPath = Path.Combine(fooPath, filename);
                #endregion

                if (File.Exists(fooPath) == false)
                {
                    return content;
                }

                using (FileStream sourceStream = new FileStream(fooPath,
                    FileMode.Open, FileAccess.Read, FileShare.Read,
                    bufferSize: 4096, useAsync: true))
                {
                    StringBuilder sb = new StringBuilder();

                    byte[] buffer = new byte[0x1000];
                    int numRead;
                    while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        string text = Encoding.UTF8.GetString(buffer, 0, numRead);
                        sb.Append(text);
                    }

                    content = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
            }

            return content.Trim();
        }
    }
}
