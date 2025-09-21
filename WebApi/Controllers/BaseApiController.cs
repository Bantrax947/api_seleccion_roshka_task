using Core.Contracts.Resposes;
using Core.Domain;
using Core.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected ErrorResponse CrearError (AppException exception)
        {
            var error = new ErrorResponse()
            {
                ErrorType = (ErrorType)Enum.Parse(typeof(ErrorType), exception.CodigoError.ToString()),
                ErrorMessage = exception.Message
            };
            return (error);
        }
        protected ErrorResponse CrearError(Exception exception)
        {
            var error = new ErrorResponse()
            {
                ErrorType = (ErrorType)Enum.Parse(typeof(ErrorType), ErrorType.ErrorInternoServidor.ToString()),
                ErrorMessage = exception.Message
            };
            return (error);
        }
    }
}