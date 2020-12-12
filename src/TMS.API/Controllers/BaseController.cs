using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TMS.Domain.Entities;
using TMS.Domain.Services;

namespace TMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        public Account Account => (Account)HttpContext.Items["Account"];
        protected readonly IMapper _mapper;
        protected readonly IBaseService _service;

        public BaseController(IMapper mapper, IBaseService service)
        {
            _mapper = mapper;
            _service = service;
        }
    }
}