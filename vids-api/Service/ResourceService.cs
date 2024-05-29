using Vids.Model;

namespace Vids.Service
{
    public class ResourceService
    {
        public static string GetFirstImage(string resourceFolder, string ownerId, string directoryName, string itemId)
        {
            var folderPath = Path.Combine(resourceFolder, ownerId, directoryName, itemId);
            var directoryInfo = Directory.CreateDirectory(folderPath);
            var firstLogo = directoryInfo.GetFiles().Where((f) =>
            f.Extension == ".jpg" ||
            f.Extension == ".jpeg" ||
            f.Extension == ".png" ||
            f.Extension == ".svg"
            ).FirstOrDefault();

            var relativeFolderPath = Path.Combine("resources", ownerId, directoryName, itemId);

            if (firstLogo != null)
            {
                var logoPath = Path.Combine(relativeFolderPath, firstLogo.Name);
                return logoPath;
            }
            else
            {
                return string.Empty;
            }

        }

        public static async Task<Result<string>> SaveFileAsync(string resourceFolder, string ownerId, string directoryName, string itemId, IFormFile file)
        {
            try
            {
                var folderPath = Path.Combine(resourceFolder, ownerId, directoryName, itemId);
                Directory.CreateDirectory(folderPath);

                if (file.Length > 0)
                {
                    string filePath = Path.Combine(folderPath, file.FileName);
                    using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    string relativePath = Path.Combine("resources",ownerId, directoryName, itemId, file.FileName);
                    return new Result<string>() { Data = relativePath };
                }
                else
                {
                    return new Result<string>() { Status = ResultStatusValues.Error, Message = "File length is 0." };
                }

            }
            catch (Exception ex)
            {
                return new Result<string>
                {
                    Status = ResultStatusValues.Error,
                    Message = ex.Message + "#" + ex.InnerException?.Message
                };
            }
        }

        public static void DeleteFolder(string resourceFolder, string ownerId, string directoryName, string itemId)
        {
            var folderPath = Path.Combine(resourceFolder, ownerId, directoryName, itemId);
            if (Directory.Exists(folderPath))
                Directory.Delete(folderPath, true);
        }

        public static void DeleteFile(string resourceFolder, string ownerId, string directoryName, string itemId, string fileName)
        {
            var folderPath = Path.Combine(resourceFolder, ownerId, directoryName, itemId);
            string filePath = Path.Combine(folderPath, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

    }
}
