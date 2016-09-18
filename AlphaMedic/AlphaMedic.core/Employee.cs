using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alphamedic.core
{
    public abstract class Employee : User
    {
        public DateTime EmployementDate { get; set; }

        public DateTime DismissalDate { get; set; }

        public string EmployementRecordBookNumber { get; set; }

        public bool IsWorking()
        {
            throw new NotImplementedException();
        }
    }
}
