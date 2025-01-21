using Server.Interfaces.Services;
using Server.Models.Entities;
using Server.Interfaces.Repositories;
using Server.Models.DTOS;
using Server.common.Exceptions;

namespace Server.Services
{
    public class ProductPresentationService : IProductPresentationService
    {
        private readonly IProductPresentationRepository _productPresentationRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppointmentService _appointmentService;

        public ProductPresentationService(
            IProductPresentationRepository productPresentationRepository,
            ICurrentUserService currentUserService,
            IAppointmentService appointmentService
        ){
            _productPresentationRepository = productPresentationRepository;
            _currentUserService = currentUserService;
            _appointmentService = appointmentService;
        }

        public async Task<ProductPresentation> GetProductPresentationAsync(GetProductPresentationQueryParams filter)
        {
            if (string.IsNullOrEmpty(filter.ProductId))
                throw new BadRequestException("productId is required");

            return await _productPresentationRepository.GetProductPresentationAsync(filter);
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

        public async Task<ProductPresentation> CreateProductPresentationtAsync(ProductPresentationDTO data, string visiteId)
        {
            var appointment = await _appointmentService.GetAppointmentByVisiteIdAsync(visiteId);

            if (appointment == null)
                throw new BadRequestException("appointment not found");
            
            var productPresentation = new ProductPresentation
            {
                AppointmentId = appointment.Id,
                Appointment = appointment,
                ProductId = data.ProductId,
            };

            productPresentation.ProductSlides = data.ProductSlides.Select(slide => new ProductSlide
            {
                ProductPresentationId = productPresentation.Id,
                SlideId = slide.SlideId,
                Comment = slide.Comment,
                Feedback = slide.Feedback,
                TimeSpent = slide.TimeSpent,
                OrderNumber = slide.OrderNumber
            }).ToList();

            return await _productPresentationRepository.AddProductPresentationAsync(productPresentation);
        }

        public async Task UpdateProductPresentationtAsync(string id, UpdateProductPresentationDTO productPresentation)
        {
            if (!Enum.IsDefined(typeof(ProductPresentationFeedbackType), productPresentation.GeneralFeedback))
                throw new BadRequestException("Invalid general feedback");

            await _productPresentationRepository.UpdateProductPresentationAsync(id, productPresentation);
        }

    }
}
