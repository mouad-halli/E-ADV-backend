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
    public class ProductPresentationController : ControllerBase
    {
        private readonly IProductPresentationService _productPresentationService;
        private readonly ILogger<ProductPresentationController> _logger;

        public ProductPresentationController(
            IProductPresentationService productPresentationService,
            ILogger<ProductPresentationController> logger
        ) {
            _productPresentationService = productPresentationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetProductPresentation([FromQuery] GetProductPresentationQueryParams filter )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productPresentation = await _productPresentationService.GetProductPresentationAsync(filter);

            return Ok(productPresentation);
        }

        [HttpGet("summary")]
        public async Task<ActionResult> FindUserProductPresentation([FromQuery] FindProductPresentationDTO data)
        {
            var response = new ProductPresentationResponse {
                latestPresentationDate = "",
                feedback = 0,
                presentationStatus = "not-presented"
            };

            var productPresentation = await _productPresentationService.FindUserProductPresentation(data);

            if (productPresentation == null)
                return Ok(response);

            // if (productPresentation.ProductSlides != null && productPresentation.ProductSlides.Count != 0) {
            //     if (productPresentation.ProductSlides.All(slide => slide.TimeSpent > 3 || slide.Feedback != FeedbackType.None || !string.IsNullOrEmpty(slide.Comment)))
            //         response.presentationStatus = "presented";
            //     else if (productPresentation.ProductSlides.Any(slide => slide.TimeSpent > 3 || slide.Feedback != FeedbackType.None || !string.IsNullOrEmpty(slide.Comment)))
            //         response.presentationStatus = "continue";
            // }

            // CreatedAt == DateTime.Today
            //presented   continue   continue  not-p
            //[3][3][3] | [3][3][] | [3][][] | [][][]
            //presented      presented 
            //[-1][-1][3] | [-1][3][3]
            //presented      continue      continue    continue      not-p
            //[3][3][3][3] | [3][3][3][] | [3][3][][] | [3][][][] | [][][][]

            // CreatedAt != DateTime.Today -> added cases: -
            //Replay        continue        continue      not-presented
            //[3][3][3]  |  [3][3][]     |  [3][][]     | [][][]
            //Replay          replay         replay
            //[3][3][3]  |  [-1][-1][3]  |  [-1][3][]  | -> get to one of the above
            //[3][3][3]  |  [][][]  |  [-1][-1][3]  | -> get to one of the above

            if (productPresentation.ProductSlides != null && productPresentation.ProductSlides.Count != 0)
            {
                if (productPresentation.ProductSlides.All(slide => slide.TimeSpent > 3 || slide.Feedback != FeedbackType.None || !string.IsNullOrEmpty(slide.Comment)))
                    response.presentationStatus = productPresentation.CreatedAt.ToUniversalTime().Date == DateTime.Today ? "presented" : "replay";
                else if (productPresentation.ProductSlides.Any(slide => slide.TimeSpent > 3 || slide.Feedback != FeedbackType.None || !string.IsNullOrEmpty(slide.Comment)))
                    response.presentationStatus = "continue";
            }

            if (response.presentationStatus != "not-presented" && productPresentation.ProductSlides != null)
            {
                response.latestPresentationDate = productPresentation.CreatedAt.ToShortDateString();

                var feedbacks = productPresentation.ProductSlides
                                    .Where(s => s.Feedback != FeedbackType.None)
                                    .Select(s => s.Feedback == FeedbackType.Neutral ? 2.5 : (double)s.Feedback);

                response.feedback = feedbacks.Any() ? feedbacks.Average() : 0;
            }

            return Ok(response);
        }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<ProductPresentation>>> GetAllProductPresentations()
        // {
        //     var products = await _productPresentationService.GetAllProductPresentationsAsync();
        //     return Ok(products);
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductPresentation>> GetProductPresentationById(string id)
        {
            var product = await _productPresentationService.GetProductPresentationByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductPresentation([FromBody] ProductPresentationDTO data, [FromQuery] string visiteId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productPresentation = await _productPresentationService.CreateProductPresentationtAsync(data, visiteId);

            return Ok(productPresentation);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProductPresentation(string id, UpdateProductPresentationDTO productPresentation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productPresentationService.UpdateProductPresentationtAsync(id, productPresentation);
            return NoContent();
        }

    }
}
