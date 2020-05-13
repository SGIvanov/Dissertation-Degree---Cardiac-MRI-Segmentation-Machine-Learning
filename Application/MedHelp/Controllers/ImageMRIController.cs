using System;
using System.IO;
using System.Web.Http;

namespace MedHelp.Controllers
{
    [Route("api/ImageMRI")]
    public class ImageMRIController : ApiController
    {
        [HttpGet]
        public string GetMRI(string id)
        {
            var fullPath = System.Web.Hosting.HostingEnvironment.MapPath($@"~/data/{id}.nii.gz");
            var bytes = File.ReadAllBytes(fullPath);
            return Convert.ToBase64String(bytes);
        }
    }
}
