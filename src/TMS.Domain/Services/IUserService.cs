using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Domain.Models;

namespace TMS.Domain.Services
{
    public interface IUserService : IBaseService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest authModel, string ipAddress);
    }
}
