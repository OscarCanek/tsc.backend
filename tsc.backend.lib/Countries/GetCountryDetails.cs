using System;
namespace tsc.backend.lib.Countries
{
    public class GetCountryDetails
    {
        public Guid Id { get; set; }

        // filters
        public string Name { get; set; }
        public string Alfa2 { get; set; }
        public int Top { get; set; }
    }
}
