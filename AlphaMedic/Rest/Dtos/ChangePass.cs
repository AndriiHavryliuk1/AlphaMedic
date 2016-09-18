using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Dtos
{
    public class ChangePass
    {
        public string OldPass { get; set; }
        public string NewPass { get; set; }
    }
}