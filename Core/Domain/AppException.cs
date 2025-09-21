using Core.Enum;

namespace Core.Domain
{
    public class AppException : Exception 
    {
        public ErrorType CodigoError { get; }

        public AppException(string message, ErrorType errorType) : base(message)
        {
            CodigoError = errorType;
        }
    }
}