namespace TMS.Domain.Constants
{
    public class AppSettings
    {
        public JwtSettings JwtSettings { get; set; }
        public const string ConfigName = "AppSettings";

        public string EmailFrom { get; set; }

        public string SmtpHost { get; set; }

        public int SmtpPort { get; set; }

        public string SmtpUser { get; set; }

        public string SmtpPass { get; set; }

        public bool SmtpUseSsl { get; set; }
    }
}