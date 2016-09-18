using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Dtos
{
    public class EmailPostDto
    {
        public Appointment appointment { get; set; }
        public bool deleteFlag { get; set; }
    }
}