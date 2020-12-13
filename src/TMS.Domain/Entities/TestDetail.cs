using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain.Entities
{
    public class TestDetail : BaseEntity
    {
        public int TestDetailId { get; set; }

        public string TestDetailName { get; set; }

        public int TestId { get; set; }

        public override string GetForeignKeyName()
        {
            return "TestId";
        }
    }
}
