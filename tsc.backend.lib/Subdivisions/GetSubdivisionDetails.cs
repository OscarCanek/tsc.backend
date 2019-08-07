using System;
namespace tsc.backend.lib.Subdivisions
{
    public class GetSubdivisionDetails
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }

        // filters
        public int Top { get; set; }
    }
}
