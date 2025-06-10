using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Interfaces.Services;
using Server.Models.DTOS;
using Server.Models.Entities;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductSlideController : ControllerBase
    {
        private readonly IProductSlideService _productSlideService;
        private readonly ILogger<ProductSlideController> _logger;

        public ProductSlideController(
            IProductSlideService productSlideService,
            ILogger<ProductSlideController> logger
        ) {
            _productSlideService = productSlideService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductSlide>>> GetAllProductSlides()
        {
            var productSlides = await _productSlideService.GetAllProductSlidesAsync();
            return Ok(productSlides);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductSlide>> GetProductSlideById(string id)
        {
            var productSlide = await _productSlideService.GetProductSlideByIdAsync(id);
            if (productSlide == null)
            {
                return NotFound();
            }
            return Ok(productSlide);
        }

        [HttpPut("{slideId}")]
        public async Task<ActionResult> UpdateProductSlide(string slideId, UpdateProductSlideDTO productSlideData)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // _logger.LogInformation("good");
            var updatedSlide = await _productSlideService.UpdateProductSlideAsync(slideId, productSlideData);
            return Ok(updatedSlide);
        }
    }
}
