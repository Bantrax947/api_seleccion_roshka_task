using Core.Enum;

namespace Core.Contracts.Resposes
{
    public class ErrorResponse
    {
        public ErrorType ErrorType { get; set; }
        public required string ErrorMessage { get; set; }
    }
}