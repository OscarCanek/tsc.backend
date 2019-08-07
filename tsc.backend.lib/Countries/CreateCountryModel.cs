using System;

namespace tsc.backend.lib.Countries
{
    public class CreateCountryModel
    {
        public string CommonName { get; set; }
        public string IsoName { get; set; }
        public string Alfa2 { get; set; }
        public string Alfa3 { get; set; }
        public Int16 CountryCode { get; set; }
        public string PhonePrefix { get; set; }
    }
}
