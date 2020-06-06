using MedHelp.Infrastructure.Models;
using MedHelp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedHelp.Helpers
{
    public static class Mapper
    {
        public static MRIImageViewModel MapMRIDtoEntityToViewModel(MRIImage image)
        {
            return new MRIImageViewModel
            {
                FullScanId = image.FullScanId,
                Id = (int)image.Id,
                Name = image.Name,
                UploadedDate = image.UploadedDate,
                FullScanName = image.MRIImage2?.Name
            };
        }

        public static IList<MRIImageViewModel> MapMRIDtoListToViewModel(IList<MRIImage> images)
        {
            return images.Select(MapMRIDtoEntityToViewModel).ToList();
        }

        public static MRIImage MapMRIViewModelEntityToDto(MRIImageViewModel viewModel)
        {
            return new MRIImage
            {
                FullScanId = viewModel.FullScanId,
                Id = viewModel.Id,
                Name = viewModel.Name,
                UploadedDate = viewModel.UploadedDate
            };
        }

        public static IList<MRIImage> MapMRIViewModelListToDto(IList<MRIImageViewModel> viewModels)
        {
            return viewModels.Select(MapMRIViewModelEntityToDto).ToList();
        }
    }
}