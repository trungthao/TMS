using System;
using System.Text.Json.Serialization;

namespace TMS.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        [JsonIgnore]
        public int RefreshTokenId { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpireDate;

        public DateTime CreatedDate { get; set; }

        public string CreatedByIp { get; set; }

        public DateTime? RevokedDate { get; set; }

        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }

        public bool IsActive => RevokedDate == null && !IsExpired;
    }
}