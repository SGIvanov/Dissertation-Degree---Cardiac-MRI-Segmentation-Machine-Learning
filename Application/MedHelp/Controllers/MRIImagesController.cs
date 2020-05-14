using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MedHelp.Helpers;
using MedHelp.Infrastructure;
using MedHelp.Infrastructure.Models;
using MedHelp.Models;

namespace MedHelp.Controllers
{
    public class MRIImagesController : Controller
    {
        readonly MRIImageApplicationService mriImageService = new MRIImageApplicationService();

        // GET: MRIImages
        public async Task<ActionResult> Index()
        {
            var mRIDtoImages = await mriImageService.GetSegmentationMRIImages();
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
        public FileContentResult Create(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var mriImage = new MRIImage
                {
                    Name = Path.GetFileName(file.FileName),
                    UploadedDate = DateTime.Now
                };
                var path = Path.Combine(Server.MapPath("~/Commands/Algorithm/testing"), "testing_axial_full_pat10.nii.gz");
                file.SaveAs(path);
                var bytes = System.IO.File.ReadAllBytes(path);
                mriImage.Image = Convert.ToBase64String(bytes);

                var resultFileName = mriImageService.ExecuteSegmentation(mriImage);

                if (!string.IsNullOrEmpty(resultFileName))
                {
                    var fileName = Server.MapPath("~/Commands/Algorithm/models/testing/" + resultFileName);
                    var mimeType = MimeMapping.GetMimeMapping(fileName);
                    byte[] stream = System.IO.File.ReadAllBytes(fileName);
                    return File(stream, mimeType, mriImage.Name + " Segmented");
                }
                return null;
            }

            return null;
        }

        public async Task<ActionResult> Delete(decimal id)
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
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            MRIImage mRIImage = await mriImageService.FindMRIById(id);
            await mriImageService.DeleteMri(mRIImage);
            return RedirectToAction("Index");
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
