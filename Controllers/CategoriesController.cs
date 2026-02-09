using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Application.Categories.Queries;
using MyPortfolio.DTOs;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(
            IMediator mediator,
            ILogger<CategoriesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <param name="includeInactive">Include inactive categories in results</param>
        /// <param name="orderBy">Order by field: SortOrder, Name, CreatedAt</param>
        /// <param name="orderDescending">Order in descending order</param>
        /// <returns>List of categories</returns>
        [HttpGet]
        public async Task<ActionResult<CategoriesListResponseDto>> GetCategories(
            [FromQuery] bool includeInactive = false,
            [FromQuery] string orderBy = "SortOrder",
            [FromQuery] bool orderDescending = false)
        {
            try
            {
                var query = new GetCategoriesQuery
                {
                    IncludeInactive = includeInactive,
                    OrderBy = orderBy,
                    OrderDescending = orderDescending
                };

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories");
                return StatusCode(500, new { message = "An error occurred while retrieving categories" });
            }
        }

        /// <summary>
        /// Get active categories only (convenience endpoint)
        /// </summary>
        /// <returns>List of active categories</returns>
        [HttpGet("active")]
        public async Task<ActionResult<CategoriesListResponseDto>> GetActiveCategories(
            [FromQuery] string orderBy = "SortOrder",
            [FromQuery] bool orderDescending = false)
        {
            try
            {
                var query = new GetCategoriesQuery
                {
                    IncludeInactive = false,
                    OrderBy = orderBy,
                    OrderDescending = orderDescending
                };

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active categories");
                return StatusCode(500, new { message = "An error occurred while retrieving categories" });
            }
        }
    }
}
