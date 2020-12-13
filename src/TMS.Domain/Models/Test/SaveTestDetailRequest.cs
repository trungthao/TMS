using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain.Models
{
    public class SaveTestDetailRequest : SaveBaseEntityRequest
    {
        public int TestDetailId { get; set; }

        public string TestDetailName { get; set; }
    }
}
