using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MedHelp.Helpers;
using MedHelp.Infrastructure;
using MedHelp.Infrastructure.Models;

namespace MedHelp.Controllers
{
    public class MRIImagesController : Controller
    {
        readonly MRIImageApplicationService mriImageService = new MRIImageApplicationService();

        // GET: MRIImages
        public async Task<ActionResult> Index()
        {
            var mRIDtoImages = await mriImageService.GetAllMRIImages();
            var mRIViewModels = Mapper.MapMRIDtoListToViewModel(mRIDtoImages);
            return View(mRIViewModels);
        }

        public async Task<ActionResult> Details(decimal id)
        {
            var mRIDtoImage = await mriImageService.FindMRIById(id);
            var mRIViewModel = Mapper.MapMRIDtoEntityToViewModel(mRIDtoImage);
            if (mRIViewModel == null)
            {
                return HttpNotFound();
            }
            return View(mRIViewModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var mriParentImage =await mriImageService.SaveImageForSegmentation
                    (file,
                    Server.MapPath("~/Commands/Algorithm/testing"),
                    Server.MapPath("~/data"));

                _ =  Task.Run(async () =>
                  {
                      var successfulySegmented = mriImageService.ExecuteSegmentation(mriParentImage);
                      if (successfulySegmented)
                      {
                          await mriImageService.SaveSegmentedImage(
                              mriParentImage,
                              Server.MapPath("~/Commands/Algorithm/models/testing"),
                              Server.MapPath("~/data"));
                      }
                  });

                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<ActionResult> Delete(int id)
        {
            var mriDto = await mriImageService.FindMRIById(id);
            var mRIImage = Mapper.MapMRIDtoEntityToViewModel(mriDto);
            if (mRIImage == null)
            {
                return HttpNotFound();
            }
            return View(mRIImage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MRIImage mRIImage = await mriImageService.FindMRIById(id);
            await mriImageService.DeleteMri(mRIImage);
            return RedirectToAction("Index");
        }

        [HttpGet, ActionName("Download")]
        public async Task<FileContentResult> Download(int id)
        {
            var mRIDtoImage = await mriImageService.FindMRIById(id);
            if (mRIDtoImage == null)
            {
                return null;
            }
            var file = Server.MapPath($"~/data/{mRIDtoImage.Id}.nii.gz");
            var mimeType = MimeMapping.GetMimeMapping(file);
            byte[] stream = System.IO.File.ReadAllBytes(file);
            return File(stream, mimeType, mRIDtoImage.Name);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mriImageService.Dispose(disposing);
            }
            base.Dispose(disposing);
        }
    }
}
