using System;

namespace MedHelp.Models
{
    public class MRIResult
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public DateTime UploadDate { get; set; }

        public string ImageBase64 { get; set; }
    }
}