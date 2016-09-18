using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    public class EmailInput
    {
        public string UserName { get; set; }
        public string Email  { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}