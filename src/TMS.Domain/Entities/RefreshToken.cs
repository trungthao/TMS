using System;
using System.Text.Json.Serialization;

namespace TMS.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public DateTime CreatedDate { get; set; }

        public string CreatedByIp { get; set; }

        public DateTime? RevokedDate { get; set; }

        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }

        public bool IsActive => RevokedDate == null && !IsExpired;
    }
}