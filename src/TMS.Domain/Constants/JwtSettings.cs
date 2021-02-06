using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain.Constants
{
    public class JwtSettings
    {
        public const string Jwt = "Jwt";

        public string SecretKey { get; set; }
        public int ExpireMinutes { get; set; }
        public int RefreshTokenExpireDays { get; set; }
    }
}
