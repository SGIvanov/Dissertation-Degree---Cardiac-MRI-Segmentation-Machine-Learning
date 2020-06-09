using MedHelp.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MedHelp.Infrastructure
{
    public class MRIImageApplicationService
    {
        private Entities db;

        public MRIImageApplicationService()
        {
            db = new Entities();
        }

        public async Task<IList<MRIImage>> GetAllMRIImages()
        {
            return await db.MRIImages.ToListAsync();
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

        public bool ExecuteSegmentation(MRIImage file)
        {
            var processInfo = new ProcessStartInfo("cmd.exe", @"/c D:\Dissertation\Application\MedHelp\Commands\HeartMri.bat")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            var process = Process.Start(processInfo);

            process.Start();

            process.WaitForExit(120000);
            if (process.HasExited)
            {
                process.Close();
                return true;
            }
            return false;
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

        public async Task<MRIImage> SaveImageForSegmentation(HttpPostedFileBase file, string algorithmPath, string localPath)
        {
            var mriParentImage = new MRIImage
            {
                Name = Path.GetFileName(file.FileName),
                UploadedDate = DateTime.Now,
                Image = "placeholder"
            };
            var segmentPath = Path.Combine(algorithmPath, "testing_axial_full_pat10.nii.gz");
            file.SaveAs(segmentPath);
            await SaveMri(mriParentImage);

            var localSavePath = Path.Combine(localPath, $"{mriParentImage.Id}.nii.gz");
            mriParentImage.Image = localSavePath;
            await EditMri(mriParentImage);
            file.SaveAs(localSavePath);

            //Pre save segmentation
            var mriSegmentImage = new MRIImage
            {
                Name = $"Hearth - {mriParentImage.Name}",
                FullScanId = mriParentImage.Id,
                UploadedDate = null,
                Image = "placeholder"
            };
            await SaveMri(mriSegmentImage);

            return mriParentImage;
        }

        public async Task SaveSegmentedImage(MRIImage parentImage, string resultPath, string localPath)
        {
            var mriSegmentImage = new MRIImage
            {
                Name = $"Hearth - {parentImage.Name}",
                FullScanId = parentImage.Id,
                UploadedDate = DateTime.Now,
                Id = parentImage.MRIImage1.First().Id
            };
            using (db = new Entities())
            {
                var filePath = Path.Combine(resultPath, "window_seg_pat10__segmentation_niftynet.nii.gz");
                byte[] stream = File.ReadAllBytes(filePath);

                var newLocalPath = Path.Combine(localPath, $"{mriSegmentImage.Id}.nii.gz");
                mriSegmentImage.Image = newLocalPath;
                await EditMri(mriSegmentImage);
                File.WriteAllBytes(newLocalPath, stream);
            }
        }
    }
}
