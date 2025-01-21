# Models

## Entities

### Appointment

- `string Id`
- `string UserId`
- `User User`
- `string ContactId`
- `string VisiteId`
- `string LocationId`
- `Location Location`
- `DateTime CreatedAt`
- `ICollection<ProductPresentation>? ProductPresentations`

### Location

- `string Id`
- `double Latitude`
- `double Longitude`

### ProductPresentation

- `string Id`
- `string AppointmentId`
- `Appointment Appointment`
- `string ProductId`
- `ICollection<ProductSlide>? ProductSlides`
- `DateTime CreatedAt`
- `string GeneralComment`
- `ProductPresentationFeedbackType GeneralFeedback`

### ProductSlide

- `string Id`
- `string ProductPresentationId`
- `ProductPresentation ProductPresentation`
- `string SlideId`
- `string Comment`
- `FeedbackType Feedback`
- `double TimeSpent`
- `DateTime StartDate`
- `int OrderNumber`

### User

- `int EmployeeNumber`
- `string FirstName`
- `string LastName`

## DTOs

### AppointmentDTO

- `string ContactId`
- `string VisiteId`
- `LocationDto Location`

### LocationDto

- `double Latitude`
- `double Longitude`

### ProductPresentationDTO

- `string ProductId`
- `List<ProductSlideDTO> ProductSlides`

### ProductSlideDTO

- `string SlideId`
- `string Comment`
- `FeedbackType Feedback`
- `double TimeSpent`
- `int OrderNumber`

### UserDto

- `string FirstName`
- `string LastName`
- `string Email`
- `string Password`

### LoginDto

- `string Email`
- `string Password`