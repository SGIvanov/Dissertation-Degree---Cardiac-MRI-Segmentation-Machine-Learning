using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MedHelp.Models
{
    public class MRIImageViewModel
    {
        public int Id { get; set; }

        [Display(Name = "File Name")]
        public string Name { get; set; }

        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; }

        public decimal? FullScanId { get; set; }

        public string Image { get; set; }

        [Display(Name = "Full MRI Name")]
        public string FullScanName { get; set; }
    }
}