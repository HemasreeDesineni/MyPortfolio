using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortfolio.Application.Photos.Queries;
using MyPortfolio.DTOs;

namespace MyPortfolio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController(
        IMediator mediator,
        ILogger<PhotosController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<PhotosController> _logger = logger;

        /// <summary>
        /// Get photos by category ID
        /// </summary>
        /// <param name="categoryId">Category ID to filter photos</param>
        /// <param name="includeInactive">Include inactive photos in results</param>
        /// <param name="includePrivate">Include private photos in results</param>
        /// <param name="orderBy">Order by field: SortOrder, Title, CreatedAt, TakenAt, ViewCount</param>
        /// <param name="orderDescending">Order in descending order</param>
        /// <param name="limit">Maximum number of photos to return</param>
        /// <param name="offset">Number of photos to skip (for pagination)</param>
        /// <returns>List of photos in the specified category</returns>
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<PhotosListResponseDto>> GetPhotosByCategory(
            [FromRoute] int categoryId,
            [FromQuery] bool includeInactive = false,
            [FromQuery] bool includePrivate = false,
            [FromQuery] string orderBy = "SortOrder",
            [FromQuery] bool orderDescending = false,
            [FromQuery] int? limit = null,
            [FromQuery] int? offset = null)
        {
            try
            {
                if (categoryId <= 0)
                {
                    return BadRequest(new { message = "Category ID must be greater than 0" });
                }

                var query = new GetPhotosByCategoryQuery
                {
                    CategoryId = categoryId,
                    IncludeInactive = includeInactive,
                    IncludePrivate = includePrivate,
                    OrderBy = orderBy,
                    OrderDescending = orderDescending,
                    Limit = limit,
                    Offset = offset
                };

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving photos for category {CategoryId}", categoryId);
                return StatusCode(500, new { message = "An error occurred while retrieving photos" });
            }
        }

        /// <summary>
        /// Get active public photos by category ID (convenience endpoint)
        /// </summary>
        /// <param name="categoryId">Category ID to filter photos</param>
        /// <param name="orderBy">Order by field: SortOrder, Title, CreatedAt, TakenAt, ViewCount</param>
        /// <param name="orderDescending">Order in descending order</param>
        /// <param name="limit">Maximum number of photos to return</param>
        /// <returns>List of active public photos in the specified category</returns>
        [HttpGet("category/{categoryId}/public")]
        public async Task<ActionResult<PhotosListResponseDto>> GetPublicPhotosByCategory(
            [FromRoute] int categoryId,
            [FromQuery] string orderBy = "SortOrder",
            [FromQuery] bool orderDescending = false,
            [FromQuery] int? limit = null)
        {
            try
            {
                if (categoryId <= 0)
                {
                    return BadRequest(new { message = "Category ID must be greater than 0" });
                }

                var query = new GetPhotosByCategoryQuery
                {
                    CategoryId = categoryId,
                    IncludeInactive = false,
                    IncludePrivate = false,
                    OrderBy = orderBy,
                    OrderDescending = orderDescending,
                    Limit = limit,
                    Offset = null
                };

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving public photos for category {CategoryId}", categoryId);
                return StatusCode(500, new { message = "An error occurred while retrieving photos" });
            }
        }
    }
}
