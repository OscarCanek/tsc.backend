using System;
namespace tsc.backend.lib.Countries
{
    public interface ICountryHandler : IBaseCrudHandler<Guid, GetCountryDetails, CreateCountryModel, UpdateCountryModel, RemoveCountryModel, CountryModel>
    {
    }
}
