using System;
using System.ComponentModel.DataAnnotations;

namespace MedHelp.Models
{
    public class MRIImageViewModel
    {
        public decimal Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; }
        public decimal? FullScanId { get; set; }
        public string Image { get; set; }
    }
}