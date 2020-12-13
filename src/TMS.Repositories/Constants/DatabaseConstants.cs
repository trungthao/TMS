using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Repositories.Constants
{
    public class DatabaseConstants
    {
        public const string mscTemplateStoreNameInsert = "Proc_Insert{0}";
        public const string mscTemplateStoreNameUpdate = "Proc_Update{0}";
        public const string mscTemplateStoreNameDeleteById = "Proc_Delete{0}ById";
        public const string mscTemplateStoreNameGetObjectById = "Proc_Get{0}ById";
    }
}
