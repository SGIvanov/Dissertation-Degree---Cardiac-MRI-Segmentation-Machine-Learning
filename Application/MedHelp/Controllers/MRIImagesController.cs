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
            MRIImage mRIImage = await db.MRIImages.FindAsync(id);
            if (mRIImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.FullScanId = new SelectList(db.MRIImages, "Id", "Name", mRIImage.FullScanId);
            return View(mRIImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,UploadedDate,FullScanId,Image")] MRIImage mRIImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mRIImage).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.FullScanId = new SelectList(db.MRIImages, "Id", "Name", mRIImage.FullScanId);
            return View(mRIImage);
        }

        public async Task<ActionResult> Delete(decimal id)
        {
            MRIImage mRIImage = await db.MRIImages.FindAsync(id);
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
            MRIImage mRIImage = await db.MRIImages.FindAsync(id);
            db.MRIImages.Remove(mRIImage);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
