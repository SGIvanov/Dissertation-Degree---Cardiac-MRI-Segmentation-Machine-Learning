using MedHelp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
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

        public async Task<MRIImage> FindMRIById(decimal id)
        {
            return await db.MRIImages.FindAsync(id);
        }

        public void SaveMri(MRIImage image)
        {
            db.MRIImages.Add(image);
            db.SaveChanges();
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

        public string ExecuteSegmentation(MRIImage file)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", @"/c D:\ITSGFork\StudProjects\team00\WebApp\MedHelp\Commands\HeartMri.bat")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            var process = Process.Start(processInfo);

            process.Start();

            process.WaitForExit();
            if (process.HasExited)
            {
                process.Close();
                return "window_seg_pat10__segmentation_niftynet.nii.gz";
            }
            return null;
        }

        public string LoadMriFromPath(string fullPath)
        {
            var bytes = File.ReadAllBytes(fullPath);
            var base64 = Convert.ToBase64String(bytes);
            if (string.IsNullOrEmpty(base64))
            {
                throw new FileNotFoundException();
            }
            return base64;
        }
    }
}
