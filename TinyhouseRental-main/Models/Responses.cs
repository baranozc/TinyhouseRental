namespace MyProject.Models
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class UserResponse : BaseResponse
    {
        public User User { get; set; }
    }

    public class ListingResponse : BaseResponse
    {
        public Listing Listing { get; set; }
    }

    public class ReservationResponse : BaseResponse
    {
        public Reservation Reservation { get; set; }
    }

    public class CommentResponse : BaseResponse
    {
        public Comment Comment { get; set; }
    }
} 