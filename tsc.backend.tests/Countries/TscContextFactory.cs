using System;
using System.Threading.Tasks;
using tsc.backend.lib.Models;
using tsc.backend.tests.DBContexts;

namespace tsc.backend.tests.Countries
{
    public sealed class TscContextFactory : SqlServerContextFactory<TscContext, TscContext>
    {
        public TscContextFactory()
        {
        }

        public TscContextFactory(Func<TscContext, Task> seed)
        {
        }
    }
}
