namespace Rest.Dtos
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class DepartmentFullDto
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ShortUserDto HeadDepartment { get; set; }
        public FeedbackDto[] Feedbacks { get; set; }
        public int FeedbacksCount { get; set; }
        public string URLImage { get; set; }

    }
}