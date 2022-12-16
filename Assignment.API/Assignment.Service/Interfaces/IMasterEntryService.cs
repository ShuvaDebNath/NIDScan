using Assignment.Enitites.DTO_s;
using Assignment.Service.Message;

namespace Assignment.Service.Interfaces
{
    public interface IMasterEntryService
    {
        Messages Insert(MasterEntryModel item);
    }
}
