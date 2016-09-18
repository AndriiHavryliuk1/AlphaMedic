using System;

namespace Rest.Dtos
{
    public class MedicationDto
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int MedicationId { get; set; }        
    }
}