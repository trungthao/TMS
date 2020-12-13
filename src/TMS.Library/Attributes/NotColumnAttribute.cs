using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Library.Attributes
{
    /// <summary>
    /// Những thuộc tính có attribute này sẽ không phải là một cột trong DB
    /// </summary>
    public class NotColumnAttribute : Attribute
    {
    }
}
