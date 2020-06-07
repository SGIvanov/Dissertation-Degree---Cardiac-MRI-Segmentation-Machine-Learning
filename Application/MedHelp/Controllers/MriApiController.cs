using MedHelp.Infrastructure;
using System;
using System.IO;
using System.Web.Http;

namespace MedHelp.Controllers
{
    [Route("api/MriApi")]
    public class MriApiController : ApiController
    {
        readonly MRIImageApplicationService mriImageService = new MRIImageApplicationService();

        [HttpGet]
        public string GetMRI(int id)
        {
            var fullPath = System.Web.Hosting.HostingEnvironment.MapPath($@"~/data/{id}.nii.gz");
            return mriImageService.LoadMriFromPath(fullPath);
        }
    }
}
