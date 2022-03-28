using ICSharpCode.SharpZipLib.Zip;
using PulseAssignment.BLL;
using PulseAssignment.DTO;
using PulseAssignment.Models;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PulseAssignment.Controllers
{
    public class HomeController : Controller
    {
        FileRepository _repo = new FileRepository();

        [HttpGet]
        public ActionResult Index()
        {
            return View(_repo.GetAllImages());
        }

        public ActionResult GetList()
        {
            var list = _repo.GetAllImages();
            return PartialView("_ImagesList", list);
        }

        public ActionResult Create()
        {
            return View();
        }

        public JsonResult ImageUpload(ImageViewModel model)
        {
            var file = model.ImageFile;
            if (file != null)
            {
                BinaryReader reader = new BinaryReader(file.InputStream);
                byte[] imagebyte = reader.ReadBytes(file.ContentLength);
                ImageFile img = new ImageFile();
                img.Title = model.Title;
                img.ImageText = imagebyte;
                img.ImageName = file.FileName;
                img.DateConverted = DateTime.UtcNow;
                _repo.SaveImage(img);
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayingImage(int? id)
        {
            var img = _repo.GetImagesById(id);
            var fileName = string.Format("{0}_ImageFiles.zip", DateTime.Today.Date.ToString("dd-MM-yyyy") + "_1");
            string folderpath = Server.MapPath(Url.Content("/TempImages"));
            string tempOutPutPath = folderpath + "/" + fileName;
            bool exists = Directory.Exists(folderpath);
            if (!exists) Directory.CreateDirectory(folderpath);
            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  
                byte[] buffer = new byte[4096];
                ZipEntry entry = new ZipEntry(Path.GetFileName(img.ImageName))
                {
                    DateTime = DateTime.Now,
                    IsUnicodeText = true
                };
                s.PutNextEntry(entry);
                using (MemoryStream memory = new MemoryStream(img.ImageText))
                {
                    using (BinaryReader reader = new BinaryReader(memory))
                    {
                        for (int i = 0; i < img.ImageText.Length; i++)
                        {
                            s.WriteByte(reader.ReadByte());
                        }
                    }
                }
                s.Finish();
                s.Flush();
                s.Close();
            }
            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath)) System.IO.File.Delete(tempOutPutPath);
            if (finalResult == null || !finalResult.Any()) throw new Exception(String.Format("No Files found with Image"));
            return File(finalResult, "application/zip", fileName);
        }
    }
}