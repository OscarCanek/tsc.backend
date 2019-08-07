using System;
using System.Linq;
using System.Threading.Tasks;
using tsc.backend.lib.Countries;
using tsc.backend.lib.Models;
using Xunit;

namespace tsc.backend.tests.Countries
{
    public class CountryTests
    {
        [Theory]
        [InlineData(10, "GT", "GTM", "Guatemala", "502", 320)]
        public async Task TestListCountriesAsync(int top, string alfa2, string alfa3, string name, string phonePrefix, short countryCode)
        {
            // arrange
            using (var tscFactory = new TscContextFactory())
            {
                using (TscContext tscContext = await tscFactory.CreateContextAsync())
                {
                    using (var tran = await tscContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            Country c1 = new Country
                            {
                                Alfa2 = alfa2,
                                Alfa3 = alfa3,
                                PhonePrefix = phonePrefix,
                                IsoName = name,
                                CommonName = name,
                                CountryCode = countryCode
                            };

                            await tscContext.Countries.AddAsync(c1);
                            await tscContext.SaveChangesAsync();

                            // act
                            var handler = new CountryHandler(tscContext);
                            Tuple<CountryModel[], int> result = await handler.ListAsync(new GetCountryDetails { Top = top, Alfa2 = alfa2, Name = name });

                            // assert
                            Assert.NotNull(result);
                            Assert.Single(result.Item1.Where(x => x.Alfa2 == alfa2));

                            var country = result.Item1[0];

                            Assert.Equal(alfa2, country.Alfa2);
                            Assert.Equal(alfa3, country.Alfa3);
                            Assert.Equal(name, country.CommonName);
                            Assert.Equal(name, country.IsoName);
                            Assert.Equal(phonePrefix, country.PhonePrefix);
                            Assert.Equal(countryCode, country.CountryCode);

                        }
                        catch
                        {
                            throw;
                        }
                        finally
                        {
                            tran.Rollback();
                        }
                    }
                }
            }
        }
    }
}
