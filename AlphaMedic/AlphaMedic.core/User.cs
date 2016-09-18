using System;

namespace alphamedic.core
{
    public abstract class User
    {

        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Surname { get; set; }
     
        public string Email { get; set; }
    
        public string Password { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; }

        public string Adress { get; set; }

        public bool Active { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
    


    
}
