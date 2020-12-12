using TMS.Domain.Constants;

namespace TMS.Domain.Models
{
    public class BaseResponse
    {
        public int Status { get; set; }

        public object Data { get; set; }

        public Enumeartions.ErrorCodeResponse ErrorCode { get; set; }
    }
}