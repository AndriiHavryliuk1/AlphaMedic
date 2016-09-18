using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alphamedic.core
{
    public class Doctor:Employee
    {
        public string Degree { get; set; }

        public string Education { get; set; }

        public Schedule Schedule { get; set; }
    }
}
