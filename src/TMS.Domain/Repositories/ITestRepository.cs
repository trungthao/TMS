using System.Collections.Generic;
using System.Threading.Tasks;
using TMS.Domain.Entities;

namespace TMS.Domain.Repositories
{
    public interface ITestRepository : IBaseRepository
    {
        Task<IEnumerable<Test>> GetListTest();
    }
}