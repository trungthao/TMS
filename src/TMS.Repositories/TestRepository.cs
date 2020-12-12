using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMS.Library.Interfaces;
using TMS.Domain.Entities;
using TMS.Domain.Repositories;

namespace TMS.Repositories
{
    public class TestRepository : BaseRepository, ITestRepository
    {
        public TestRepository(IDatabaseService databaseService) : base(databaseService)
        {
        }
        public async Task<IEnumerable<Test>> GetListTest()
        {
            return await _databaseService.GetListObjectAsync<Test>("SELECT * FROM `Test`;", null, null, null, System.Data.CommandType.Text);
        }
    }
}
