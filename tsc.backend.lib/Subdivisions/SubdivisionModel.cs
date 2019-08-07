using System;
using tsc.backend.lib.Countries;

namespace tsc.backend.lib.Subdivisions
{
    public class SubdivisionModel
    {
        public Guid Id { get; set; }
        public CountryModel Country { get; set; }
        public string Name { get; set; }
    }
}
