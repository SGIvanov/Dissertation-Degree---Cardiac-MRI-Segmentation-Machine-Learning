using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,UploadedDate")] MRIImageViewModel mRIImage)
        {
            if (ModelState.IsValid)
            {
                var mriDto = Mapper.MapMRIViewModelEntityToDto(mRIImage);
                await mriImageService.SaveMri(mriDto);
                return RedirectToAction("Index");
            }

            return View(mRIImage);
        }

        public async Task<ActionResult> Edit(decimal id)
        {
            var mriDto = await mriImageService.FindMRIById(id);
            var mriView = Mapper.MapMRIDtoEntityToViewModel(mriDto);
            if (mriView == null)
            {
                return HttpNotFound();
            }
            return View(mriView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,UploadedDate")] MRIImageViewModel mRIImage)
        {
            if (ModelState.IsValid)
            {
                var mriDto = Mapper.MapMRIViewModelEntityToDto(mRIImage);
                await mriImageService.EditMri(mriDto);
                return RedirectToAction("Index");
            }

            return View(mRIImage);
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
