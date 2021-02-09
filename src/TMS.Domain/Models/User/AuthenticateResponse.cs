using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TMS.Domain.Entities;

namespace TMS.Domain.Models
{
    public class AuthenticateResponse
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string JwtToken { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
