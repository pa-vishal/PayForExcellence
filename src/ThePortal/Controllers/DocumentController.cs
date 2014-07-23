using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using FolderExplorer.Models;

namespace ThePortal.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        public ActionResult DashBoard()
        {
            ViewBag.Message = "You can do the following:";
            return View();
        }

        public ActionResult Explore()
        {
            ViewBag.Message = "Your Documents Here: ";
            
            var realPath = Path.Combine(
                ConfigurationManager.AppSettings.Get("FileUploadPath"), User.Identity.Name);

            if (System.IO.File.Exists(realPath))
            {
                //http://stackoverflow.com/questions/1176022/unknown-file-type-mime
                return base.File(realPath, "application/octet-stream");
            }

            if (Directory.Exists(realPath))
            {
                var dirList = Directory.EnumerateDirectories(realPath);
                
                var dirListModel = (from dir in dirList let d = new DirectoryInfo(dir) 
                                    select new DirModel {DirName = Path.GetFileName(dir), DirAccessed = d.LastAccessTime}).ToList();
                
                var fileListModel = new List<FileModel>();

                var fileList = Directory.EnumerateFiles(realPath);
                foreach (var file in fileList)
                {
                    var f = new FileInfo(file);

                    var fileModel = new FileModel();

                    if (f.Extension.ToLower() != "php" && f.Extension.ToLower() != "aspx"
                        && f.Extension.ToLower() != "asp")
                    {
                        fileModel.FileName = Path.GetFileName(file);
                        fileModel.FileAccessed = f.LastAccessTime;
                        fileModel.FileSizeText = (f.Length < 1024) ? f.Length + " B" : f.Length / 1024 + " KB";

                        fileListModel.Add(fileModel);
                    }
                }

                var explorerModel = new ExplorerModel(dirListModel, fileListModel);

                return View("ViewDocuments", explorerModel);
            }
            return View("DashBoard");
        }

        public ActionResult UploadDashboard()
        {
            return View("UploadDocuments");
        }

        [HttpPost]
        public ActionResult Upload()
        {
            var folderName = Request.Form["folderName"];
            
            foreach (string file in Request.Files)
            {
                var postedFile = Request.Files[file];
                if (postedFile.ContentLength == 0)
                    continue;

                var savedFileName = Path.Combine(
                    ConfigurationManager.AppSettings.Get("FileUploadPath"), User.Identity.Name, folderName,
                    Path.GetFileName(postedFile.FileName));
                
                CheckCreateUserDir(savedFileName);

                postedFile.SaveAs(savedFileName);
            }
            return View("DashBoard");
        }

        private static void CheckCreateUserDir(string savedFileName)
        {
            var folder = Path.GetDirectoryName(savedFileName);
            if(!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        }
    }
}
