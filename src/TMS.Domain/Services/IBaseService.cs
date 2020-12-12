using System.Threading.Tasks;
using TMS.Domain.Entities;
using TMS.Domain.Models;

namespace TMS.Domain.Services
{
    public interface IBaseService
    {
        Task<SaveBaseEntityResponse> SaveEntity(BaseEntity entity);
    }
}