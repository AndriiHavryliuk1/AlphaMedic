using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    public class MobileAuthentificator
    {
        [Key]
        public int UserId { get; set; }

        public bool? IsUseAuth { get; set; }

        public string MacAdress { get; set; }
    }
}