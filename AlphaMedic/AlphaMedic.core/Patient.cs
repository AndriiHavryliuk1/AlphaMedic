using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alphamedic.core
{
    public class Patient:User
    {
        public BloodGroup BloodGroup { get; set; }

        public MedicalHistory MedicalHistory { get; set; }
    }

    public enum BloodGroup
    {
        First,
        Second,
        Third,
        Fourth
    }
}
