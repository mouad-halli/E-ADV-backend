# Repositories

## IAppointmentRepository

- `Task<bool> IsAppointmentVisitedAsync(string visiteId)`
- `Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()`
- `Task<Appointment> GetAppointmentByVisiteIdAsync(string visiteId, string userId)`
- `Task<Appointment> GetAppointmentByIdAsync(string id)`
- `Task<Appointment> AddAppointmentAsync(Appointment appointment)`
- `Task UpdateAppointmentAsync(Appointment appointment)`
- `Task DeleteAppointmentAsync(string id)`

## ILocationRepository

- `Task<Location?> GetByCoordinatesAsync(double latitude, double longitude)`
- `Task<Location> AddLocationAsync(Location location)`

## IProductPresentationRepository

- `Task<ProductPresentation?> GetLatestUserProductPresentation(string productId, string doctorId, string userId)`
- `Task<IEnumerable<ProductPresentation>> GetAllProductPresentationsAsync()`
- `Task<ProductPresentation> GetProductPresentationAsync(GetProductPresentationQueryParams filter)`
- `Task<ProductPresentation> GetProductPresentationByIdAsync(string id)`
- `Task<ProductPresentation> AddProductPresentationAsync(ProductPresentation productPresentation)`
- `Task UpdateProductPresentationAsync(string id, UpdateProductPresentationDTO productPresentation)`
- `Task DeleteProductPresentationAsync(string id)`

## IUserRepository

- `Task<User> GetByEmailAsync(string username)`
- `Task<User> GetByIdAsync(string id)`
- `Task AddAsync(User user)`
- `Task<bool> IsEmailTakenAsync(string username)`

## IProductSlideRepository

- `Task<IEnumerable<ProductSlide>> GetAllProductSlidesAsync()`
- `Task<ProductSlide> GetProductSlideByIdAsync(string id)`
- `Task AddProductSlideAsync(ProductSlide productSlide)`
- `Task<ProductSlide> UpdateProductSlideAsync(string slideId, ProductSlide productSlide)`
- `Task DeleteProductSlideAsync(string id)`