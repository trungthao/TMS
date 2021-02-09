using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TMS.Library.Attributes;

namespace TMS.Domain.Entities
{
    public class User : BaseEntity
    {
        [Key]
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public bool AcceptTerms { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }

        public DateTime? VerifiedDate { get; set; }

        public bool IsVerified => VerifiedDate.HasValue;

        [NotColumn]
        [JsonIgnore]
        public string Password { get; set; }

        public string VerificationToken { get; set; }

        [NotColumn]
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}
