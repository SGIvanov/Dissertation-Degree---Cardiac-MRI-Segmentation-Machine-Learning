using MedHelp.Helpers;
using MedHelp.Infrastructure;
using MedHelp.Infrastructure.Models;
using MedHelp.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace MedHelp.Tests
{
    [TestClass]
    public class MriTests
    {
        [TestMethod]
        public void MriAPi_Base64Image_ImageIsFound()
        {
            //Arrange
            var mriService = new MRIImageApplicationService();
            var mriPath = "D:\\Dissertation\\Application\\MedHelp\\data\\4.nii.gz";

            //Act
            var base64Image = mriService.LoadMriFromPath(mriPath);

            //Assert
            Assert.IsNotNull(base64Image);
            Assert.IsTrue(!string.IsNullOrEmpty(base64Image));
        }

        [TestMethod]
        public void MriAPi_Base64Image_ImageIsNotFound()
        {
            //Arrange
            var mriService = new MRIImageApplicationService();
            var mriPath = "D:\\Dissertation\\Application\\MedHelp\\data\\-1.nii.gz";

            //Act

            //Assert
            Assert.ThrowsException<FileNotFoundException>(
                () =>
                {
                    mriService.LoadMriFromPath(mriPath);
                });
        }

        [TestMethod]
        public void MriMapper_MRIImageViewModel_MRIImageDto()
        {
            //Arrange
            var mriDto = new MRIImage()
            {
                Id = 1,
                FullScanId = 0,
                Image = "test",
                Name = "image1",
                UploadedDate = DateTime.Now
            };

            //Act
            var mriView = Mapper.MapMRIDtoEntityToViewModel(mriDto);

            //Assert
            Assert.AreEqual(mriDto.Id, mriView.Id);
            Assert.AreEqual(mriDto.FullScanId, mriView.FullScanId);
            Assert.AreEqual(mriDto.Name, mriView.Name);
            Assert.AreEqual(mriDto.UploadedDate, mriView.UploadedDate);

        }

        [TestMethod]
        public void MriMapper_MRIImageDto_MRIImageViewModel()
        {
            //Arrange
            var mriView = new MRIImageViewModel()
            {
                Id = 1,
                FullScanId = 0,
                Name = "image1",
                UploadedDate = DateTime.Now
            };

            //Act
            var mriDto = Mapper.MapMRIViewModelEntityToDto(mriView);

            //Assert
            Assert.AreEqual(mriDto.Id, mriView.Id);
            Assert.AreEqual(mriDto.FullScanId, mriView.FullScanId);
            Assert.AreEqual(mriDto.Name, mriView.Name);
            Assert.AreEqual(mriDto.UploadedDate, mriView.UploadedDate);
        }
    }
}
