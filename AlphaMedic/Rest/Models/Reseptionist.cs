using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rest.Models
{
    [Table("Receptionists")]
    public class Receptionist :Employee
    {
    }
}