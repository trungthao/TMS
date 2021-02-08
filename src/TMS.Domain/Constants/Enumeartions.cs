namespace TMS.Domain.Constants
{
    public class Enumeartions
    {
        public enum EntityState
        {
            None = 0,
            Insert = 1,
            Update = 2,
            Delete = 3
        }

        public enum ErrorCodeResponse
        {

        }

        public enum ResponseServiceStatusCode
        {
            Success = 200,
            Fail = 500
        }
    }
}
