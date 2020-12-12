using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TMS.Domain.Entities;
using TMS.Domain.Models;
using TMS.Domain.Services;

namespace TMS.API.Controllers
{
    public class TestController : BaseController
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ITestService _testService;

        public TestController(IMapper mapper, ITestService testService) : base(mapper, testService)
        {
            _testService = testService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<Test>> Get()
        {
            return await _testService.GetListTest();
        }

        [HttpPost("")]
        public async Task<ActionResult<SaveTestResponse>> SaveTest(SaveTestRequest saveTestRequest)
        {
            var testEntity = _mapper.Map<Test>(saveTestRequest);
            var response = await _service.SaveEntity(testEntity);

            return (SaveTestResponse)response;
        }
    }
}
