# API Endpoints

## AuthenticationController

- `POST /api/authentication/msal-login`
- `POST /api/authentication/register`
- `POST /api/authentication/login`
- `GET /api/authentication/logout`

## AppointmentController

- `GET /api/appointment`
- `GET /api/appointment/visite/{visiteId}`
- `GET /api/appointment/isVisited/{visiteId}`
- `GET /api/appointment/{id}`
- `POST /api/appointment`

## ProductSlideController

- `GET /api/productslide`
- `GET /api/productslide/{id}`
- `PUT /api/productslide/{slideId}`

## ProductPresentationController

- `GET /api/productpresentation`
- `GET /api/productpresentation/summary`
- `GET /api/productpresentation/{id}`
- `POST /api/productpresentation`
- `PUT /api/productpresentation/{id}`

## UserController

- `GET /api/user/me`