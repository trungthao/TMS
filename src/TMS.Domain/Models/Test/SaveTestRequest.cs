using System.Collections.Generic;
using TMS.Domain.Models;

namespace TMS.Domain.Models
{
    public class SaveTestRequest : SaveBaseEntityRequest
    {
        public int TestId { get; set; }

        public string TestName { get; set; }

        public List<SaveTestDetailRequest> TestDetails { get; set; }
    }
}