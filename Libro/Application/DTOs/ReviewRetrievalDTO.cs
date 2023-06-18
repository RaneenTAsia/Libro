using Domain.Enums;

namespace Application.DTOs
{
    public class ReviewRetrievalDTO
    {
        public Rating Rating { get; set; }
        public string? ReviewContent { get; set; }
    }
}