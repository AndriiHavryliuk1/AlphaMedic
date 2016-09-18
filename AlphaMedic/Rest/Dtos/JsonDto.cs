using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Dtos
{
    public class JsonDto
    { 
        public int count { get; set; }
        public ICollection data { get; set; }
    }
}