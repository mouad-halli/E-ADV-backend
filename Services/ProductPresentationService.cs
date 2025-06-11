using Server.Interfaces.Services;
using Server.Models.Entities;
using Server.Interfaces.Repositories;
using Server.Models.DTOS;
using Server.common.Exceptions;
using System.Text.Json;

namespace Server.Services
{
    public class ProductPresentationService : IProductPresentationService
    {
        private readonly IProductPresentationRepository _productPresentationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<ProductPresentationService> _logger;


        public ProductPresentationService(
            IProductPresentationRepository productPresentationRepository,
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService,
            ILogger<ProductPresentationService> logger
        )
        {
            _productPresentationRepository = productPresentationRepository;
            _currentUserService = currentUserService;
            _appointmentService = appointmentService;
            _logger = logger;
        }

        public async Task<(ProductPresentation?, string?)> GetProductPresentationAsync(GetProductPresentationQueryParams filter)
        {
            if (string.IsNullOrEmpty(filter.ProductId))
                throw new BadRequestException("productId is required");

            ProductPresentation productPresentation = await _productPresentationRepository.GetProductPresentationAsync(filter);

            string lastPresentationStatus = productPresentation == null ? "not-presented" : FindProductPresentationStatus(productPresentation);

            string? slideIdToContinueFrom = null;

            if (lastPresentationStatus == "continue" && productPresentation != null && productPresentation.ProductSlides != null)
                slideIdToContinueFrom = productPresentation.ProductSlides.OrderByDescending(s => s.UpdatedAt).First().SlideId;

            return (productPresentation, slideIdToContinueFrom);
        }

        public async Task<ProductPresentation> FindUserProductPresentation(FindProductPresentationDTO data)
        {
            var productPresentation = await _productPresentationRepository.GetLatestUserProductPresentation(
                                        data.ProductId,
                                        data.DoctorId,
                                        _currentUserService.GetUserId()
                                    );

            return productPresentation;
        }

        public async Task<IEnumerable<ProductPresentation>> GetAllProductPresentationsAsync()
        {
            return await _productPresentationRepository.GetAllProductPresentationsAsync();
        }

        public async Task<ProductPresentation> GetProductPresentationByIdAsync(string id)
        {
            return await _productPresentationRepository.GetProductPresentationByIdAsync(id);
        }

        public string FindProductPresentationStatus(ProductPresentation productPresentation)
        {
            string presentationStatus = "not-presented";

            if (productPresentation.ProductSlides != null && productPresentation.ProductSlides.Count != 0)
            {
                if (productPresentation.ProductSlides.All(slide => slide.TimeSpent >= 3 || slide.TimeSpent == -1))
                    presentationStatus = productPresentation.CreatedAt.ToUniversalTime().Date == DateTime.Today ? "presented" : "replay";
                else if (productPresentation.ProductSlides.Any(slide => slide.TimeSpent >= 3 || slide.TimeSpent == -1))
                    presentationStatus = "continue";
            }
            return presentationStatus;
        }

        public async Task<(ProductPresentation?, string?)> CreateProductPresentationtAsync(ProductPresentationDTO data, string visiteId)
        {
            var appointment = await _appointmentService.GetAppointmentByVisiteIdAsync(visiteId);

            if (appointment == null)
                throw new BadRequestException("appointment not found");

            // if presentationsStatus == continue
            //      -> get last productPresentationSlides
            //      -> flag slides with timeSpent >= 3 with timeSpent = -1 on new presentation slides

            ProductPresentation? lastProductPresentation = await FindUserProductPresentation(new FindProductPresentationDTO
            {
                ProductId = data.ProductId,
                DoctorId = appointment.ContactId
            });

            // _logger.LogInformation("lastProductPresentation: {collection}", JsonSerializer.Serialize(lastProductPresentation));

            string lastPresentationStatus = lastProductPresentation == null ? "not-presented" : FindProductPresentationStatus(lastProductPresentation);

            string? slideIdToContinueFrom = null;

            if (lastPresentationStatus == "continue" && lastProductPresentation != null && lastProductPresentation.ProductSlides != null)
                slideIdToContinueFrom = lastProductPresentation.ProductSlides.OrderByDescending(s => s.UpdatedAt).First().SlideId;

            // _logger.LogInformation("lastPresentationStatus {status}", lastPresentationStatus);

            var productPresentation = new ProductPresentation
            {
                AppointmentId = appointment.Id,
                Appointment = appointment,
                ProductId = data.ProductId,
                // CreatedAt = DateTime.Now.AddDays(-1)
            };

            productPresentation.ProductSlides = data.ProductSlides.Select(slide =>
            {
                double timeSpent = slide.TimeSpent;

                if (lastPresentationStatus == "continue" && lastProductPresentation != null && lastProductPresentation.ProductSlides != null)
                    timeSpent = lastProductPresentation.ProductSlides.First(s => s.SlideId == slide.SlideId).TimeSpent;

                ProductSlide productSlide = new ProductSlide
                {
                    ProductPresentationId = productPresentation.Id,
                    SlideId = slide.SlideId,
                    Comment = slide.Comment,
                    Feedback = slide.Feedback,
                    TimeSpent = timeSpent >= 3 ? -1 : slide.TimeSpent,
                    OrderNumber = slide.OrderNumber
                };
                return productSlide;
            }).ToList();

            // productPresentation.ProductSlides = data.ProductSlides.Select(slide => new ProductSlide
            // {
            //     ProductPresentationId = productPresentation.Id,
            //     SlideId = slide.SlideId,
            //     Comment = slide.Comment,
            //     Feedback = slide.Feedback,
            //     TimeSpent = slide.TimeSpent,
            //     OrderNumber = slide.OrderNumber
            // }).ToList();

            ProductPresentation createdProductPresentation = await _productPresentationRepository.AddProductPresentationAsync(productPresentation);
            return (createdProductPresentation, slideIdToContinueFrom);
        }

        public async Task UpdateProductPresentationtAsync(string id, UpdateProductPresentationDTO productPresentation)
        {
            if (!Enum.IsDefined(typeof(ProductPresentationFeedbackType), productPresentation.GeneralFeedback))
                throw new BadRequestException("Invalid general feedback");

            await _productPresentationRepository.UpdateProductPresentationAsync(id, productPresentation);
        }

    }
}
