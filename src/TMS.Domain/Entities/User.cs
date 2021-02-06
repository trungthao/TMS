using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TMS.Domain.Entities
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
