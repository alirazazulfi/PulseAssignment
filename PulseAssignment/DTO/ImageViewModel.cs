using System;
using System.Web;

namespace PulseAssignment.DTO
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageName { get; set; }
        public string ImageText { get; set; }
        public DateTime DateConverted { get; set; }
        public byte[] ImageByte { get; set; }
        public HttpPostedFileWrapper ImageFile { get; set; }
    }
}