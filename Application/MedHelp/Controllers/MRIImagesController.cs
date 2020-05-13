using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MedHelp.Infrastructure.Models;

namespace MedHelp.Controllers
{
    public class MRIImagesController : Controller
    {
        private MRIEntities db = new MRIEntities();

        // GET: MRIImages
        public async Task<ActionResult> Index()
        {
            var mRIImages = db.MRIImages.Include(m => m.MRIImage2);
            return View(await mRIImages.ToListAsync());
        }

        // GET: MRIImages/Details/5
        public async Task<ActionResult> Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MRIImage mRIImage = await db.MRIImages.FindAsync(id);
            if (mRIImage == null)
            {
                return HttpNotFound();
            }
            return View(mRIImage);
        }

        // GET: MRIImages/Create
        public ActionResult Create()
        {
            ViewBag.FullScanId = new SelectList(db.MRIImages, "Id", "Name");
            return View();
        }

        // POST: MRIImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,UploadedDate,FullScanId,Image")] MRIImage mRIImage)
        {
            if (ModelState.IsValid)
            {
                db.MRIImages.Add(mRIImage);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.FullScanId = new SelectList(db.MRIImages, "Id", "Name", mRIImage.FullScanId);
            return View(mRIImage);
        }

        // GET: MRIImages/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MRIImage mRIImage = await db.MRIImages.FindAsync(id);
            if (mRIImage == null)
            {
                return HttpNotFound();
            }
            ViewBag.FullScanId = new SelectList(db.MRIImages, "Id", "Name", mRIImage.FullScanId);
            return View(mRIImage);
        }

        // POST: MRIImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: MRIImages/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MRIImage mRIImage = await db.MRIImages.FindAsync(id);
            if (mRIImage == null)
            {
                return HttpNotFound();
            }
            return View(mRIImage);
        }

        // POST: MRIImages/Delete/5
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
