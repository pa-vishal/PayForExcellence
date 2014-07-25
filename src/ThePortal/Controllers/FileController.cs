using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ElFinder;

namespace ThePortal.Controllers
{
    public partial class FileController : Controller
    {
        public virtual ActionResult Index(string folder, string subFolder)
        {
            var driver = new FileSystemDriver();
            var uploadPath = GetFileUploadRootPath();

            var root = new Root(
                    new DirectoryInfo(Path.Combine( uploadPath ,subFolder)),
                    "http://" + Request.Url.Authority + "/Files/" + subFolder)
            {
                // Sample using ASP.NET built in Membership functionality...
                // Only the super user can READ (download files) & WRITE (create folders/files/upload files).
                // Other users can only READ (download files)
                // IsReadOnly = !User.IsInRole(AccountController.SuperUser)

                IsReadOnly = false, // Can be readonly according to user's membership permission
                Alias = "Files", // Beautiful name given to the root/home folder
                MaxUploadSizeInKb = 500, // Limit imposed to user uploaded file <= 500 KB
                LockedFolders = new List<string>( new string[] { "Folder1"})
            };

            // Was a subfolder selected in Home Index page?
            if (!string.IsNullOrEmpty(subFolder))
            {
                root.StartPath = new DirectoryInfo(Path.Combine(uploadPath, subFolder));
            }

            driver.AddRoot(root);

            var connector = new Connector(driver);

            return connector.Process(this.HttpContext.Request);
        }

        public virtual ActionResult SelectFile(string target)
        {
            FileSystemDriver driver = new FileSystemDriver();
            var uploadPath = GetFileUploadRootPath();
            driver.AddRoot(
                new Root(
                    new DirectoryInfo(uploadPath),
                    "http://" + Request.Url.Authority + "/Files") { IsReadOnly = false });

            var connector = new Connector(driver);

            return Json(connector.GetFileByHash(target).FullName);
        }

        private string GetFileUploadRootPath()
        {
            var userName = string.Empty;
            if (User.Identity != null)
            {
                userName = User.Identity.Name;
            }
            var path = Path.Combine(ConfigurationManager.AppSettings.Get("FileUploadRootPath"), userName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;

        }
    }
}
