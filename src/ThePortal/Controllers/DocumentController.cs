using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using ThePortal.ViewModels;

namespace ThePortal.Controllers
{
    [Authorize]
    public partial class DocumentController : Controller
    {
        public virtual ActionResult Dashboard()
        {
            var path = GetFileUploadRootPath();

            DirectoryInfo di = new DirectoryInfo(path);
            // Enumerating all 1st level directories of a given root folder (MyFolder in this case) and retrieving the folders names.
            var folders = di.GetDirectories().ToList().Select(d => d.Name);

            return View(folders);
        }

        private string GetFileUploadRootPath()
        {
            var userName = string.Empty;
            if (User.Identity != null)
            {
                userName = User.Identity.Name;
            }
            var path = Path.Combine(ConfigurationManager.AppSettings.Get("FileUploadRootPath"), userName);
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            
        }

        [GET("FileManager/{subFolder?}")]
        public virtual ActionResult Files(string subFolder)
        {        // FileViewModel contains the root MyFolder and the selected subfolder if any
            var model = new FileViewModel
            {
                Folder = GetFileUploadRootPath(),
                SubFolder = subFolder
            };

            return View(model);
        }

    }
}
