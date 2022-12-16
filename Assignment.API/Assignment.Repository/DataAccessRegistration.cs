using Microsoft.Extensions.DependencyInjection;
using Assignment.Repository.Repositories;
using Assignment.Repository.Interfaces;

namespace UserManagment.Repository
{
    public static class DataAccessRegistration
    {
        public static void AddDataAccessLayer(this IServiceCollection service)
        {
            service.AddTransient<IMasterEntryRepository, MasterEntryRepository>();
        }

    }
}
