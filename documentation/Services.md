# Services

## IAppointmentService

- `Task<bool> IsAppointmentVisitedAsync(string visiteId)`
- `Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()`
- `Task<Appointment> GetAppointmentByVisiteIdAsync(string visiteId)`
- `Task<Appointment> GetAppointmentByIdAsync(string id)`
- `Task<Appointment> AddAppointmentAsync(AppointmentDTO appointmentData)`

## ICurrentUserService

- `string GetUserId()`

## ILocationService

- `Task<Location?> GetByCoordinatesAsync(double latitude, double longitude)`
- `Task<Location> AddLocationAsync(LocationDto location)`

## ITokenService

- `string CreateToken(List<Claim> claims)`

## IProductPresentationService

- `Task<ProductPresentation> FindUserProductPresentation(FindProductPresentationDTO data)`
- `Task<IEnumerable<ProductPresentation>> GetAllProductPresentationsAsync()`
- `Task<ProductPresentation> GetProductPresentationAsync(GetProductPresentationQueryParams filter)`
- `Task<ProductPresentation> GetProductPresentationByIdAsync(string id)`
- `Task<ProductPresentation> CreateProductPresentationtAsync(ProductPresentationDTO data, string visiteId)`
- `Task UpdateProductPresentationtAsync(string id, UpdateProductPresentationDTO productPresentation)`

## IProductSlideService

- `Task<IEnumerable<ProductSlide>> GetAllProductSlidesAsync()`
- `Task<ProductSlide> GetProductSlideByIdAsync(string id)`
- `Task<ProductSlide> UpdateProductSlideAsync(string slideId, UpdateProductSlideDTO productSlideData)`

## IUserService

- `Task<User> getCurrentUser()`
- `Task<(string? errorMsg, IdentityResult? result)> CreateUser(User newUser, string userPassword)`
- `Task RegisterUserAsync(string firstname, string lastname, string email, string password)`
- `Task<(string jwtToken, User user)> LoginAsync(string email, string password)`
- `Task<(string jwtToken, User user)> MsalLoginAsync(JObject graphUser)`
- `Task<User> findUserById(string userId)`