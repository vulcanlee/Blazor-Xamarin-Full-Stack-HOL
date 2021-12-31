using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using BAL.Helpers;

namespace Backend.Helpers
{
    public static class UploadImageHelper
    {
        public static async Task PrepareUploadImageFolder(IConfiguration configuration)
        {
            SetImageRootPath(configuration);
            SetTempImageRootPath(configuration);
            await PrepareDirectoryAsync(MagicHelper.RootImageFolder);
            await PrepareDirectoryAsync(MagicHelper.RootTempImageFolder);
            for (int fooDirectoryIndex = 0; fooDirectoryIndex < MagicHelper.TotalImageFolder; fooDirectoryIndex++)
            {
                string fooDirectoryString = fooDirectoryIndex.ToString("D3");
                string fullPath = Path.Combine(MagicHelper.RootImageFolder, fooDirectoryString);
                await PrepareDirectoryAsync(fullPath);
            }
        }
        public static void SetImageRootPath(IConfiguration configuration)
        {
            string currentDirectory = configuration[AppSettingHelper.UploadImagePath];
            string path = Path.Combine(currentDirectory, MagicHelper.ImageFolderName);
            MagicHelper.RootImageFolder = path;
        }
        public static void SetTempImageRootPath(IConfiguration configuration)
        {
            string currentDirectory = configuration[AppSettingHelper.UploadImagePath];
            string path = Path.Combine(currentDirectory, MagicHelper.ImageFolderName,
                MagicHelper.ImageTempFolderName);
            MagicHelper.RootTempImageFolder = path;
        }
        public static async Task PrepareDirectoryAsync(string path)
        {
            if (Directory.Exists(path) == false)
            {
                await Task.Run(() =>
                {
                    Directory.CreateDirectory(path);
                });
            }
        }
        public static string GetImageFilename(string image)
        {
            return $"{image}.jpg";
        }
        public static string GetWebImageFilename(string image)
        {
            return $"{image}-Web.jpg";
        }
        public static async Task RemoveImageAsync(string folder, string filename, string extension)
        {
            string pathProduction = Path.Combine(MagicHelper.RootImageFolder, folder, $"{filename}.{extension}");
            await Task.Run(() =>
            {
                var fi = new FileInfo(pathProduction);
                fi.Delete();
            });
            return;
        }
        public static async Task<(string folder, string filename)> MoveImageToProduction(string tempfilename)
        {
            string pathTemp = Path.Combine(MagicHelper.RootTempImageFolder, tempfilename);
            var fi = new FileInfo(pathTemp);
            string newFilenameGuid = Guid.NewGuid().ToString();
            string newFilename = $"{newFilenameGuid}{fi.Extension}";
            int fooDirectoryIndex = (int)(DateTime.Now.Millisecond % MagicHelper.TotalImageFolder);
            string fooDirectoryString = fooDirectoryIndex.ToString("D3");
            string productionPath = Path.Combine(MagicHelper.RootImageFolder, fooDirectoryString);
            string productionImagePath = Path.Combine(productionPath, GetImageFilename(newFilenameGuid));
            await Task.Run(() =>
            {
                using (Image image = Image.Load(pathTemp))
                {
                    image.Save(productionImagePath, new JpegEncoder());
                }
                fi.Delete();
            });

            return (fooDirectoryString, Path.GetFileName(productionImagePath));
        }
        public static async Task<(string folder, string filename, string extension)>
            MoveImage2Production(string filename, string extension)
        {
            string pathTemp = Path.Combine(MagicHelper.RootTempImageFolder, $"{filename}.{extension}");
            var fi = new FileInfo(pathTemp);
            string newFilenameGuid = Guid.NewGuid().ToString();
            string newFilename = $"{newFilenameGuid}{fi.Extension}";
            int fooDirectoryIndex = (int)(DateTime.Now.Millisecond % MagicHelper.TotalImageFolder);
            string fooDirectoryString = fooDirectoryIndex.ToString("D3");
            string productionPath = Path.Combine(MagicHelper.RootImageFolder, fooDirectoryString);
            string pathProduction = Path.Combine(productionPath, GetImageFilename(newFilenameGuid));
            await Task.Run(() =>
            {
                using (Image image = Image.Load(pathTemp))
                {
                    image.Save(pathProduction, new JpegEncoder());
                }
                fi.Delete();
            });

            return (fooDirectoryString, newFilenameGuid, "jpg");
        }
        public static async Task Resize(IConfiguration configuration, string filename)
        {
            string path = Path.Combine(MagicHelper.RootImageFolder, filename);
            var fi = new FileInfo(path);
            string destinationFilename = fi.Name;
            string pathWeb = Path.Combine(MagicHelper.RootImageFolder,
                $"{destinationFilename}{MagicHelper.ImageForWebPostfix}.jpg");
            using (Image image = Image.Load(path))
            {
                // Resize the image in place and return it for chaining.
                // 'x' signifies the current image processing context.
                image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));

                // The library automatically picks an encoder based on the file extension then
                // encodes and write the data to disk.
                // You can optionally set the encoder to choose.
                await image.SaveAsync(pathWeb, new JpegEncoder());
            } // Dispose - releasing memory into a memory pool ready for the next image you wish to process.
        }
        public static string GetImageUrl(string folder, string filename, string fileextension)
        {
            return $"/{MagicHelper.ImageFolderName}/{folder}/{filename}.{fileextension}";
        }
        public static async Task RemoveProductionImageAsync(string folder, string filename, string fileExtension)
        {
            string productionImagePath = Path.Combine(MagicHelper.RootImageFolder, folder, $"{filename}.{fileExtension}");
            await Task.Run(() =>
            {
                File.Delete(productionImagePath);
            });

            return ;
        }
    }
}
