# Exceptions

## ForbiddenException

Thrown when a user tries to access a resource they do not have permission to access.

### Properties

- `HttpStatusCode statusCode`

## BadRequestException

Thrown when a bad request is made to the server.

### Properties

- `HttpStatusCode StatusCode`

## NotFoundException

Thrown when a requested resource is not found.

### Properties

- `HttpStatusCode StatusCode`

## UnauthorizedException

Thrown when a user is not authenticated.

### Properties

- `HttpStatusCode statusCode`