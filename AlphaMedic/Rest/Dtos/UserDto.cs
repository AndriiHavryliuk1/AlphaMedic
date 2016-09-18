using Rest.Models;
using System;

namespace Rest.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public GenderType Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string URLImage { get; set; }
        public string Email { get; set; }
    }

    public class ShortUserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}