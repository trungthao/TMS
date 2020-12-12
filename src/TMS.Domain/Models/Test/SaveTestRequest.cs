using TMS.Domain.Models;

namespace TMS.Domain.Models
{
    public class SaveTestRequest : SaveBaseEntityRequest
    {
        public string TestName { get; set; }
    }
}