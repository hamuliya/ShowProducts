using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Infrastructure.ExceptionFilter.Interface
{
    public interface IExceptionHandler
    {
        void OnException(ExceptionContext context);
    }
}