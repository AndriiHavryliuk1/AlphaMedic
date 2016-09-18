using RestWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestWebApi.DTOs
{
    public class DepartmentDetailsDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDescription { get; set; }        
        public UserDto DepartmentHead { get; set; }                
    }
}