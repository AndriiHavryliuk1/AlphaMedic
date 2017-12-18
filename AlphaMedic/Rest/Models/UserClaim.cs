using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    public class UserClaim
    {        
        public UserClaim()
        {
            Users = new List<User>();
        }

        [Key]
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
    }
}