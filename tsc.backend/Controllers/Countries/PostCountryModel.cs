using System;
namespace tsc.backend.Controllers.Countries
{
    public class PostCountryModel
    {
        public string CommonName { get; set; }
        public string IsoName { get; set; }
        public string Alfa2 { get; set; }
        public string Alfa3 { get; set; }
        public string CountryCode { get; set; }
        public string PhonePrefix { get; set; }
    }
}
