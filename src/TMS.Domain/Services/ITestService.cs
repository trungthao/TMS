using System.Collections.Generic;
using System.Threading.Tasks;
using TMS.Domain.Entities;

namespace TMS.Domain.Services
{
    public interface ITestService : IBaseService
    {
        Task<IEnumerable<Test>> GetListTest();
    }
}