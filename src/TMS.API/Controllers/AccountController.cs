using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.API.Models.Accounts;
using TMS.Domain.Services;

namespace TMS.API.Controllers
{
    public class AccountController : BaseController
    {
        // private readonly IAccountService _accountService;
        // private readonly IMapper _mapper;

        // public AccountController(IAccountService accountService, IMapper mapper)
        // {
        //     _accountService = accountService;
        //     _mapper = mapper;
        // }

        // public ActionResult<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        // {
        //     var response = _accountService.Authenticate(model, IpAddress());
        //     SetTokenCookie(response.RefreshToken);

        //     return Ok(response);
        // }

        // private void SetTokenCookie(string token)
        // {
        //     var cookieOptions = new CookieOptions
        //     {
        //         HttpOnly = true,
        //         Expires = DateTime.UtcNow.AddDays(7)
        //     };

        //     Response.Cookies.Append("refreshToken", token, cookieOptions);
        // }

        // private string IpAddress()
        // {
        //     if (Request.Headers.ContainsKey("X-Forwarded-For")) 
        //     {
        //         return Request.Headers["X-Forwarded-For"];
        //     }
        //     else
        //     {
        //         return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        //     }
        // }
        public AccountController(IMapper mapper, IBaseService service) : base(mapper, service)
        {
        }
    }
}