using PulseAssignment.Models;
using System.Collections.Generic;
using System.Linq;
namespace PulseAssignment.BLL
{
    public class FileRepository
    {
        PulseAssignmentEntities db = new PulseAssignmentEntities();
        public List<ImageFile> GetAllImages()
        {
            return db.ImageFiles.ToList();
        }
        public ImageFile GetImagesById(int? id)
        {
            return db.ImageFiles.SingleOrDefault(x => x.Id == id);
        }
        public bool SaveImage(ImageFile imageFile)
        {
            db.ImageFiles.Add(imageFile);
            db.SaveChanges();
            return true;
        }
    }
}