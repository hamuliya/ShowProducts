using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPI.Global.ExceptionFilter
{
    public interface IExceptionFilter
    {
        void OnException(ExceptionContext context);
    }
}