using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMS.Domain.Entities;
using TMS.Domain.Repositories;
using TMS.Domain.Services;
using TMS.Library.Interfaces;

namespace TMS.Services
{
    public class TestService : BaseService, ITestService
    {
        private readonly ITestRepository _testRepository;
        public TestService(ITestRepository testRepository, IConfigService configService) : base(testRepository, configService)
        {
            _testRepository = testRepository;
        }

        public async Task<IEnumerable<Test>> GetListTest()
        {
            return await _testRepository.GetListTest();
        }
    }
}
