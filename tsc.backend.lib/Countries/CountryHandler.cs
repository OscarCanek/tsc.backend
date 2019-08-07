using System;
using System.Linq;
using System.Threading.Tasks;
using Dawn;
using Microsoft.EntityFrameworkCore;
using tsc.backend.lib.Models;

namespace tsc.backend.lib.Countries
{
    public class CountryHandler : ICountryHandler
    {
        private readonly TscContext tscContext;

        public CountryHandler(TscContext tscContext)
        {
            this.tscContext = tscContext;
        }

        public async Task<CountryModel> CreateAsync(CreateCountryModel model)
        {
            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.IsoName, x => x.NotEmpty())
                .Member(x => x.CommonName, x => x.NotEmpty());

            model.IsoName = model.IsoName.Trim();
            model.CommonName = model.CommonName.Trim();

            var exists = await this.tscContext.Countries
                .AnyAsync(x => x.IsoName == model.IsoName);

            if (exists)
            {
                // here we need to create custom exceptions
                throw new Exception("Country already exists");
            }

            var country = new Country
            {
                IsoName = model.IsoName,
                CommonName = model.CommonName,
                Alfa2 = model.Alfa2,
                Alfa3 = model.Alfa3,
                CountryCode = model.CountryCode,
                PhonePrefix = model.PhonePrefix
            };

            await this.tscContext.Countries.AddAsync(country);
            await this.tscContext.SaveChangesAsync();

            return new CountryModel
            {
                Id = country.Id,
                IsoName = country.IsoName,
                CommonName = country.CommonName,
                Alfa2 = country.Alfa2,
                Alfa3 = country.Alfa3,
                CountryCode = country.CountryCode,
                PhonePrefix = country.PhonePrefix
            };
        }

        public async Task<CountryModel> GetDetailsAsync(GetCountryDetails model)
        {
            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.Id, x => x.NotEqual(Guid.Empty));

            var country = await this.tscContext.Countries
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (country == null)
            {
                throw new Exception("Country not found");
            }

            return new CountryModel
            {
                Id = country.Id,
                IsoName = country.IsoName,
                CommonName = country.CommonName,
                Alfa2 = country.Alfa2,
                Alfa3 = country.Alfa3,
                CountryCode = country.CountryCode,
                PhonePrefix = country.PhonePrefix
            };
        }

        public async Task<Tuple<CountryModel[], int>> ListAsync(GetCountryDetails model)
        {
            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.Top, x => Guard.InRange(x, 0, 100));

            var countries = this.tscContext.Countries;

            IQueryable<Country> query = null;

            if (!string.IsNullOrEmpty(model.Name) || !string.IsNullOrEmpty(model.Alfa2))
            {
                if (!string.IsNullOrEmpty(model.Name))
                {
                    query = countries.Where(x => x.CommonName.Contains(model.Name) || x.IsoName.Contains(model.Name));
                }

                if (!string.IsNullOrEmpty(model.Alfa2))
                {
                    if (query != null)
                    {
                        query = query.Where(x => x.Alfa2.Contains(model.Alfa2));
                    }
                    else
                    {
                        query = countries.Where(x => x.Alfa2.Contains(model.Alfa2));
                    }
                }

                if (query != null)
                {
                    query = query.Take(model.Top);
                }
            }
            else
            {
                query = countries.Take(model.Top);
            }

            var result = await query
                .Select(x => new CountryModel
                {
                    Id = x.Id,
                    IsoName = x.IsoName,
                    CommonName = x.CommonName,
                    Alfa2 = x.Alfa2,
                    Alfa3 = x.Alfa3,
                    CountryCode = x.CountryCode,
                    PhonePrefix = x.PhonePrefix
                })
                .ToArrayAsync();

            return Tuple.Create(result, result.Length);
        }

        public async Task<Guid> RemoveAsync(RemoveCountryModel model)
        {
            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.Id, x => x.NotEqual(Guid.Empty));

            var country = await this.tscContext.Countries
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (country == null)
            {
                return model.Id;
            }

            this.tscContext.Remove(country);
            await this.tscContext.SaveChangesAsync();

            return model.Id;
        }

        public async Task<CountryModel> UpdateAsync(UpdateCountryModel model)
        {
            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.IsoName, x => x.NotEmpty())
                .Member(x => x.CommonName, x => x.NotEmpty());

            model.IsoName = model.IsoName.Trim();
            model.CommonName = model.CommonName.Trim();

            var country = await this.tscContext.Countries
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (country == null)
            {
                // here we need to create custom exceptions
                throw new Exception("Country not found");
            }

            country.IsoName = model.IsoName;
            country.CommonName = model.CommonName;
            country.Alfa2 = model.Alfa2;
            country.Alfa3 = model.Alfa3;
            country.CountryCode = model.CountryCode;
            country.PhonePrefix = model.PhonePrefix;

            try
            {
                await this.tscContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await CountryExistsAsync(model.Id))
                {
                    throw new Exception("Country not found", e);
                }

                throw;
            }

            return new CountryModel
            {
                Id = country.Id,
                IsoName = country.IsoName,
                CommonName = country.CommonName,
                Alfa2 = country.Alfa2,
                Alfa3 = country.Alfa3,
                CountryCode = country.CountryCode,
                PhonePrefix = country.PhonePrefix
            };
        }

        private async Task<bool> CountryExistsAsync(Guid id)
        {
            return await this.tscContext.Countries
                .AnyAsync(x => x.Id == id);
        }
    }
}
