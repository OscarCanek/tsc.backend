using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace tsc.backend.tests.DBContexts
{
    public class SqlServerContextFactory<TTestDbContext, TDbContext> : IDisposable
        where TTestDbContext : DbContext
        where TDbContext : DbContext
    {
        private DbConnection connection;
        private readonly Func<TTestDbContext, Task> seed;
        private TTestDbContext _context;

        protected SqlServerContextFactory()
        {
        }

        protected SqlServerContextFactory(Func<TTestDbContext, Task> seed)
        {
            this.seed = seed;
        }

        public virtual async Task<TTestDbContext> CreateContextAsync()
        {
            var options = CreateOptions();
            using (TTestDbContext context = (TTestDbContext)Activator.CreateInstance(typeof(TTestDbContext), options))
            {
                this.connection = context.Database.GetDbConnection();
                context.Database.EnsureCreated();
                if (seed != null)
                {
                    await seed(context);
                }
            }

            this._context = (TTestDbContext)Activator.CreateInstance(typeof(TTestDbContext), CreateOptions());
            return (TTestDbContext)Activator.CreateInstance(typeof(TTestDbContext), CreateOptions());
        }

        public virtual void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }

        public virtual DbContextOptions<TDbContext> CreateOptions()
        {
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "10.211.55.4",
                InitialCatalog = "tsc",
                UserID = "sa",
                Password = "G6e6XqRib"
            };

            return new DbContextOptionsBuilder<TDbContext>()
                .UseSqlServer(connectionStringBuilder.ConnectionString)
                .Options;
        }
    }
}
