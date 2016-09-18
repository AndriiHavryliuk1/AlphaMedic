using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{

    public enum GenderType
    {
        Male, Female
    }

    public abstract class User
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public GenderType Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [ForeignKey("UserClaim")]
        public int UserClaimId { get; set; }

        //[Phone]
        //[DataType(DataType.PhoneNumber)]       
       // [DisplayFormat(ConvertEmptyStringToNull = false)]
       [Required]
        public string Phone { get; set; }

        
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Address { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        public bool? Active { get; set; }
           
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string URLImage { get; set; }

        public virtual UserClaim UserClaim { get; set; }
    }
}