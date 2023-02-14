using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WebAPI.Infrastructure.ExceptionFilter.Interface;

namespace WebAPI.Infrastructure.ExceptionFilter.Services
{
    public class ExceptionFilter : IExceptionHandler
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var message = "An error occurred while processing your request.";

            if (exception is ResourceNotFoundException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                message = "The requested resource could not be found.";
            }
            else if (exception is UnauthorizedAccessException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                message = "Unauthorized access.";
            }
            else if (exception is ValidationException)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                message = "Validation Error";
            }
            else
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.Result = new JsonResult(message);

            _logger.LogError(exception, message);
        }


    }

    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() : base() { }
        public ResourceNotFoundException(string message) : base(message) { }
        public ResourceNotFoundException(string message, Exception inner) : base(message, inner) { }
    }


}
