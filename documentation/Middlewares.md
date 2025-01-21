# Middlewares

## ExceptionHandlerMiddleware

Handles exceptions globally and returns appropriate HTTP status codes and error messages.

### Methods

- `InvokeAsync(HttpContext context)`
- `HandleExceptionAsync(HttpContext context, Exception exception)`