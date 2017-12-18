using Rest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Dtos
{

    public enum EmailPostState
    {
        confirm, delete, change
    }
    public class EmailPostDto
    {
        public Appointment appointment { get; set; }
        public EmailPostState EmailState { get; set; }
    }
}