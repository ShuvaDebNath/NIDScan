using Assignment.Enitites.DTO_s;
using Assignment.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterEntryController : ControllerBase
    {
        public readonly IMasterEntryService _masterEntryService;

        public MasterEntryController(IMasterEntryService masterEntryService)
        {
            _masterEntryService = masterEntryService;
        }

        [HttpGet(nameof(Get))]
        public IActionResult Get()
        {
            return Ok("Test");
        }

        [HttpPost(nameof(Insert))]
        public IActionResult Insert([FromBody] MasterEntryModel item)
        {
            try
            {
                return Ok(_masterEntryService.Insert(item));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
