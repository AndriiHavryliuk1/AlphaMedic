using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rest.Dtos
{
    public class DateFilterDto
    {
        public DateTime? periodFrom { get; set; }
        public DateTime? periodTill { get; set; }
    }
}