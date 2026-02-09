using AutoMapper;
using MyPortfolio.Application.Categories.Queries;
using MyPortfolio.Application.Common.Interfaces;
using MyPortfolio.DTOs;
using MyPortfolio.Infrastructure.Repositories;

namespace MyPortfolio.Application.Categories.Handlers
{
    public class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, CategoriesListResponseDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoriesQueryHandler> _logger;

        public GetCategoriesQueryHandler(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<GetCategoriesQueryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoriesListResponseDto> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var categories = await _categoryRepository.GetAllCategoriesAsync(
                    request.IncludeInactive,
                    request.OrderBy ?? "SortOrder",
                    request.OrderDescending);

                var totalCount = await _categoryRepository.GetCategoriesCountAsync(request.IncludeInactive);

                var categoryDtos = _mapper.Map<List<CategoryResponseDto>>(categories);

                return new CategoriesListResponseDto
                {
                    Categories = categoryDtos,
                    TotalCount = totalCount
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                throw;
            }
        }
    }
}
