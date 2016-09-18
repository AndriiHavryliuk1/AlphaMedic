namespace RestWebApi.DTOs
{
    public class FeedbackDto
    {

        public int FeedbackId { get; set; }

        public string FeedbackDescription { get; set; }

        public string UserFullName { get; set; }

        public int UserId{get;set;}
        
    }
}