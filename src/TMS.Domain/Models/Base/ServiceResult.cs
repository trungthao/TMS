using TMS.Domain.Constants;

namespace TMS.Domain.Models
{
    public class ServiceResult
    {
        public Enumeartions.ResponseServiceStatusCode Status { get; set; } = Enumeartions.ResponseServiceStatusCode.Success;

        public object Data { get; set; }

        public Enumeartions.ErrorCodeResponse ErrorCode { get; set; }
    }
}