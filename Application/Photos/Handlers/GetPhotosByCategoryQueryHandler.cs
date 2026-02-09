using AutoMapper;
using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.Application.Photos.Queries;
using MyPortfolio.DTOs;
using MyPortfolio.Infrastructure.Repositories;

namespace MyPortfolio.Application.Photos.Handlers
{
    public class GetPhotosByCategoryQueryHandler : IQueryHandler<GetPhotosByCategoryQuery, PhotosListResponseDto>
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPhotosByCategoryQueryHandler> _logger;

        public GetPhotosByCategoryQueryHandler(
            IPhotoRepository photoRepository,
            IMapper mapper,
            ILogger<GetPhotosByCategoryQueryHandler> logger)
        {
            _photoRepository = photoRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PhotosListResponseDto> Handle(GetPhotosByCategoryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var photos = await _photoRepository.GetPhotosByCategoryAsync(
                    request.CategoryId,
                    request.IncludeInactive,
                    request.IncludePrivate,
                    request.OrderBy ?? "SortOrder",
                    request.OrderDescending,
                    request.Limit,
                    request.Offset);

                var totalCount = await _photoRepository.GetPhotosByCategoryCountAsync(
                    request.CategoryId, 
                    request.IncludeInactive, 
                    request.IncludePrivate);

                var categoryName = await _photoRepository.GetCategoryNameAsync(request.CategoryId);

                var photoDtos = _mapper.Map<List<PhotoResponseDto>>(photos);
                
                // Set category name for each photo (not available in mapping)
                foreach (var photoDto in photoDtos)
                {
                    photoDto.CategoryName = categoryName;
                }

                return new PhotosListResponseDto
                {
                    Photos = photoDtos,
                    TotalCount = totalCount,
                    CategoryId = request.CategoryId,
                    CategoryName = categoryName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving photos for category {CategoryId}", request.CategoryId);
                throw;
            }
        }
    }
}
