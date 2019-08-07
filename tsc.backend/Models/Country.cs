using System;
using System.Collections.Generic;

namespace tsc.backend.Models
{
    public class Country
    {
        public Guid Id { get; set; }
        public string CommonName { get; set; }
        public string IsoName { get; set; }
        public string Alfa2 { get; set; }
        public string Alfa3 { get; set; }
        public string CountryCode { get; set; }
        public string PhonePrefix { get; set; }
        public byte RowVersion { get; set; }

        public ICollection<Subdivision> Subdivisions { get; set; }
    }
}
