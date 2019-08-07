using System;
namespace tsc.backend.lib.Models
{
    public class Subdivision
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CountryId { get; set; }
        public byte[] RowVersion { get; set; }

        public Country CountryNav { get; set; }
    }
}
