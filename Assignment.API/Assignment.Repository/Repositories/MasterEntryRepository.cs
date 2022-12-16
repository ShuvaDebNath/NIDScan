using Assignment.Enitites.DTO_s;
using Assignment.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Assignment.Repository.Repositories
{
   public class MasterEntryRepository : GenericRepository<MasterEntryModel>, IMasterEntryRepository
    {
        public MasterEntryRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public bool ExecuteWriteOperation(string sqlQuery)
        {
            using var sqlConnection = new SqlConnection(_connectionStringUserDB);
            var sqlCommand = new SqlCommand(sqlQuery, sqlConnection);

            sqlConnection.Open();
            var isExecuted = sqlCommand.ExecuteNonQuery();
            sqlConnection.Dispose();

            return isExecuted > 0;
        }
    }
}
