using System;
using System.Linq;
using System.Threading.Tasks;
using Dawn;
using Microsoft.EntityFrameworkCore;
using tsc.backend.lib.Countries;
using tsc.backend.lib.Models;

namespace tsc.backend.lib.Subdivisions
{
    public class SubdivisionHandler : ISubdivisionHandler
    {
        private readonly TscContext tscContext;

        public SubdivisionHandler(TscContext tscContext)
        {
            this.tscContext = tscContext;
        }

        public async Task<SubdivisionModel> CreateAsync(CreateSubdivisionModel model)
        {
            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.CountryId, x => x.NotEqual(Guid.Empty))
                .Member(x => x.Name, x => x.NotEmpty());

            model.Name = model.Name.Trim();

            var exists = await this.tscContext.Subdivisions
                .AnyAsync(x => x.Name == model.Name);

            if (exists)
            {
                // here we need to create custom exceptions
                throw new Exception("Subdivision already exists");
            }

            var country = await this.tscContext.Countries
                .FirstOrDefaultAsync(x => x.Id == model.CountryId);

            if (country == null)
            {
                // here we need to create custom exceptions
                throw new Exception("invalid country");
            }

            var subdivision = new Subdivision
            {
                CountryId = model.CountryId,
                Name = model.Name
            };

            await this.tscContext.Subdivisions.AddAsync(subdivision);
            await this.tscContext.SaveChangesAsync();

            return new SubdivisionModel
            {
                Id = subdivision.Id,
                Name = subdivision.Name,
                Country = new CountryModel
                {
                    Id = country.Id,
                    IsoName = country.IsoName,
                    CommonName = country.CommonName,
                    Alfa2 = country.Alfa2,
                    Alfa3 = country.Alfa3,
                    CountryCode = country.CountryCode,
                    PhonePrefix = country.PhonePrefix
                }
            };
        }

        public async Task<SubdivisionModel> GetDetailsAsync(GetSubdivisionDetails model)
        {
            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.Id, x => x.NotEqual(Guid.Empty))
                .Member(x => x.CountryId, x => x.NotEqual(Guid.Empty));

            var country = await this.tscContext.Countries
                .FirstOrDefaultAsync(x => x.Id == model.CountryId);

            if (country == null)
            {
                // here we need to create custom exceptions
                throw new Exception("invalid country");
            }

            var subdivision = await this.tscContext.Subdivisions
                .FirstOrDefaultAsync(x => x.Id == model.Id && x.CountryId == model.CountryId);

            if (country == null)
            {
                throw new Exception("Country not found");
            }

            return new SubdivisionModel
            {
                Id = subdivision.Id,
                Name = subdivision.Name,
                Country = new CountryModel
                {
                    Id = country.Id,
                    IsoName = country.IsoName,
                    CommonName = country.CommonName,
                    Alfa2 = country.Alfa2,
                    Alfa3 = country.Alfa3,
                    CountryCode = country.CountryCode,
                    PhonePrefix = country.PhonePrefix
                }
            };
        }

        public async Task<Tuple<SubdivisionModel[], int>> ListAsync(GetSubdivisionDetails model)
        {
            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.Top, x => Guard.InRange(x, 0, 100))
                .Member(x => x.CountryId, x => x.NotEqual(Guid.Empty));

            var country = await this.tscContext.Countries
                .FirstOrDefaultAsync(x => x.Id == model.CountryId);

            if (country == null)
            {
                // here we need to create custom exceptions
                throw new Exception("invalid country");
            }

            var subdivisions = this.tscContext.Subdivisions
                .Where(x => x.CountryId == model.CountryId)
                .Include(x => x.CountryNav)
                .Take(model.Top);

            var result = await subdivisions
                .Select(x => new SubdivisionModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Country = new CountryModel
                    {
                        Id = x.CountryNav.Id,
                        IsoName = x.CountryNav.IsoName,
                        CommonName = x.CountryNav.CommonName,
                        Alfa2 = x.CountryNav.Alfa2,
                        Alfa3 = x.CountryNav.Alfa3,
                        CountryCode = x.CountryNav.CountryCode,
                        PhonePrefix = x.CountryNav.PhonePrefix
                    }
                })
                .ToArrayAsync();

            return Tuple.Create(result, result.Length);
        }

        public async Task<Guid> RemoveAsync(RemoveSubdivisionModel model)
        {
            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.Id, x => x.NotEqual(Guid.Empty));

            var subdivision = await this.tscContext.Subdivisions
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (subdivision == null)
            {
                return model.Id;
            }

            this.tscContext.Remove(subdivision);
            await this.tscContext.SaveChangesAsync();

            return model.Id;
        }

        public async Task<SubdivisionModel> UpdateAsync(UpdateSubdivisionModel model)
        {

            Guard.Argument(model, nameof(model))
                .NotNull()
                .Member(x => x.Name, x => x.NotEmpty());

            model.Name = model.Name.Trim();

            var subdivision = await this.tscContext.Subdivisions
                .Include(x => x.CountryNav)
                .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (subdivision == null)
            {
                // here we need to create custom exceptions
                throw new Exception("Subdivision not found");
            }

            subdivision.Name = model.Name;

            try
            {
                await this.tscContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!await SubdivisionExistsAsync(model.Id))
                {
                    throw new Exception("Subdivision not found", e);
                }

                throw;
            }

            return new SubdivisionModel
            {
                Id = subdivision.Id,
                Name = subdivision.Name,
                Country = new CountryModel
                {
                    Id = subdivision.CountryNav.Id,
                    IsoName = subdivision.CountryNav.IsoName,
                    CommonName = subdivision.CountryNav.CommonName,
                    Alfa2 = subdivision.CountryNav.Alfa2,
                    Alfa3 = subdivision.CountryNav.Alfa3,
                    CountryCode = subdivision.CountryNav.CountryCode,
                    PhonePrefix = subdivision.CountryNav.PhonePrefix
                }
            };
        }

        private async Task<bool> SubdivisionExistsAsync(Guid id)
        {
            return await this.tscContext.Subdivisions
                .AnyAsync(x => x.Id == id);
        }
    }
}
