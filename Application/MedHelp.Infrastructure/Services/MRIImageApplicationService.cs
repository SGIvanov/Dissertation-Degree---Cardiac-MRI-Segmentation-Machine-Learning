using MedHelp.Infrastructure.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MedHelp.Infrastructure
{
    public class MRIImageApplicationService
    {
        private readonly MRIEntities db;

        public MRIImageApplicationService()
        {
            db = new MRIEntities();
        }

        public async Task<IList<MRIImage>> GetAllMRIImages()
        {
            return await db.MRIImages.ToListAsync();
        }

        public async Task<IList<MRIImage>> GetSegmentationMRIImages()
        {
            return await db.MRIImages.Where(m => m.FullScanId != null).ToListAsync();
        }

        public async Task<MRIImage> FindMRIById(decimal id)
        {
            return await db.MRIImages.FindAsync(id);
        }

        public async Task SaveMri(MRIImage image)
        {
            db.MRIImages.Add(image);
            await db.SaveChangesAsync();
        }

        public async Task EditMri(MRIImage image)
        {
            db.Entry(image).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public async Task DeleteMri(MRIImage image)
        {
            db.MRIImages.Remove(image);
            await db.SaveChangesAsync();
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
        }
    }
}
