
using Assignment.Enitites.DTO_s;
using Assignment.Repository.Interfaces;
using Assignment.Service.Interfaces;
using Assignment.Service.Message;
using Microsoft.Extensions.Logging;

namespace Assignment.Service.Services
{
    public class MasterEntryService : IMasterEntryService
    {
        private readonly IMasterEntryRepository _masterEntryRepository;
        private readonly ILogger<MasterEntryService> _logger;

        public MasterEntryService(IMasterEntryRepository masterEntryRepository, ILogger<MasterEntryService> logger)
        {
            _masterEntryRepository = masterEntryRepository;
            _logger = logger;
        }

        public Messages Insert(MasterEntryModel item)
        {
            try
            {
                var sqlQuery = $"INSERT INTO [dbo].[{item.TableName}] ";

                sqlQuery += InsertQueryGeneratorWithKeyParams(item);

                sqlQuery += InsertQueryGeneratorWithValueParams(item);

                var result = _masterEntryRepository.ExecuteWriteOperation(sqlQuery);

                if (result)
                {
                    _logger.LogInformation($"Data Save Success!");
                    return MessageType.SaveSuccess(item);
                }
                _logger.LogInformation($"Data Save Fail!");
                return MessageType.SaveError(null);
            }
            catch (Exception ex)
            {
                string innserMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                _logger.LogInformation($"Sourc: {ex.Source};\t Stack Trace: {ex.StackTrace};\t Message: {ex.Message};\t Inner Exception: {innserMsg};\n", "");
                throw;
            }
        }


        private string InsertQueryGeneratorWithKeyParams(MasterEntryModel item)
        {
            Object objQueryParams = new Object();

            objQueryParams = item.QueryParams;

            string strQueryParamsJsonData = Convert.ToString(objQueryParams);

            var dataQueryParams = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(strQueryParamsJsonData);

            var sqlQuery = " ( ";

            foreach (var itemQueryParams in dataQueryParams)
            {
                sqlQuery += " " + itemQueryParams.Key + ",";
            }

            sqlQuery += " MakeDate ";

            return sqlQuery;
        }

        private string InsertQueryGeneratorWithValueParams(MasterEntryModel item)
        {
            Object objQueryParams = new Object();

            objQueryParams = item.QueryParams;

            string strQueryParamsJsonData = Convert.ToString(objQueryParams);

            var dataQueryParams = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(strQueryParamsJsonData);

            var sqlQuery = " ) VALUES (";

            foreach (var itemQueryParams in dataQueryParams)
            {
                if (itemQueryParams.Value.ToLower() == "newid()")
                {
                    sqlQuery += " " + itemQueryParams.Value + " ,";
                }
                else
                {
                    sqlQuery += " N'" + itemQueryParams.Value + "' ,";
                }
            }
            sqlQuery += " getdate()";
            sqlQuery += ")";

            return sqlQuery;
        }        

    }
}
