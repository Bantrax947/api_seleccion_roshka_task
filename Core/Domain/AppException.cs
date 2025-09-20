using Core.Enum;

namespace Core.Domain
{
    public class AppException : Exception 
    {
        public ErrorType CodigoError;

        public AppException(ErrorType codigoError, string mensajeError) : base(mensajeError)
        {
            CodigoError = codigoError;
        }
    }
}