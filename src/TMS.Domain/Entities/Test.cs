using System.Collections.Generic;
using System.Linq;
using TMS.Library.Attributes;

namespace TMS.Domain.Entities
{
    public class Test : BaseEntity
    {
        public int TestId { get; set; }

        public string TestName { get; set; }

        [NotColumn]
        public List<TestDetail> TestDetails { get; set; }

        public override List<List<BaseEntity>> GetDetails()
        {
            var details = new List<List<BaseEntity>>();
            details.Add(TestDetails.OfType<BaseEntity>().ToList());

            return details;
        }
    }
}