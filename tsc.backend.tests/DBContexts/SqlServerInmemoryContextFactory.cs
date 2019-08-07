using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tsc.backend.tests.DBContexts
{
    public abstract class SqlServerInmemoryContextFactory<TTestDbContext, TDbContext> : IDisposable
        where TTestDbContext : DbContext
        where TDbContext : DbContext
    {
        private readonly Func<TTestDbContext, Task> seed;

        protected SqlServerInmemoryContextFactory()
        {
        }

        protected SqlServerInmemoryContextFactory(Func<TTestDbContext, Task> seed)
        {
            this.seed = seed;
        }

        public virtual async Task<TTestDbContext> CreateContextAsync()
        {
            var options = CreateOptions();
            using (TTestDbContext context = (TTestDbContext)Activator.CreateInstance(typeof(TTestDbContext), options))
            {
                context.Database.EnsureCreated();
                if (seed != null)
                {
                    await seed(context);
                }
            }

            return (TTestDbContext)Activator.CreateInstance(typeof(TTestDbContext), CreateOptions());
        }

        public virtual void Dispose()
        {
        }

        public virtual DbContextOptions<TDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<TDbContext>()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EFProviders.InMemory;Trusted_Connection=True;ConnectRetryCount=0")
                .Options;
        }
    }
}
