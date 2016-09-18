using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    public class BloodGroup
    {
        public BloodGroup() {}


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BloodGroupId { get; set; }
        public string Group { get; set; }
    }
}