using TMS.API.Models.Accounts;

namespace TMS.Domain.Services
{
    public interface IAccountService : IBaseService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model, string ipAddress);
    }
}