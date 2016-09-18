using System;

namespace RestWebApi.Models
{
    public class User
    {

        public int UserId { get; set; }
        
        public string UserName { get; set; }
        
        public string UserSurname { get; set; }
     
        public string UserEmail { get; set; }
    
        public string UserPassword { get; set; }

        public Gender UserGender { get; set; }

        public DateTime UserDateOfBirth { get; set; }

        public string UserPhone { get; set; }

        public string UserAdress { get; set; }

        public bool Active { get; set; }

        
    }

    public enum Gender
    {
        Male,
        Female
    }
    


    
}
