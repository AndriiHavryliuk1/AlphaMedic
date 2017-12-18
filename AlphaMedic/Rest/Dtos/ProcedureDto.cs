using Rest.Models;
using System;
using System.Collections.Generic;

namespace Rest.Dtos
{
    public class ProcedureDto
    {
        public int ProcedureId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal? Price { get; set; }
        public ShortUserDto Doctor { get; set; }
        public virtual string Type { get; set; }
        public AppointmentState State { get; set; }

    }

    public class ExaminationDto : ProcedureDto
    {
        public Diagnosis Diagnosis { get; set; }
        public override string Type { get { return "Examination"; } } 
    }

    public class VaccinationDto : ProcedureDto
    {
        public override string Type { get { return "Vaccination"; } }
    }
    public class TreatmentDto : ProcedureDto
    {
        public IEnumerable<MedicationDto> Medications { get; set; }

        public string Result { get; set; }

        public override string Type { get { return "Treatment"; } }
    }
}