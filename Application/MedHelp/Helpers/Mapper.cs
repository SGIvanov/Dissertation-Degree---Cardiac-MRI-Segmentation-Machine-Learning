using MedHelp.Infrastructure.Models;
using MedHelp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MedHelp.Helpers
{
    public class Mapper
    {
        public MRIImageViewModel MapMRIDtoEntityToViewModel(MRIImage image)
        {
            return new MRIImageViewModel
            {
                FullScanId = image.FullScanId,
                Id = image.Id,
                Name = image.Name,
                UploadedDate = image.UploadedDate
            };
        }

        public IList<MRIImageViewModel> MapMRIDtoListToViewModel(IList<MRIImage> images)
        {
            return images.Select(MapMRIDtoEntityToViewModel).ToList();
        }

        public MRIImage MapMRIViewModelEntityToDto(MRIImageViewModel viewModel)
        {
            return new MRIImage
            {
                FullScanId = viewModel.FullScanId,
                Id = viewModel.Id,
                Name = viewModel.Name,
                UploadedDate = viewModel.UploadedDate
            };
        }

        public IList<MRIImage> MapMRIViewModelListToDto(IList<MRIImageViewModel> viewModels)
        {
            return viewModels.Select(MapMRIViewModelEntityToDto).ToList();
        }
    }
}